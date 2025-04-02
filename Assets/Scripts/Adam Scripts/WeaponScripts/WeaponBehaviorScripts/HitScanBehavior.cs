using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanBehavior : IWeaponBehavior
{
    // raycast forward from shootpoint position, display object hit
    public void FireGun(Transform shootPoint)
    {
        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.forward, out hit))
        {
            Debug.Log($"Shot {hit.collider.name}. (raycast)");
        }
    }
}