using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousGun : GunBase
{
    //Constructor instantiates new ContinuousBehvaior then initializes ammoMax and ammoCount based on params
    public ContinuousGun(GameObject fireVisualPrefab, int initialAmmoMax, int initialAmmoCount, int damage)
    {
        weaponBehavior = new ContinuousBehavior(fireVisualPrefab);
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
            Debug.Log("Full on ammo for continuous gun");
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

    //Currently unused method to swap weapon's behavior
    /*
    public override void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        this.weaponBehavior = newBehavior;
    }
    */
}
