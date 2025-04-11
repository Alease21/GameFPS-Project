using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponBase myWeapon;
    public WeaponBase weapon1,
                      weapon2,
                      weapon3;
    public MeleeBase weapon4;// fix all of this later into 1?

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    [SerializeField] private Transform meleeSetPoint;

    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon,
                       meleeWeapon;
    [SerializeField] private GameObject[] weaponPrefabs;//change to grab from weaponSO with projectile/fire prefabs?
    [SerializeField] private GameObject currWeapon;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject fireVisualPrefab;
    [SerializeField] private GameObject hitScanShotPrefab;

    [SerializeField] private float continuousTickRate;//possible to grab from weaponSO upon item pickup?
    public bool isHoldingFire = false;
    //set up to never flip false after true, could change for dropping weapons in future.
    public bool hasHitScan = false,
                hasProjectile = false,
                hasContinuous = false;

    private void Start()
    {
        myWeapon = weapon1;
    }
    //Initial gun object and gameobject instantiation based on weaponType param. New gun instantiated with params initialAmmoMax and initialAmmoCount
    //Immediately swap currWeapon/myWeapon to new instantiated gameobject/gun and update ammo.
    public void WeaponPrefabSpawn(WeaponSO.WeaponType weaponType, int initialAmmoMax, int initialAmmoCount, int damage)
    {
        if (currWeapon != null)
        {
            currWeapon.SetActive(false);
        }

        //Based on weaponType, check if player has that weapon (bool), if false then instantiate gameobject and gun object and flip appropriate bool true
        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                if (!hasHitScan)
                {
                    hitScanWeapon = GameObject.Instantiate(weaponPrefabs[0], gunSetPoint.position, gunSetPoint.transform.rotation);
                    hitScanWeapon.transform.parent = gunSetPoint.transform;
                    currWeapon = hitScanWeapon;

                    weapon1 = new HitScanGun(hitScanShotPrefab, initialAmmoMax, initialAmmoCount, damage);
                    weapon1.shootPoint = shootPoint;
                    myWeapon = weapon1;

                    hasHitScan = true;
                    Debug.Log("Enemy Hitscan weapon instantiated");
                }
                break;

            case WeaponSO.WeaponType.Projectile:
                if (!hasProjectile)
                {
                    projectileWeapon = GameObject.Instantiate(weaponPrefabs[1], gunSetPoint.position, gunSetPoint.transform.rotation);
                    projectileWeapon.transform.parent = transform;
                    currWeapon = projectileWeapon;

                    weapon2 = new ProjectileGun(projectilePreFab, initialAmmoMax, initialAmmoCount);
                    weapon2.shootPoint = shootPoint;
                    myWeapon = weapon2;

                    hasProjectile = true;
                    Debug.Log("Enemy projectile weapon instantiated");
                }
                break;
            case WeaponSO.WeaponType.Continuous:
                if (!hasContinuous)
                {
                    continuousWeapon = GameObject.Instantiate(weaponPrefabs[2], gunSetPoint.position, gunSetPoint.transform.rotation);
                    continuousWeapon.transform.parent = transform;
                    currWeapon = continuousWeapon;

                    //StartCoroutine(ContinuousWeaponFire());

                    weapon3 = new ContinuousGun(fireVisualPrefab, initialAmmoMax, initialAmmoCount);
                    weapon3.shootPoint = shootPoint;
                    myWeapon = weapon3;

                    hasContinuous = true;
                    Debug.Log("Enemy continuous weapon instantiated");
                }
                break;
        }
    }
    public void WeaponPrefabSpawn(WeaponSO.WeaponType weaponType, float range)
    {
        switch (weaponType)
        {
            case WeaponSO.WeaponType.Melee:
                meleeWeapon = GameObject.Instantiate(weaponPrefabs[3], meleeSetPoint.position, meleeSetPoint.transform.rotation);
                meleeWeapon.transform.parent = meleeSetPoint.transform;
                currWeapon = meleeWeapon;

                weapon4 = new TestMeleeWeapon(range);
                Debug.Log("Enemy melee weapon Instantiated");
                break;

        }
    }
}
