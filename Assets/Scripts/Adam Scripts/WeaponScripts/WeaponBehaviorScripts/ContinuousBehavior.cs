using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuousBehavior : IWeaponBehavior
{
    public GameObject firePrefab;
    public void FireGun(Transform shootPoint)
    {
        //Visual currently unimplemented
        Debug.Log("im firing some fire, trust me");
    }
}
