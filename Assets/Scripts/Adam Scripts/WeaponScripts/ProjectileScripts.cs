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
        ThrowProjectile
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
    private void Start()
    {
        //does this make sense? will it still cause errors for projectiles without these comps?
        sphereCollider = GetComponent<SphereCollider>() ? GetComponent<SphereCollider>() : null;

        EnemyDeathManager.instance.onEnemyDeath += InRangeCleanup;

        switch (projectileType)
        {
            case ProjectileType.GunProjectile:
                //sphereCollider = GetComponent<SphereCollider>();
                //sphereCollider.radius = explodeRange;
                break;
            case ProjectileType.ThrowProjectile:
                sphereCollider.enabled = true;
                sphereCollider.radius = explodeRange;
                StartCoroutine(ThrowExplodeTimer());
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

    private void OnTriggerEnter(Collider other)
    {
        //adding enemyies/player to list if in range 
        if (projectileType == ProjectileType.ThrowProjectile)
        {
            if (other.tag == "Player" || other.tag == "Enemy")
            {
                if (!inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Add(other.gameObject);
                }
            }
        }
        else
        {
            //'Collision' with any collider destroys projectile
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
            //Debug.Log($"{projectileType} projectile collided with {other.gameObject.name}. {projectileDamage} damage done.)");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //remove enemies/player if out of range
        if (projectileType == ProjectileType.ThrowProjectile)
        {
            if (other.tag == "Player" || other.tag == "Enemy")
            {
                if (inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Remove(other.gameObject);
                }
            }
        }
    }
    //General explode, used for throwables and possibly for projectile gun as well
    public void OnExplode()
    {
        for (int i = 0; i < inRangeColliders.Count; i++)
        {
            switch (inRangeColliders[i].tag)
            {
                case "Player":
                    inRangeColliders[i]?.GetComponent<PlayerStatsScript>().TakeDamage(projectileDamage);
                    break;
                case "Enemy":
                    inRangeColliders[i]?.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
                    break;
            }
        }
        Destroy(gameObject);
    }

    //ThrowableProjectileMethods
    #region ThrowableProjectileMethods
    //coroutine to track throwable explode timer and then trigger explode
    public IEnumerator ThrowExplodeTimer()
    {
        yield return new WaitForSecondsRealtime(explodeTime);
        InRangeCleanup();

        Vector3 initSphereScale = explodeSphere.transform.localScale;

        for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
        {
            float explodeLerpRatio = timer / explodeDur;

            explodeSphere.transform.localScale = Vector3.Lerp(initSphereScale, new Vector3(explodeRange, explodeRange, explodeRange), explodeLerpRatio);
            yield return null;
        }

        OnExplode();
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
