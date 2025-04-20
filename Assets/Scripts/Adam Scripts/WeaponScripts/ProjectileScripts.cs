using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileScripts : MonoBehaviour
{
    public enum ProjectileType
    {
        GunProjectile,
        Fire,
        HitScan,
        Grenade,
        SmokeBomb
    }

    //custom inspectorize me
    public ProjectileType projectileType;
    public int projectileDamage;

    private Vector3 initialPos;
    private float fireDistance = 2.255f;//make this adjustable in inspector?

    private List<GameObject> inRangeColliders = new List<GameObject>();
    private SphereCollider sphereCollider;
    public float explodeRange;
    public float explodeTime;
    
    public GameObject explodeSphere;
    private float explodeDur = 0.2f;

    private float smokeDur = 5f;
    [SerializeField]private bool isSmokin = false;

    private void Start()
    {
        //does this make sense? will it still cause errors for projectiles without these comps?
        sphereCollider = GetComponent<SphereCollider>() ? GetComponent<SphereCollider>() : null;

        EnemyDeathManager.instance.onEnemyDeath += InRangeCleanup;

        switch (projectileType)
        {
            case ProjectileType.GunProjectile:
                sphereCollider.radius = explodeRange / 2;
                break;
            case ProjectileType.Grenade:
            case ProjectileType.SmokeBomb:
                sphereCollider.enabled = true;
                sphereCollider.radius = explodeRange / 2;
                StartCoroutine(ExplodeTimer());
                break;
            case ProjectileType.Fire:
                initialPos = transform.position;
                break;
            case ProjectileType.HitScan:
                StartCoroutine(HitScanVisualDestroyer());
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
        else if (projectileType != ProjectileType.Grenade 
            && projectileType != ProjectileType.SmokeBomb)
        {
            //'Collision' with any collider destroys projectile
            OnDealDamage(collision.gameObject);
            Destroy(gameObject);
        }
    }
    public void OnDealDamage(GameObject other)
    {
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerStatsScript>().TakeDamage(projectileDamage);
                break;
            case "Enemy":
                other.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
                break;
            case "EnvironEnemy":
                //if(other.GetComponent<EnvironEnemy>() is IDestructable destructable)
                other.GetComponent<BarrelScript>().OnTakeDamage(projectileDamage);
                break;
        }
        Destroy(gameObject);
    }

    #region ExplodingSphereStuff
    private void OnTriggerEnter(Collider other)
    {
        //adding enemyies/player to list if in range 
        if (projectileType == ProjectileType.GunProjectile 
            || projectileType == ProjectileType.Grenade)
        {
            if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "EnvironEnemy")
            {
                if (!inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Add(other.gameObject);
                }
            }
        }
        else if (projectileType == ProjectileType.SmokeBomb)
        {
            if (isSmokin && other.tag == "Player")
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
            if (other.tag == "Player" || other.tag == "Enemy" || other.tag == "EnvironEnemy")
            {
                if (inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Remove(other.gameObject);
                }
            }
        }
        else if (projectileType == ProjectileType.SmokeBomb)
        {
            if (isSmokin && other.tag == "Player")
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
            for (int i = 0; i < inRangeColliders.Count; i++)
            {
                switch (inRangeColliders[i].tag)
                {
                    case "Player":
                        PlayerStatsScript.instance.TakeDamage(projectileDamage);
                        break;
                    case "Enemy":
                        inRangeColliders[i]?.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
                        break;
                    case "EnvironEnemy":
                        inRangeColliders[i]?.GetComponent<BarrelScript>().OnTakeDamage(projectileDamage);
                        break;
                }
            }
            Destroy(gameObject);
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
        //InRangeCleanup();

        Vector3 initSphereScale = explodeSphere.transform.localScale;

        for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
        {
            float explodeLerpRatio = timer / explodeDur;

            explodeSphere.transform.localScale = Vector3.Lerp(initSphereScale, new Vector3(explodeRange, explodeRange, explodeRange), explodeLerpRatio);
            yield return null;
        }
        //clean up after lerp to avoid error? possible enemy dying during lerp causing null ref exception with list
        InRangeCleanup();
        OnExplode();
    }
    public IEnumerator SmokeCoro()
    {
        isSmokin = true;
        yield return new WaitForSecondsRealtime(smokeDur);
        isSmokin = false;
        PlayerStatsScript.instance.isHidden = false; //probably causes issues with multiple smoke areas when one disappears
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

    //HitScanWeaponMethod(s)
    #region HitScanVisualMethod
    // Coroutine fade, and then destroy spawned hitscan visual object
    public IEnumerator HitScanVisualDestroyer()
    {
        float fadeDur = 0.5f;
        Color mat = gameObject.GetComponent<Renderer>().material.color;

        for (float timer = 0f; timer < fadeDur; timer += Time.deltaTime)
        {
            float lerpRatio = timer / fadeDur;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(mat,new Color(1f, 1f, 1f, 0f), lerpRatio);
            yield return null;
        }
        Destroy(gameObject);
    }
    #endregion
}
