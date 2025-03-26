using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "NewItem/NewWeapon")]
public class WeaponSO : ScriptableObject
{
    public enum WeaponType
    {
        HitScan,
        Projectile
    }
    public string weaponName;
    public WeaponType weaponType;
    public string weaponDescription;
}
