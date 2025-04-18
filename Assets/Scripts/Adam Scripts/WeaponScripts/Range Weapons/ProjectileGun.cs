using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : GunBase
{
    //Constructor instantiates new ProjectileBehavior then initializes ammoMax, ammoCount, and weaponDamage based on params
    public ProjectileGun(GameObject projectilePrefab, int initialAmmoMax, int initialAmmoCount, int damage)
    {
        weaponBehavior = new ProjectileBehavior { projectilePrefab = projectilePrefab };
        ammoMax = initialAmmoMax;
        ammoCount = initialAmmoCount;
        weaponDamage = damage;
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

    //If weapon has ammo, call FireGun() from weaponBehvaior, else display debug message
    public override void Use()
    {
        if (weaponBehavior != null)
        {
            if (ammoCount > 0)
            {
                weaponBehavior.FireGun(shootPoint, weaponDamage);
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

    //Currently unused method to swap weapon's behavior
    /*
    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        weaponBehavior = newBehavior;
    }
    */

}