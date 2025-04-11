using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanGun : WeaponBase
{
    //Constructor instantiates new HitScanBehavior then initializes ammoMax and ammoCount based on params
    public HitScanGun(GameObject hitScanShotPrefab, int initialAmmoMax, int initialAmmoCount, int damage)
    {
        weaponBehavior = new HitScanBehavior { hitScanDamage = damage, hitScanShotPrefab = hitScanShotPrefab};
        ammoMax = initialAmmoMax;
        ammoCount = initialAmmoCount;
    }

    //Checks ammo count against ammo max and decides if ammo should be added
    //or to display debug message
    public override void AmmoGet(int amount)
    {
        if (ammoCount < ammoMax)
        {
            if (amount >= (ammoMax - ammoCount))
            {
                ammoCount = ammoMax;
            }
            else
            {
                ammoCount += amount;
            }
        }
        else
        {
            Debug.Log("Full on ammo for hitscan gun");
        }
    }

    //If weapon has ammo, call FireGun() from weaponBehvaior, else display debug message
    public override void Use()
    {
        if (weaponBehavior != null)
        {
            if (ammoCount > 0)
            {
                weaponBehavior.FireGun(shootPoint);
                ammoCount--;
            }
            else
            {
                Debug.Log("Out of ammo in hitscan gun");
            }
        }
        else
        {
            Debug.Log("No weapon behavior set");
        }
    }

    //Currently unused method to swap weapon's behavior
    /*
    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        weaponBehavior = newBehavior;
    }
    */
}