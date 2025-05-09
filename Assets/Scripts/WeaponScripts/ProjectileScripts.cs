using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Android;

public class ProjectileScripts : MonoBehaviour
{
    public enum ProjectileType
    {
        GunProjectile,
        Fire,
        Grenade,
        SmokeBomb
    }
    private AudioSource audioSource;
    private ParticleSystem projParticleSystem = null,
                           projParticleSystem2 = null;
    private Rigidbody rb;

    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }

        if (!GetComponent<AudioSource>())
        {
            transform.AddComponent<AudioSource>();
        }
    }
    //custom inspectorize me
    [SerializeField] private ProjectileType projectileType;
    [HideInInspector] public float projectileDamage;


    private Vector3 initialPos;
    public Vector3 InitialFirePos { get { return initialPos; } private set { initialPos = value; } }

    [HideInInspector] public float fireDistance;

    private List<GameObject> inRangeColliders = new List<GameObject>();
    private SphereCollider sphereCollider;
    [HideInInspector] public float explodeRange;
    [HideInInspector] public float explodeTime;
    
    private float explodeDur = 1f;

    [SerializeField] private float smokeDur = 5f;
    [SerializeField] private bool isSmokin = false;
    public bool IsSmokin { get { return isSmokin; } private set { isSmokin = value; } }

    //SaveLoad variables
    private float throwExplodeTime;
    private float smokeTime;
    private Vector3 velocity;
    public float ThrowExplodeTime { get { return throwExplodeTime; } private set { throwExplodeTime = value; } }
    public float SmokeTime { get { return smokeTime; } private set { smokeTime = value; } }
    public Vector3 Velocity { get { return velocity; } private set { velocity = value; } }
    public bool amSaved;

    private void Start()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>();
        projParticleSystem = GetComponentInChildren<ParticleSystem>();
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();

        SaveLoadControl.instance.saveGame += OnSaveGame;
        EnemyDeathManager.instance.onEnemyDeath += InRangeCleanup;

        switch (projectileType)
        {
            case ProjectileType.GunProjectile:
                audioSource.clip = SFX_Library.instance.explosion1;

                sphereCollider.radius = explodeRange / 2;
                break;
            case ProjectileType.Fire:
                //audioSource.clip = **add fire hit sound?**

                initialPos = transform.position;
                break;
            case ProjectileType.Grenade:
                audioSource.clip = SFX_Library.instance.explosion1;

                sphereCollider.enabled = true;
                sphereCollider.radius = explodeRange / 2;
                StartCoroutine(ExplodeTimer());
                break;
            case ProjectileType.SmokeBomb:
                projParticleSystem2 = transform.Find("GroundFog").GetComponent<ParticleSystem>();

                sphereCollider.enabled = true;
                StartCoroutine(ExplodeTimer());
                break;

        }
        //Debug.Log($"partiSys1: {projParticleSystem.gameObject.name}\npartiSys2: {projParticleSystem2.gameObject.name}");
    }
    private void Update()
    {
        // Destroy fire visual after a certain distance has been travelled
        // (relocate once projectile system is swapped to object pooling)
        if (projectileType == ProjectileType.Fire)
        {
            if ((initialPos - transform.position).magnitude >= fireDistance)
            {
                if (amSaved)
                {
                    gameObject.SetActive(false);
                }
                else
                {
                    GUIDRegistry.RemoveGUID(GetComponent<MyGUID>().GUID);
                    SaveLoadControl.instance.saveGame -= OnSaveGame;
                    Destroy(gameObject);
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (projectileType == ProjectileType.GunProjectile)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(ExplodeTimer());
        }
        else if (projectileType == ProjectileType.Fire)
        {
            OnDealDamage(collision.gameObject, true);

            if (amSaved)
            {
                gameObject.SetActive(false);
            }
            else
            {
                GUIDRegistry.RemoveGUID(GetComponent<MyGUID>().GUID);
                SaveLoadControl.instance.saveGame -= OnSaveGame;
                Destroy(gameObject);
            }
        }
        /*
        else if (projectileType != ProjectileType.Grenade
            && projectileType != ProjectileType.SmokeBomb)
        {  
            //delete me? am i unused?

            //'Collision' with any collider destroys projectile
            OnDealDamage(collision.gameObject);
            Destroy(gameObject);
        }
        */
    }
    public void OnDealDamage(GameObject other, bool useDOTDamage = false)
    {
        if (other.GetComponent<PlayerStatsScript>())
        {
            other.GetComponent<PlayerStatsScript>().TakeDamage(projectileDamage, useDOTDamage);
        }
        else if (other.GetComponent<EnemyScript>())
        {
            other.GetComponent<EnemyScript>().TakeDamage(projectileDamage, useDOTDamage);
        }
        else if (other.GetComponent<BarrelScript>())
        {
            other.GetComponent<BarrelScript>().OnTakeDamage(projectileDamage, useDOTDamage);
        }
    }

    #region ExplodingStuff
    private void OnTriggerEnter(Collider other)
    {
        //adding enemyies/player to list if in range 
        if (projectileType == ProjectileType.GunProjectile 
            || projectileType == ProjectileType.Grenade)
        {
            if (other.GetComponent<PlayerStatsScript>() || other.GetComponent<EnemyScript>() || other.GetComponent<BarrelScript>())
            {
                if (!inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Add(other.gameObject);
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (isSmokin)
        {
            if (other.GetComponent<PlayerStatsScript>())
            {
                PlayerStatsScript.instance.isHidden = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //remove enemies/player if out of range
        if (projectileType == ProjectileType.GunProjectile
            || projectileType == ProjectileType.Grenade)
        {
            if (other.GetComponent<PlayerStatsScript>() || other.GetComponent<EnemyScript>() || other.GetComponent<BarrelScript>())
            {
                if (inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Remove(other.gameObject);
                }
            }
        }
        else if (projectileType == ProjectileType.SmokeBomb)
        {
            if (isSmokin && other.GetComponent<PlayerStatsScript>())
            {
                PlayerStatsScript.instance.isHidden = false;
            }
        }
    }
    //General explode, used for throwables and for projectile gun
    public void OnExplode()
    {
        if (projectileType != ProjectileType.SmokeBomb)
        {
            projParticleSystem?.Play();

            for (int i = 0; i < inRangeColliders.Count; i++)
            {
                if (inRangeColliders[i] != null)
                {
                    OnDealDamage(inRangeColliders[i].gameObject, true);
                }
            }

            if (amSaved)
            {
                StartCoroutine(PlayAudioAfterDestroy.SoundAfterDisable(this.gameObject, audioSource.clip.length));
            }
            else
            {
                StartCoroutine(PlayAudioAfterDestroy.SoundAfterDestroy(this.gameObject, audioSource.clip.length));
                GUIDRegistry.RemoveGUID(GetComponent<MyGUID>().GUID);
                SaveLoadControl.instance.saveGame -= OnSaveGame;
            }
        }
        else
        {
            StartCoroutine(SmokeCoro());
        }
    }

    //coroutine to track throwable explode timer and then trigger explode
    public IEnumerator ExplodeTimer(float _timer = 0f)
    {
        
        //for loop to track timer for save/load
        for (float timer = _timer; timer < explodeTime; timer += Time.deltaTime)
        {
            ThrowExplodeTime = timer;
            yield return null;
        }

        if (audioSource.clip) //maybe delete me later
        {
            audioSource.Play();
        }
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        projParticleSystem?.gameObject.SetActive(true);
        if(projParticleSystem2 != null)
        {
            projParticleSystem2.gameObject.SetActive(true);
        }

        if (projectileType == ProjectileType.SmokeBomb)
        {
            projParticleSystem?.Play();//fix me to be better? or remove this extra thing
            projParticleSystem2?.Play();

            float initColliderRadius = sphereCollider.radius;
            float alpha = 0f;

            for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
            {
                float explodeLerpRatio = timer / explodeDur;

                //smoke color lerp
                alpha = Mathf.Lerp(0, 0.5f, explodeLerpRatio);//hard coded in alpha value for now
                projParticleSystem2.transform.GetComponent<Renderer>().material.color = new Color(projParticleSystem2.transform.GetComponent<Renderer>().material.color.r,
                    projParticleSystem2.transform.GetComponent<Renderer>().material.color.g,
                    projParticleSystem2.transform.GetComponent<Renderer>().material.color.b, alpha);

                //trigger sphere radius lerp
                sphereCollider.radius = Mathf.Lerp(initColliderRadius, explodeRange / 2, explodeLerpRatio);
                yield return null;
            }
        }

        InRangeCleanup();
        OnExplode();
    }
    public IEnumerator SmokeCoro(float _timer = 0f)
    {
        isSmokin = true;

        //for loop to track timer for save/load
        for (float timer = _timer; timer < (smokeDur - explodeDur); timer += Time.deltaTime)//initial smoke spawn done in OnExplode (above) so
                                                                                            // wait time is (smokeDur - explodeDur)
        {
            smokeTime = timer;
            yield return null;
        }

        float initColliderRadius = sphereCollider.radius;
        float alpha = 0f;

        for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
        {
            float fadeLerpRatio = timer / explodeDur;

            //smoke color fade
            alpha = Mathf.Lerp(0.5f, 0, fadeLerpRatio);//hard coded in alpha value for now
            projParticleSystem2.transform.GetComponent<Renderer>().material.color = new Color(projParticleSystem2.transform.GetComponent<Renderer>().material.color.r, 
                projParticleSystem2.transform.GetComponent<Renderer>().material.color.g, 
                projParticleSystem2.transform.GetComponent<Renderer>().material.color.b, alpha);

            //trigger sphere radius lerp
            sphereCollider.radius = Mathf.Lerp(initColliderRadius, 0f, fadeLerpRatio);
            yield return null;
        }
        if (amSaved)
        {
            gameObject.SetActive(false);
        }
        else
        {
            GUIDRegistry.RemoveGUID(GetComponent<MyGUID>().GUID);//maybe make me better or move to guid scripts
            SaveLoadControl.instance.saveGame -= OnSaveGame;

            Destroy(gameObject);
        }
    }

    // Check for destroyed/null elements in list, then delete if any are found
    public void InRangeCleanup()
    {
        for (int i = 0; i < inRangeColliders.Count; i++)
        {
            if (inRangeColliders[i] == null || inRangeColliders[i].IsDestroyed())
            {
                inRangeColliders.Remove(inRangeColliders[i]);
            }
        }
    }
    #endregion

    //run on save game instead, destroy disabled projectile obj if game saved 
    public void OnSaveGame()
    {
        if (gameObject != null)
        {
            velocity = rb.velocity;
            if (!gameObject.activeInHierarchy)
            {
                SaveLoadControl.instance.saveGame -= OnSaveGame;
                GUIDRegistry.RemoveGUID(GetComponent<MyGUID>().GUID);//maybe make me better or move to guid scripts
                Destroy(gameObject);
            }
            amSaved = true;
        }
    }

    #region SaveLoadMethods
    public void OnLoadGameData(float[] fArray, Vector3[] vArray, bool _isSmokin)
    {
        StopAllCoroutines();

        projParticleSystem?.gameObject.SetActive(false);
        if (projParticleSystem2 != null)
        {
            projParticleSystem2?.gameObject.SetActive(false);
        }

        isSmokin = _isSmokin;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = vArray[0];

        switch (projectileType)
        {
            case ProjectileType.GunProjectile:
                PlayAudioAfterDestroy.EnableVisualOnGameLoad(gameObject);
                break;
            case ProjectileType.Fire:
                InitialFirePos = vArray[1];
                break;
            case ProjectileType.Grenade:
                PlayAudioAfterDestroy.EnableVisualOnGameLoad(gameObject);
                StartCoroutine(ExplodeTimer(fArray[0]));
                break;
            case ProjectileType.SmokeBomb:
                if (_isSmokin)
                {
                    projParticleSystem?.gameObject.SetActive(true);
                    projParticleSystem2?.gameObject.SetActive(true);
                    projParticleSystem?.Play();
                    projParticleSystem2.transform.GetComponent<Renderer>().material.color += new Color(0f, 0f, 0f, 0.5f);//bandaid fix for smoke starting at 0 alpha when loaded,
                                                                                                                         //maybe track alpha value on save
                    projParticleSystem2?.Play();
                    StartCoroutine(SmokeCoro(fArray[1]));
                }
                else
                {
                    StartCoroutine(ExplodeTimer(fArray[0]));
                }
                break;
        }
        //run in range checker?
    }

    #endregion
}
