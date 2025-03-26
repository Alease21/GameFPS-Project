using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public WeaponBase myWeapon;
    public Transform shootPoint;
    public GameObject projectilePreFab;

    public Dictionary<WeaponSO.WeaponType, GameObject> weaponPrefabs = new Dictionary<WeaponSO.WeaponType, GameObject>();
    public GameObject initialWeapon;
    public GameObject currWeapon;
    public Transform gunSetPoint;

    public bool hasHitScan;
    public bool hasProjectile;

    void Start()
    {
        myWeapon = new Gun(new RaycastBehavior());
        myWeapon.shootPoint = shootPoint;

        HaveWeaponChecker(WeaponSO.WeaponType.HitScan, initialWeapon);

        GameObject hitScanWeapon = GameObject.Instantiate(initialWeapon, gunSetPoint.position, gunSetPoint.transform.rotation);
        hitScanWeapon.transform.parent = transform;
        currWeapon = hitScanWeapon;
        myWeapon.SetWeaponBehavior(new RaycastBehavior());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (hasHitScan)
            {
                GeneralWeaponSwap(WeaponSO.WeaponType.HitScan);
                Debug.Log("HitScan Weapon Loaded");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (hasProjectile)
            {
                GeneralWeaponSwap(WeaponSO.WeaponType.Projectile);
                Debug.Log("Projectile Weapon Loaded");
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            myWeapon.Use();
        }
    }
    public void GeneralWeaponSwap(WeaponSO.WeaponType weaponType)
    {
        currWeapon.SetActive(false);

        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                currWeapon = weaponPrefabs[WeaponSO.WeaponType.HitScan];
                break;
            case WeaponSO.WeaponType.Projectile:
                currWeapon = weaponPrefabs[WeaponSO.WeaponType.Projectile];
                break;
        }
        currWeapon.SetActive(true);
    }

    public void GeneralSpawnWeapon(WeaponSO.WeaponType weaponType)
    {
        currWeapon.SetActive(false);

        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                if (!hasHitScan)
                    hasHitScan = true;

                Debug.Log("HitScan Weapon picked up");

                GameObject hitScanWeapon = GameObject.Instantiate(weaponPrefabs[WeaponSO.WeaponType.HitScan], gunSetPoint.position, gunSetPoint.transform.rotation);
                hitScanWeapon.transform.parent = transform;
                currWeapon = hitScanWeapon;
                myWeapon.SetWeaponBehavior(new RaycastBehavior());
                break;

            case WeaponSO.WeaponType.Projectile:
                if (!hasProjectile) 
                    hasProjectile = true;

                Debug.Log("Projectile Weapon picked up");

                GameObject projectileWeapon = GameObject.Instantiate(weaponPrefabs[WeaponSO.WeaponType.Projectile], gunSetPoint.position, gunSetPoint.transform.rotation);
                projectileWeapon.transform.parent = transform;
                currWeapon = projectileWeapon;
                myWeapon.SetWeaponBehavior(new ProjectileBehavior { projectilePrefab = projectilePreFab });
                break;
        }
    }
    public void HaveWeaponChecker(WeaponSO.WeaponType weaponType, GameObject prefab)
    {
        if (!weaponPrefabs.ContainsKey(weaponType))
        {
            weaponPrefabs.Add(weaponType, prefab);
        }   
    }
}