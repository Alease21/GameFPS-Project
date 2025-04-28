using System.Collections;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class WeaponController : MonoBehaviour
{
    //Singleton setup
    public static WeaponController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private PlayerStatsScript playerStatsScript;

    public GunBase myGun;
    public GunBase weapon1,
                   weapon2,
                   weapon3;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    
    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon;
    [SerializeField] private GameObject[] weaponPrefabs;//change to grab from weaponSO with projectile/fire prefabs?
    public GameObject currWeapon;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject fireVisualPrefab;
    [SerializeField] private GameObject hitScanShotPrefab;

    [SerializeField] private float continuousTickRate;//possible to grab from weaponSO upon item pickup?
    private bool isHoldingFire = false;
    private float continuousTimer = 0f;

    public bool isHitScan,
                isProjectile,
                isContinuous,
                isUnarmed;

    //set up to never flip false after true, could change for dropping weapons in future.
    public bool hasHitScan = false,
                hasProjectile = false,
                hasContinuous = false;

    void Start()
    {
        playerStatsScript = PlayerStatsScript.instance;//Does this make sense?

        isUnarmed = true;
        //hard coded in hitscan values. not sure how to grab in a more dynamic way.
        //maybe start with no weapon and choose 1 of 3?
        //WeaponPrefabSpawn(WeaponSO.WeaponType.HitScan, 20, 20, 10);
    }
    
    void Update()
    {
        if (myGun != null)
        {
            //if player has hitscan weapon, swap to hitscan weapon
            if (Input.GetKeyDown(KeyCode.Alpha1) && !isHitScan)
            {
                if (hasHitScan)
                {
                    myGun = weapon1;
                    WeaponPrefabSwap(WeaponSO.WeaponType.HitScan);
                }
            }
            //if player has projectile weapon, swap to projectile weapon
            if (Input.GetKeyDown(KeyCode.Alpha2) && !isProjectile)
            {
                if (hasProjectile)
                {
                    myGun = weapon2;
                    WeaponPrefabSwap(WeaponSO.WeaponType.Projectile);
                }
            }
            //if player has continuous weapon, swap to continuous weapon
            if (Input.GetKeyDown(KeyCode.Alpha3) && !isContinuous)
            {
                if (hasContinuous)
                {
                    myGun = weapon3;
                    WeaponPrefabSwap(WeaponSO.WeaponType.Continuous);
                }
            }
            //if equipped weapon is not continuous, use currently equipped weapon and update ammo
            if (Input.GetMouseButtonDown(0) && !isContinuous)
            {
                myGun.Use();
                AmmoStatUpdater();
            }
            //if equipped weapon is continuous, use weapon at continouousTickRate intervals
            if (Input.GetMouseButton(0) && isContinuous)
            {
                if (!isHoldingFire)
                {
                    isHoldingFire = true;
                    myGun.Use();
                }
                else
                {
                    continuousTimer += Time.deltaTime;
                    if (continuousTimer >= continuousTickRate / 5)
                    {
                        myGun.Use();
                        continuousTimer = 0f;
                    }
                }
                AmmoStatUpdater();
            }
            //On left mouse release, update ammo and flip isHoldingFire to false if it is true
            if (Input.GetMouseButtonUp(0) && isContinuous)
            {
                AmmoStatUpdater();
                Debug.Log("test button up");
                if (isHoldingFire)
                {
                    isHoldingFire = false;
                }
            }
        }
    }

    //Coroutine to decrement ammoCount for continuous weapon every continuousTickRate seconds
    public IEnumerator ContinuousWeaponFire()
    {
        while (myGun == weapon3)
        {
            while (isHoldingFire && myGun.ammoCount > 0)
            {
                myGun.ammoCount--;
                yield return new WaitForSeconds(continuousTickRate);
            }
            yield return null;
        }
    }

    //Initial gun object and gameobject instantiation based on weaponType param. New gun instantiated with params initialAmmoMax, initialAmmoCount and damage.
    //Immediately swap currWeapon/myWeapon to new instantiated gameobject/gun and update ammo.
    public void WeaponPrefabSpawn(WeaponSO.WeaponType weaponType, int initialAmmoMax, int initialAmmoCount, int damage, float range)
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

                    weapon1 = new HitScanGun(hitScanShotPrefab, initialAmmoMax, initialAmmoCount, damage) { shootPoint = shootPoint };
                    myGun = weapon1;
                    playerStatsScript.maxHitscanAmmo = myGun.ammoMax;
                    hasHitScan = true;

                    Debug.Log("Hitscan weapon instantiated");
                }
                break;

            case WeaponSO.WeaponType.Projectile:
                if (!hasProjectile)
                {
                    projectileWeapon = GameObject.Instantiate(weaponPrefabs[1], gunSetPoint.position, gunSetPoint.transform.rotation);
                    projectileWeapon.transform.parent = gunSetPoint.transform;
                    currWeapon = projectileWeapon;

                    weapon2 = new ProjectileGun(projectilePreFab, initialAmmoMax, initialAmmoCount, damage, range) { shootPoint = shootPoint };
                    myGun = weapon2;
                    playerStatsScript.maxProjectileAmmo = myGun.ammoMax;
                    hasProjectile = true;

                    Debug.Log("Projectile weapon instantiated");
                }
                break;
            case WeaponSO.WeaponType.Continuous:
                if (!hasContinuous)
                {
                    continuousWeapon = GameObject.Instantiate(weaponPrefabs[2], gunSetPoint.position, gunSetPoint.transform.rotation);
                    continuousWeapon.transform.parent = gunSetPoint.transform;
                    currWeapon = continuousWeapon;

                    weapon3 = new ContinuousGun(fireVisualPrefab, initialAmmoMax, initialAmmoCount, damage) { shootPoint = shootPoint };
                    myGun = weapon3;
                    playerStatsScript.maxContinuousAmmo = myGun.ammoMax;
                    StartCoroutine(ContinuousWeaponFire());
                    hasContinuous = true;

                    Debug.Log("Continuous weapon instantiated");
                }
                break;
        }

        EquippedWeaponBool(weaponType);
        InventoryController.instance.OnWeaponSwap();
        AmmoStatUpdater();
    }

    //Swap between weapons that the player currently has, start/stop ContinuousWeaponFire
    //coroutine based on what weapon is swapped to
    public void WeaponPrefabSwap(WeaponSO.WeaponType newWeaponType)
    {
        currWeapon.SetActive(false);
        isHoldingFire = false;
        EquippedWeaponBool(newWeaponType);
        InventoryController.instance.OnWeaponSwap();

        switch (newWeaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                currWeapon = hitScanWeapon;
                myGun = weapon1;
                break;
            case WeaponSO.WeaponType.Projectile:
                currWeapon = projectileWeapon;
                myGun = weapon2;
                break;
            case WeaponSO.WeaponType.Continuous:
                currWeapon = continuousWeapon;
                myGun = weapon3;
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
        //eventually implement switch for weapontype, add weapontype param and get
        //weapon type from pickup for specific gun ammo packs
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
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }
}