using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeBombThrowable : ThrowableBase
{
    public int damage;
    public float timer;
    public float range;

    public SmokeBombThrowable(WeaponSO weaponSO)
    {
        throwablePrefab = weaponSO.weaponPrefab;
        throwableCount = weaponSO.ammoCount;
        throwableMax = weaponSO.ammoMax;
        range = weaponSO.range;
        timer = weaponSO.explodeTime;
        throwableSpeed = weaponSO.projectileSpeed;
    }
    public override bool CountGet(int amount)
    {
        if (throwableCount < throwableMax)
        {
            if (throwableCount + amount >= throwableMax)
            {
                throwableCount = throwableMax;
            }
            else
            {
                throwableCount += amount;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void Use(Transform throwPoint)
    {
        GameObject projectile = GameObject.Instantiate(throwablePrefab, throwPoint.position, Quaternion.LookRotation(throwPoint.transform.forward));
        projectile.GetComponent<Rigidbody>().velocity = throwPoint.forward * throwableSpeed;
        //projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        projectile.GetComponent<ProjectileScripts>().explodeRange = range;
        projectile.GetComponent<ProjectileScripts>().explodeTime = timer;

        throwableCount--;
    }
}
