using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBehavior : IWeaponBehavior
{
    public GameObject firePrefab;
    public void FireGun(Transform shootPoint)
    {
        // fix me, im a mess. give me a parent and destroy timer
        //GameObject fireEffect = GameObject.Instantiate(firePrefab, shootPoint.position, Quaternion.LookRotation(shootPoint.transform.up));
        Debug.Log("im firing some fire, trust me");
    }
}
