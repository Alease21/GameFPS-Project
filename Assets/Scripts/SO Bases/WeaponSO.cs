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
        Melee,
        Grenade,
        SmokeBomb
    }
    //editor script to show relevant vars depending on weapon type? (no ammo for melee)
    public string weaponName;
    public WeaponType weaponType;
    public int ammoMax;
    public int ammoCount;
    public int damage;
    public float range;
    public GameObject weaponPrefab;
    public AnimationClip attackAnimation;

    public GameObject hitBoxPrefab;
    public float explodeTime;
    public GameObject projectilePrefab;
    public float projectileSpeed;
}
public class HitScanWeapon : WeaponSO
{

}
public class ProjectileWeapon : WeaponSO
{
    //public float explodeTime;
    public float projectileSpeed;
    public GameObject projectilePrefab;
}
public class ContinuousWeapon : WeaponSO
{
    public float projectileSpeed;
    public GameObject projectilePrefab;
    // tick rate
}

// throwables are added w/ a throwables class that pulls info from these SOs
public class Grenade : WeaponSO
{
    public float explodeTime;//time until explode
    public float projectileSpeed;
    public GameObject projectilePrefab;
    //
}
public class SmokeBomb : WeaponSO
{
    public float explodeTime; //time until explode
    public float projectileSpeed;
    public GameObject projectilePrefab;
    //smoke dur
    //smokin bool?
}