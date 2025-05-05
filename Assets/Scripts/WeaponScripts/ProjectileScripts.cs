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
    private ParticleSystem projParticleSystem,
        projParticleSystem2;
    private void OnEnable()
    {
        if (!GetComponent<AudioSource>())
        {
            transform.AddComponent<AudioSource>();
        }
    }
    //custom inspectorize me
    [SerializeField] private ProjectileType projectileType;
    [HideInInspector] public float projectileDamage;


    private Vector3 initialPos;
    private float fireDistance = 2.255f;//make this adjustable in inspector?

    private List<GameObject> inRangeColliders = new List<GameObject>();
    private SphereCollider sphereCollider;
    [HideInInspector] public float explodeRange;
    [HideInInspector] public float explodeTime;
    
    [SerializeField] private GameObject explodeSphere;
    private float explodeDur = 1f;

    [SerializeField] private float smokeDur = 5f;
    [SerializeField] private bool isSmokin = false;

    private void Start()
    {
        sphereCollider = GetComponentInChildren<SphereCollider>() ? GetComponentInChildren<SphereCollider>() : null;
        projParticleSystem = GetComponentInChildren<ParticleSystem>() ? GetComponentInChildren<ParticleSystem>() : null;
        audioSource = GetComponent<AudioSource>();

        EnemyDeathManager.instance.onEnemyDeath += InRangeCleanup;

        switch (projectileType)
        {
            case ProjectileType.GunProjectile:
                audioSource.clip = SFX_Library.instance.explosion1;

                sphereCollider.radius = explodeRange / 2;
                break;
            case ProjectileType.Fire:
                //audioSource.clip = **add fire hit sound?**
                projParticleSystem?.Play();

                initialPos = transform.position;
                break;
            case ProjectileType.Grenade:
                audioSource.clip = SFX_Library.instance.explosion1;

                sphereCollider.enabled = true;
                sphereCollider.radius = explodeRange / 2;
                StartCoroutine(ExplodeTimer());
                break;
            case ProjectileType.SmokeBomb:
                //audioSource.clip = 
                projParticleSystem2 = transform.Find("GroundFog").GetComponent<ParticleSystem>();

                sphereCollider.enabled = true;
                StartCoroutine(ExplodeTimer());
                break;
        }
    }
    private void Update()
    {
        // Destroy fire visual after a certain distance has been travelled
        // (relocate once projectile system is swapped to object pooling)
        if (projectileType == ProjectileType.Fire)
        {
            Vector3 distanceTravelled = (initialPos - transform.position);

            if (distanceTravelled.magnitude >= fireDistance)
            {
                Destroy(gameObject);
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
            Destroy(gameObject);
        }
        else if (projectileType != ProjectileType.Grenade
            && projectileType != ProjectileType.SmokeBomb)
        {  
            //delete me? am i unused?

            //'Collision' with any collider destroys projectile
            OnDealDamage(collision.gameObject);
            Destroy(gameObject);
        }
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

    #region ExplodingSphereStuff
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
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

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

            StartCoroutine(PlayAudioAfterDestroy.SoundAfterDestroy(this.gameObject, audioSource.clip.length));
        }
        else
        {
            StartCoroutine(SmokeCoro());
        }
    }

    //coroutine to track throwable explode timer and then trigger explode
    public IEnumerator ExplodeTimer()
    {
        yield return new WaitForSecondsRealtime(explodeTime);
        if (audioSource.clip) //maybe delete me later
        {
            audioSource.Play();
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
    public IEnumerator SmokeCoro()
    {
        isSmokin = true;
        yield return new WaitForSecondsRealtime(smokeDur - explodeDur);

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

        /*
        float initColliderRadius = sphereCollider.radius;
        Color smokeSphereColor = explodeSphere.GetComponent<Renderer>().material.color;

        //Collider radius lerp to fix enter/exit issues, color fade out
        for (float timer = 0f; timer < 1f;  timer += Time.deltaTime)
        {                           //hard coded in value for smoke fade duration
            float smokeFadeRatio = timer / 1f;

            explodeSphere.GetComponent<Renderer>().material.color = Color.Lerp(smokeSphereColor, smokeSphereColor * new Color(1f,1f,1f,0f), smokeFadeRatio);
            sphereCollider.radius = Mathf.Lerp(initColliderRadius, 0f, smokeFadeRatio);
            yield return null;
        }
        */
        Destroy(gameObject);
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
}
