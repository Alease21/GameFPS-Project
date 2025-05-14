using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileGun : GunBase
{
    private float explodeRange;

    //Constructor instantiates new ProjectileBehavior then initializes ammoMax, ammoCount, and weaponDamage based on params
    public ProjectileGun(WeaponSO weaponSO)
    {
        weaponBehavior = new ProjectileBehavior(weaponSO.projectilePrefab, weaponSO.projectileSpeed);
        ammoMax = weaponSO.ammoMax;
        ammoCount = weaponSO.ammoCount;
        weaponDamage = weaponSO.damage;
        explodeRange = weaponSO.range;
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
            //Debug.Log("Full on ammo for projectile gun");
        }
    }

    //If weapon has ammo, call FireGun() from weaponBehvaior, else display debug message
    public override void Use()
    {
        if (weaponBehavior != null)
        {
            if (ammoCount > 0)
            {
                weaponBehavior.FireGun(shootPoint, weaponDamage, explodeRange);
                ammoCount--;
            }
            else
            {
                //Debug.Log("Out of ammo in projectile gun");
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
        weaponBehavior = newBehavior;
    }
    */

}