using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBehavior : IGunBehavior
{
    public GameObject fireVisualPrefab;
    public float fireSpeed = 10f;

    public void FireGun(Transform shootPoint, int damage)
    {
        GameObject projectile = GameObject.Instantiate(fireVisualPrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.forward * fireSpeed;
        projectile.GetComponent<ProjectileScripts>().projectileDamage = damage;
        //Debug.Log("im firing some fire");
    }
}
