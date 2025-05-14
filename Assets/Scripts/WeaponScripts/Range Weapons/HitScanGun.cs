using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitScanGun : GunBase
{
    //Constructor instantiates new HitScanBehavior then initializes ammoMax and ammoCount based on params
    public HitScanGun(WeaponSO weaponSO)
    {
        weaponBehavior = new HitScanBehavior();
        ammoMax = weaponSO.ammoMax;
        ammoCount = weaponSO.ammoCount;
        weaponDamage = weaponSO.damage;
    }
    public HitScanGun(WeaponSO weaponSO, int cheatAmmo)
    {
        weaponBehavior = new HitScanBehavior();
        ammoMax = cheatAmmo;
        ammoCount = cheatAmmo;
        weaponDamage = weaponSO.damage;
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
            //Debug.Log("Full on ammo for hitscan gun");
        }
    }

    //If weapon has ammo, call FireGun() from weaponBehvaior, else display debug message
    public override void Use()
    {
        if (weaponBehavior != null)
        {
            if (ammoCount > 0)
            {
                weaponBehavior.FireGun(shootPoint, weaponDamage, 0f);//filler range for now (unused value)
                ammoCount--;
            }
            else
            {
                //Debug.Log("Out of ammo in hitscan gun");
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