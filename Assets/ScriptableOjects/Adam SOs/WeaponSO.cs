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
        Continuous
    }
    public string weaponName;
    public WeaponType weaponType;
    public int ammoMax;
    public int ammoCount;
}
