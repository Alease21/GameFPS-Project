using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
{
    public Transform shootPoint;
    public IWeaponBehavior weaponBehavior;

    public void SetWeaponBehavior(IWeaponBehavior newBehavior)
    {
        weaponBehavior = newBehavior;
    }

    public void Use()
    {
        if (weaponBehavior != null)
        {
            weaponBehavior.FireGun(shootPoint);
        }
        else
        {
            Debug.Log("No weapon behavior set");
        }
    }
}