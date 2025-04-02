using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base abstract class for a gun weapon
public abstract class WeaponBase
{
    public Transform shootPoint;
    public IWeaponBehavior weaponBehavior;
    public int ammoCount;
    public int ammoMax;

    public abstract void AmmoGet(int amount);
    public abstract void Use();

    //Currently unused method to swap weapon's behavior
        //public abstract void SetWeaponBehavior(IWeaponBehavior newBehavior);
}