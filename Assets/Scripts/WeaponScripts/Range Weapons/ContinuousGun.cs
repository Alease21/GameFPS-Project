using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousGun : GunBase
{
    float fireRange;

    //Constructor instantiates new ContinuousBehvaior then initializes ammoMax and ammoCount based on params
    public ContinuousGun(WeaponSO weaponSO)
    {
        weaponBehavior = new ContinuousBehavior(weaponSO.projectilePrefab, weaponSO.projectileSpeed);
        ammoMax = weaponSO.ammoMax;
        ammoCount = weaponSO.ammoCount;
        weaponDamage = weaponSO.damage;
        fireRange = weaponSO.range;
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
            //Debug.Log("Full on ammo for continuous gun");
        }
    }

    //If weapon has ammo, call FireGun() from weaponBehvaior, else display debug message
    public override void Use()
    {
        if (weaponBehavior != null)
        {
            if (ammoCount > 0)
            {
                weaponBehavior.FireGun(shootPoint, weaponDamage, fireRange);
            }
            else
            {
                //Debug.Log("Out of ammo in continuous gun");
            }
        }
        else
        {
            //Debug.Log("No weapon behavior set");
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
