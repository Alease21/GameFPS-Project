using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : WeaponBase
{
    //Constructor instantiates new ProjectileBehavior then initializes ammoMax and ammoCount based on params
    public ProjectileGun(GameObject projectilePrefab, int initialAmmoMax, int initialAmmoCount)
    {
        weaponBehavior = new ProjectileBehavior { projectilePrefab = projectilePrefab };
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
            Debug.Log("Full on ammo for projectile gun");
        }
    }

    //Currently unused method to swap weapon's behavior
    /*
    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        weaponBehavior = newBehavior;
    }
    */

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
                Debug.Log("Out of ammo in projectile gun");
            }
        }
        else
        {
            Debug.Log("No weapon behavior set");
        }
    }
}