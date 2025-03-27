using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase
{
    public Transform shootPoint;
    public IWeaponBehavior weaponBehavior;
    public int ammoCount;
    public int ammoMax;

    public abstract void AmmoGet(int amount);
    public abstract void SetWeaponBehavior(IWeaponBehavior newBehavior);
    public abstract void Use();
}