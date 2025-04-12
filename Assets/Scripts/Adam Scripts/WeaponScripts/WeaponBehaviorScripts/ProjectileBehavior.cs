using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : IGunBehavior
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;

    // Instantiate projectile with projectileSpeed velocity (forward)
    public void FireGun(Transform shootPoint, int damage)
    {
        GameObject projectile = GameObject.Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.forward * projectileSpeed;
        projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        Debug.Log("Projectile launched.");
    }
}