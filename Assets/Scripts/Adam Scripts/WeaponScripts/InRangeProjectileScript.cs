using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProjectileScripts;

public class InRangeProjectileScript : MonoBehaviour
{
    /*
    private ProjectileScripts projectileScripts;

    void Start()
    {
        projectileScripts = GetComponentInParent<ProjectileScripts>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //adding enemyies/player to list if in range 
        if (projectileScripts.projectileType == ProjectileType.ThrowProjectile)
        {
            if (other.tag == "Player" || other.tag == "Enemy")
            {
                if (!inRangeColliders.Contains(other.gameObject))
                {
                    inRangeColliders.Add(other.gameObject);
                }
            }
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
    */
}
