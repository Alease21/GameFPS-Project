using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;

    [SerializeField] private WeaponBase myWeapon;
    public WeaponBase weapon1,
                       weapon2,
                       weapon3;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject firePrefab;

    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private GameObject currWeapon;
    //[SerializeField] private WeaponSO.WeaponType currWeaponType;

    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon;
    [SerializeField] private float continuousTickRate;
    public bool isHoldingFire = false;

    private bool isHitScan,
                 isProjectile,
                 isContinuous;

    //set up to never flip false after true, could change for dropping weapons in future.
    public bool hasHitScan = false,
                hasProjectile = false,
                hasContinuous = false;

    void Start()
    {
        //hard coded in hitscan values. not sure how to grab in a more dynamic way.
        //maybe start with no weapon and choose 1 of 3?
        WeaponPrefabSpawn(WeaponSO.WeaponType.HitScan, 20, 20);
        isHitScan = true;

        myWeapon = weapon1;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isHitScan)
        {
            if (hasHitScan)
            {
                myWeapon = weapon1;
                WeaponPrefabSwap(WeaponSO.WeaponType.HitScan);

                Debug.Log("Swapped to hitscan weapon");
            }
            else
            {
                Debug.Log("No weapon in 1st slot");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isProjectile)
        {
            if (hasProjectile)
            {
                myWeapon = weapon2;
                WeaponPrefabSwap(WeaponSO.WeaponType.Projectile);

                Debug.Log("Swapped to projectile weapon");
            }
            else
            {
                Debug.Log("No weapon in 2nd slot");
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && !isContinuous)
        {
            if (hasContinuous)
            {
                myWeapon = weapon3;
                WeaponPrefabSwap(WeaponSO.WeaponType.Continuous);

                Debug.Log("Swapped to continuous weapon");
            }
            else
            {
                Debug.Log("No weapon in 3rd slot");
            }
        }
        if (Input.GetMouseButtonDown(0) && !isContinuous)
        {
            myWeapon.Use();
            AmmoStatUpdater();
        }
        if (Input.GetMouseButton(0) && isContinuous)
        {
            AmmoStatUpdater();
            if (!isHoldingFire)
            {
                isHoldingFire = true;
            }
        }
        else if(Input.GetMouseButtonUp(0) && isContinuous)
        {
            //maybe delete?
            AmmoStatUpdater();

            if (isHoldingFire)
            {
                isHoldingFire = false;
            }
        }
    }
    public IEnumerator ContinuousWeaponFire()
    {
        while (true)
        {
            if (isHoldingFire)
            {
                myWeapon.Use();
                yield return new WaitForSeconds(continuousTickRate);
            }
            yield return new WaitForFixedUpdate();
            yield return null;
        }
    }

    public void WeaponPrefabSpawn(WeaponSO.WeaponType weaponType, int initialAmmoMax, int initialAmmoCount)
    {
        if (currWeapon != null)
        {
            currWeapon.SetActive(false);
        }

        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                if (!hasHitScan)
                {
                    hitScanWeapon = GameObject.Instantiate(weaponPrefabs[0], gunSetPoint.position, gunSetPoint.transform.rotation);
                    hitScanWeapon.transform.parent = transform;
                    currWeapon = hitScanWeapon;

                    weapon1 = new HitScanGun(initialAmmoMax, initialAmmoCount);
                    weapon1.shootPoint = shootPoint;
                    myWeapon = weapon1;

                    hasHitScan = true;

                    Debug.Log("Hitscan weapon instantiated");
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

                    Debug.Log("Projectile weapon instantiated");
                }
                break;
            case WeaponSO.WeaponType.Continuous:
                if (!hasContinuous)
                {
                    continuousWeapon = GameObject.Instantiate(weaponPrefabs[2], gunSetPoint.position, gunSetPoint.transform.rotation);
                    continuousWeapon.transform.parent = transform;
                    currWeapon = continuousWeapon;
                    StartCoroutine(ContinuousWeaponFire());

                    weapon3 = new ContinuousGun(firePrefab, initialAmmoMax, initialAmmoCount);
                    weapon3.shootPoint = shootPoint;
                    myWeapon = weapon3;

                    hasContinuous = true;

                    Debug.Log("Continuous weapon instantiated");
                }
                break;
        }

        BoolStuff(weaponType);
        AmmoStatUpdater();
    }

    public void WeaponPrefabSwap(WeaponSO.WeaponType newWeaponType)
    {
        currWeapon.SetActive(false);
        BoolStuff(newWeaponType);


        switch (newWeaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                currWeapon = hitScanWeapon;
                myWeapon = weapon1;
                StopCoroutine(ContinuousWeaponFire());
                //currWeaponType = WeaponSO.WeaponType.HitScan;
                break;
            case WeaponSO.WeaponType.Projectile:
                currWeapon = projectileWeapon;
                myWeapon = weapon2;
                StopCoroutine(ContinuousWeaponFire());
                //currWeaponType = WeaponSO.WeaponType.Projectile;
                break;
            case WeaponSO.WeaponType.Continuous:
                currWeapon = continuousWeapon;
                myWeapon = weapon3;
                StartCoroutine(ContinuousWeaponFire());
                break;
        }
        if (!currWeapon.activeInHierarchy)
        {
            currWeapon.SetActive(true);
        }
    }
    public void BoolStuff(WeaponSO.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                isHitScan = true;
                isProjectile = false;
                isContinuous = false;
                break;
            case WeaponSO.WeaponType.Projectile:
                isHitScan = false;
                isProjectile = true;
                isContinuous = false;
                break;
            case WeaponSO.WeaponType.Continuous:
                isHitScan = false;
                isProjectile = false;
                isContinuous = true;
                break;
        }
    }

    public void AmmoStatUpdater()
    {
        if (weapon1 != null)
        {
            playerStatsScript.hitScanWeaponAmmo = weapon1.ammoCount;

        }
        if (weapon2 != null)
        {
            playerStatsScript.projectileWeaponAmmo = weapon2.ammoCount;
        }
        if (weapon3 != null)
        {
            playerStatsScript.continuousWeaponAmmo = weapon3.ammoCount;
        }
    }
}