using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBase
{
    public Gun(IWeaponBehavior weaponBehavior)
    {
        this.weaponBehavior = weaponBehavior;
    }
}