using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : WeaponBase
{
    public ProjectileGun(GameObject projectilePrefab, int initialAmmoMax, int initialAmmoCount)
    {
        weaponBehavior = new ProjectileBehavior { projectilePrefab = projectilePrefab };
        ammoMax = initialAmmoMax;
        ammoCount = initialAmmoCount;
    }

    public override void AmmoGet(int amount)
    {
        if (ammoCount <= ammoMax)
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

    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        weaponBehavior = newBehavior;
    }

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