using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitScanBehavior : IGunBehavior
{
    public int hitScanDamage;
    public GameObject hitScanShotPrefab;

    // raycast forward from shootpoint position, display object hit
    public void FireGun(Transform shootPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            switch (hit.transform.tag)
            {
                case "Player":
                    hit.transform.GetComponent<PlayerStatsScript>().TakeDamage(hitScanDamage);
                    break;
                case "Enemy":
                    hit.transform.GetComponent<EnemyScript>().TakeDamage(hitScanDamage);
                    break;
                case "EnvironEnemy":
                    hit.transform.GetComponent<BarrelScript>().OnTakeDamage(hitScanDamage);
                    break;
            }
            Debug.Log($"Shot {hit.transform.name}. (raycast)");
        }

        GameObject hitScanShot = GameObject.Instantiate(hitScanShotPrefab, shootPoint);
    }
}