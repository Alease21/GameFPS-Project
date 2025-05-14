using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : IGunBehavior
{
    public GameObject projectilePrefab;
    public float projectileSpeed;

    public ProjectileBehavior(GameObject projectilePrefab, float projectileSpeed)
    {
        this.projectilePrefab = projectilePrefab;
        this.projectileSpeed = projectileSpeed;
    }

    // Instantiate projectile with projectileSpeed velocity (forward)
    public void FireGun(Transform shootPoint, float damage, float range)
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.forward * projectileSpeed;
        projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        projectile.GetComponent<ProjectileScripts>().explodeRange = range;
    }
}