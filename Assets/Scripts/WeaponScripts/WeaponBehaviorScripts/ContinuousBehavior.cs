using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBehavior : IGunBehavior
{
    public GameObject fireVisualPrefab;
    public float fireSpeed;
    public ContinuousBehavior(GameObject projectilePrefab, float projectileSpeed)
    {
        fireVisualPrefab = projectilePrefab;
        fireSpeed = projectileSpeed;
    }

    public void FireGun(Transform shootPoint, float damage, float range)
    {
        GameObject projectile = GameObject.Instantiate(fireVisualPrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.forward * fireSpeed;
        projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        projectile.GetComponent<ProjectileScripts>().fireDistance = range;
        //Debug.Log("im firing some fire");
    }
}
