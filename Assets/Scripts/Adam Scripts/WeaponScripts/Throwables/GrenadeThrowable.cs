using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrowable : ThrowableBase
{
    public int damage;
    public float timer;
    public float range;

    public GrenadeThrowable(GameObject throwablePrefab, int throwableCount, int throwableMax, int damage, float range, float timer)
    {
        this.throwablePrefab = throwablePrefab;
        this.throwableCount = throwableCount;
        this.throwableMax = throwableMax;
        this.damage = damage;
        this.range = range;
        this.timer = timer;
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
        GameObject projectile = GameObject.Instantiate(throwablePrefab, throwPoint.position, Quaternion.LookRotation(throwPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = throwPoint.forward * throwableSpeed;
        projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        projectile.GetComponent<ProjectileScripts>().explodeRange = range;
        projectile.GetComponent<ProjectileScripts>().explodeTime = timer;

        //ammo-- add
    }
}
