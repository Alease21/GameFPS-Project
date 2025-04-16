using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HitScanBehavior : IGunBehavior
{
    public GameObject hitScanShotPrefab;

    // raycast forward from shootpoint position and deal damage to hit target
    public void FireGun(Transform shootPoint, int damage)
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            switch (hit.transform.tag)
            {
                case "Player":
                    hit.transform.GetComponent<PlayerStatsScript>().TakeDamage(damage);
                    hit.transform.GetComponent<PlayerStatsScript>().UiStatUpdate?.Invoke();
                    break;
                case "Enemy":
                    hit.transform.GetComponent<EnemyScript>().TakeDamage(damage);
                    break;
                case "EnvironEnemy":
                    hit.transform.GetComponent<BarrelScript>().OnTakeDamage(damage);
                    break;
            }
            Debug.Log($"Shot {hit.transform.name}. (raycast)");
        }

        // rough visual for the hitscan attack (fades away after short time)
        GameObject hitScanShot = GameObject.Instantiate(hitScanShotPrefab, shootPoint);
    }
}