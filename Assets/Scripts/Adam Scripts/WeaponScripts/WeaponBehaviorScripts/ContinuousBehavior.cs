using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBehavior : IWeaponBehavior
{
    public GameObject fireVisualPrefab;
    public float fireSpeed = 10f;

    public ContinuousBehavior(GameObject fireVisualPrefab)
    {
        this.fireVisualPrefab = fireVisualPrefab;
    }

    public void FireGun(Transform shootPoint)
    {
        GameObject projectile = GameObject.Instantiate(fireVisualPrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        projectile.GetComponent<Rigidbody>().velocity = shootPoint.forward * fireSpeed;

        //Debug.Log("im firing some fire");
    }
}
