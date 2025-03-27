using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousGun : WeaponBase
{
    public ContinuousGun(GameObject firePrefab, int initialAmmoMax, int initialAmmoCount)
    {
        weaponBehavior = new ContinuousBehavior { firePrefab = firePrefab };
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
            Debug.Log("Full on ammo for continuous gun");
        }
    }

    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        this.weaponBehavior = newBehavior;
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
                Debug.Log("Out of ammo in continuous gun");
            }
        }
        else
        {
            Debug.Log("No weapon behavior set");
        }
    }
}
