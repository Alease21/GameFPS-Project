using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "NewItem/NewWeapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType
    {
        HitScan,
        Projectile,
        Continuous,
        Melee
    }
    //editor script to show relevant vars depending on weapon type? (no ammo for melee)
    public string weaponName;
    public WeaponType weaponType;
    public int ammoMax;
    public int ammoCount;
    public int damage;
    public float range;
}
