using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;

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
    public float projectileDamage;

    private Vector3 initialPos;
    private float fireDistance = 2.255f;//make this adjustable in inspector?

    private List<GameObject> inRangeColliders = new List<GameObject>();
    private SphereCollider sphereCollider;
    public float explodeRange;
    public float explodeTime;
    
    [SerializeField] private GameObject explodeSphere;
    private float explodeDur = 0.2f;

    [SerializeField] private float smokeDur = 5f;
    [SerializeField] private bool isSmokin = false;

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
                sphereCollider.enabled = true;
                sphereCollider.radius = explodeRange / 2;
                StartCoroutine(ExplodeTimer());
                break;
            case ProjectileType.SmokeBomb:
                sphereCollider.enabled = true;
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
        else if (projectileType == ProjectileType.Fire)
        {
            OnDealDamage(collision.gameObject, true);
            Destroy(gameObject);
        }
        else if (projectileType != ProjectileType.Grenade
            && projectileType != ProjectileType.SmokeBomb)
        {
            //'Collision' with any collider destroys projectile
            OnDealDamage(collision.gameObject);
            Destroy(gameObject);
        }
    }
    public void OnDealDamage(GameObject other, bool useDOTDamage = false)
    {
        if (other.GetComponent<PlayerStatsScript>())
        {
            other.GetComponent<PlayerStatsScript>().TakeDamage(projectileDamage);
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
        if (projectileType != ProjectileType.SmokeBomb)
        {
            for (int i = 0; i < inRangeColliders.Count; i++)
            {
                if (inRangeColliders[i] != null)
                {
                    if (inRangeColliders[i]?.GetComponent<PlayerStatsScript>())
                    {
                        PlayerStatsScript.instance.TakeDamage(projectileDamage, true);
                    }
                    else if (inRangeColliders[i]?.GetComponent<EnemyScript>())
                    {
                        inRangeColliders[i]?.GetComponent<EnemyScript>().TakeDamage(projectileDamage, true);
                    }
                    else if (inRangeColliders[i]?.GetComponent<BarrelScript>())
                    {
                        inRangeColliders[i]?.GetComponent<BarrelScript>().OnTakeDamage(projectileDamage, true);
                    }
                }
            }
            Destroy(gameObject);
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            StartCoroutine(SmokeCoro());
        }
    }

    //coroutine to track throwable explode timer and then trigger explode
    public IEnumerator ExplodeTimer()
    {
        yield return new WaitForSecondsRealtime(explodeTime);

        Vector3 initSphereScale = explodeSphere.transform.localScale;
        float initColliderRadius = sphereCollider.radius;
        Color smokeSphereColor = explodeSphere.GetComponent<Renderer>().material.color;

        for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
        {
            float explodeLerpRatio = timer / explodeDur;

            explodeSphere.transform.localScale = Vector3.Lerp(initSphereScale, new Vector3(explodeRange, explodeRange, explodeRange), explodeLerpRatio);
            
            //smoke bomb specific stuff: lerping collider radius to fix enter/exit issues & color fade in
            if (projectileType == ProjectileType.SmokeBomb)
            {
                explodeSphere.GetComponent<Renderer>().material.color = Color.Lerp(smokeSphereColor, smokeSphereColor + new Color(0f,0f,0f,0.7f), explodeLerpRatio);
                sphereCollider.radius = Mathf.Lerp(initColliderRadius, explodeRange / 2, explodeLerpRatio);
            }
            yield return null;
        }

        InRangeCleanup();
        OnExplode();
    }
    public IEnumerator SmokeCoro()
    {
        isSmokin = true;
        yield return new WaitForSecondsRealtime(smokeDur);

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
