using System.Collections;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;

    [SerializeField] private WeaponBase myWeapon;
    public WeaponBase weapon1,
                      weapon2,
                      weapon3;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    
    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon;
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private GameObject currWeapon;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject fireVisualPrefab;//continuous fire visual (not implemented yet)

    [SerializeField] private float continuousTickRate;//possible to grab from weaponSO upon item pickup?
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
        //if 1 key pressed and if player has hitscan weapon, swap to hitscan weapon
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
        //if 2 key pressed and if player has projectile weapon, swap to projectile weapon
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
        //if 3 key pressed and if player has continuous weapon, swap to continuous weapon
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
        //On left mouse click, use currently equipped weapon and update ammo
        if (Input.GetMouseButtonDown(0) && !isContinuous)
        {
            myWeapon.Use();
            AmmoStatUpdater();
        }

        //On left mouse hold and myWeapon isContinuous, call use() for myWeapon and update
        //ammo every frame then flip isHoldingFire true if it is false.
        //On left mouse release, update ammo and flip isHoldingFire to false if it is true
        if (Input.GetMouseButton(0) && isContinuous)
        {
            myWeapon.Use();

            AmmoStatUpdater();
            if (!isHoldingFire)
            {
                isHoldingFire = true;
            }
        }
        else if(Input.GetMouseButtonUp(0) && isContinuous)
        {
            AmmoStatUpdater();//maybe delete?

            if (isHoldingFire)
            {
                isHoldingFire = false;
            }
        }
    }

    //Coroutine to decrement ammoCount for continuous weapon every continuousTickRate seconds
    public IEnumerator ContinuousWeaponFire()
    {
        while (true)
        {
            if (isHoldingFire)
            {
                myWeapon.ammoCount--;
                yield return new WaitForSeconds(continuousTickRate);
            }
            yield return new WaitForFixedUpdate();
            yield return null;
        }
    }

    //Initial gun object and gameobject instantiation based on weaponType param. New gun instantiated with params initialAmmoMax and initialAmmoCount
    //Immediately swap currWeapon/myWeapon to new instantiated gameobject/gun and update ammo.
    public void WeaponPrefabSpawn(WeaponSO.WeaponType weaponType, int initialAmmoMax, int initialAmmoCount)
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

                    weapon3 = new ContinuousGun(fireVisualPrefab, initialAmmoMax, initialAmmoCount);
                    weapon3.shootPoint = shootPoint;
                    myWeapon = weapon3;

                    hasContinuous = true;
                    Debug.Log("Continuous weapon instantiated");
                }
                break;
        }

        EquippedWeaponBool(weaponType);
        AmmoStatUpdater();
    }

    //Swap between weapons that the player currently has, start/stop ContinuousWeaponFire
    //coroutine based on what weapon is swapped to
    public void WeaponPrefabSwap(WeaponSO.WeaponType newWeaponType)
    {
        currWeapon.SetActive(false);
        EquippedWeaponBool(newWeaponType);

        switch (newWeaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                currWeapon = hitScanWeapon;
                myWeapon = weapon1;
                StopCoroutine(ContinuousWeaponFire());
                break;
            case WeaponSO.WeaponType.Projectile:
                currWeapon = projectileWeapon;
                myWeapon = weapon2;
                StopCoroutine(ContinuousWeaponFire());
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

    //Controls bools for what weapon is currently equipped
    public void EquippedWeaponBool(WeaponSO.WeaponType weaponType)
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

    //Updates player stats with current ammo count
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