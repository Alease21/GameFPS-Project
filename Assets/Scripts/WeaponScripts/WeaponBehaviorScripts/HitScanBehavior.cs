using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitScanBehavior : IGunBehavior
{
    public GameObject hitScanShotPrefab;
    public HitScanBehavior()
    {

    }
    // raycast forward from shootpoint position and deal damage to hit target
    public void FireGun(Transform shootPoint, float damage, float range)
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            if (hit.transform.GetComponent<PlayerStatsScript>())
            {
                hit.transform.GetComponent<PlayerStatsScript>().TakeDamage(damage);
            }
            else if (hit.transform.GetComponent<EnemyScript>())
            {
                hit.transform.GetComponent<EnemyScript>().TakeDamage(damage);
            }
            else if (hit.transform.GetComponent<BarrelScript>())
            {
                hit.transform.GetComponent<BarrelScript>().OnTakeDamage(damage);
            }
            //Debug.Log($"Shot {hit.transform.name}. (raycast)");
        }
    }
}