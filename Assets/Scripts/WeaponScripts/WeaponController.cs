using System;
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

    private PlayerStatsScript _playerStatsScript;

    private GunBase _myGun;
    private GunBase _weapon1,
                   _weapon2,
                   _weapon3;
    public GunBase MyGun { get { return _myGun; } private set { _myGun = value; } }
    public GunBase Weapon1 { get { return _weapon1; } private set { _weapon1 = value; } }
    public GunBase Weapon2 { get { return _weapon2; } private set { _weapon2 = value; } }
    public GunBase Weapon3 { get { return _weapon3; } private set { _weapon3 = value; } }

    [SerializeField]private bool _isHitScan,
                 _isProjectile,
                 _isContinuous,
                 _hasHitScan = false,
                 _hasProjectile = false,
                 _hasContinuous = false,
                 _isUnarmed = true; //change this to start for if player should start w/ weapon

    //at some point make all calls through property (private stuff sets field atm)
    public bool IsHitScan { get { return _isHitScan; } private set { _isHitScan = value; } }
    public bool IsProjectile { get { return _isProjectile; } private set { _isProjectile = value; } }
    public bool IsContinuous { get { return _isContinuous; } private set { _isContinuous = value; } }
    public bool IsUnarmed { get { return _isUnarmed; } private set { _isUnarmed = value; } }
    public bool HasHitScan { get { return _hasHitScan; } private set { _hasHitScan = value; } }
    public bool HasProjectile { get { return _hasProjectile; } private set { _hasProjectile = value; } }
    public bool HasContinuous { get { return _hasContinuous; } private set { _hasContinuous = value; } }

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    [SerializeField] private Transform gunEmptyObj;

    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon;
    [SerializeField] private GameObject currWeaponPrefab;

    [SerializeField] private float continuousTickRate;//possible to grab from weaponSO upon item pickup?
    private float continuousTimer = 0f;
    private bool isHoldingFire = false;
    public bool IsHoldingFire { get { return isHoldingFire; } private set { isHoldingFire = value; } }


    public Action OnFireWeapon; //sfx
    public Action OnSwapWeapon; //sfx

    //Bandaid fix please figure out weapon spawn on game load
    public WeaponSO[] weaponSOArray;
    //*****************************************************

    void Start()
    {
        _playerStatsScript = PlayerStatsScript.instance;//Does this make sense?
        gunEmptyObj = transform.Find("Main Camera").Find("GunEmpty");

        //SaveLoadControl.instance.gameLoad += WeaponPrefabSwap;
    }

    void Update()
    {
        if (_myGun != null)
        {
            //if player has hitscan weapon, swap to hitscan weapon
            if (Input.GetKeyDown(KeyCode.Alpha1) && !_isHitScan)
            {
                if (_hasHitScan)
                {
                    _myGun = _weapon1;
                    WeaponPrefabSwap(WeaponSO.WeaponType.HitScan);
                }
            }
            //if player has projectile weapon, swap to projectile weapon
            if (Input.GetKeyDown(KeyCode.Alpha2) && !_isProjectile)
            {
                if (_hasProjectile)
                {
                    _myGun = _weapon2;
                    WeaponPrefabSwap(WeaponSO.WeaponType.Projectile);
                }
            }
            //if player has continuous weapon, swap to continuous weapon
            if (Input.GetKeyDown(KeyCode.Alpha3) && !_isContinuous)
            {
                if (_hasContinuous)
                {
                    _myGun = _weapon3;
                    WeaponPrefabSwap(WeaponSO.WeaponType.Continuous);
                }
            }
            //if equipped weapon is not continuous, use currently equipped weapon and update ammo
            if (Input.GetMouseButtonDown(0) && !_isContinuous)
            {
                OnFireWeapon?.Invoke();

                CameraMovement.instance.StartCoroutine(CameraMovement.instance.GunRecoilCoro(WeaponSO.WeaponType.HitScan));

                _myGun.Use();
                AmmoStatUpdater();
            }
            //if equipped weapon is continuous, use weapon at continouousTickRate intervals
            if (Input.GetMouseButton(0) && _isContinuous)
            {
                if (!isHoldingFire)
                {
                    OnFireWeapon?.Invoke();

                    isHoldingFire = true;
                    _myGun.Use();
                }
                else
                {
                    continuousTimer += Time.deltaTime;
                    if (continuousTimer >= continuousTickRate / 5)
                    {
                        OnFireWeapon?.Invoke();

                        _myGun.Use();
                        continuousTimer = 0f;
                    }
                }
                AmmoStatUpdater();
            }
            //On left mouse release, update ammo and flip isHoldingFire to false if it is true
            if (Input.GetMouseButtonUp(0) && _isContinuous)
            {
                AmmoStatUpdater();
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
        while (_myGun == _weapon3)
        {
            while (isHoldingFire && _myGun.ammoCount > 0)
            {
                _myGun.ammoCount--;
                yield return new WaitForSeconds(continuousTickRate);
            }
            yield return null;
        }
    }

    //Initial gun object and gameobject instantiation based on weaponType param. New gun instantiated with params initialAmmoMax, initialAmmoCount and damage.
    //Immediately swap currWeapon/myWeapon to new instantiated gameobject/gun and update ammo.
    public void WeaponPrefabSpawn(WeaponSO weaponSO)
    {
        //Based on weaponType, check if player has that weapon (bool), if false then instantiate gameobject and gun object and flip appropriate bool true
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                if (!_hasHitScan)
                {
                    hitScanWeapon = GameObject.Instantiate(weaponSO.weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                    hitScanWeapon.transform.parent = gunSetPoint.transform;
                    _weapon1 = new HitScanGun(weaponSO) { shootPoint = shootPoint }; //give SO transform for specfic weapon shootpoint?

                    _hasHitScan = true;
                    Debug.Log("Hitscan weapon instantiated");
                }
                break;

            case WeaponSO.WeaponType.Projectile:
                if (!_hasProjectile)
                {
                    projectileWeapon = GameObject.Instantiate(weaponSO.weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                    projectileWeapon.transform.parent = gunSetPoint.transform;
                    _weapon2 = new ProjectileGun(weaponSO) { shootPoint = shootPoint };//give SO transform for specfic weapon shootpoint?

                    _hasProjectile = true;
                    Debug.Log("Projectile weapon instantiated");
                }
                break;
            case WeaponSO.WeaponType.Continuous:
                if (!_hasContinuous)
                {
                    continuousWeapon = GameObject.Instantiate(weaponSO.weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                    continuousWeapon.transform.parent = gunSetPoint.transform;
                    _weapon3 = new ContinuousGun(weaponSO) { shootPoint = shootPoint };//give SO transform for specfic weapon shootpoint?

                    _hasContinuous = true;
                    Debug.Log("Continuous weapon instantiated");
                }
                break;
        }
        WeaponPrefabSwap(weaponSO.weaponType, IsUnarmed);
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                _playerStatsScript.maxHitscanAmmo = _myGun.ammoMax;
                break;
            case WeaponSO.WeaponType.Projectile:
                _playerStatsScript.maxProjectileAmmo = _myGun.ammoMax;
                break;
            case WeaponSO.WeaponType.Continuous:
                _playerStatsScript.maxContinuousAmmo = _myGun.ammoMax;
                break;
        }
        EquippedWeaponBool(weaponSO.weaponType);

        AmmoStatUpdater();
    }

    //Swap between weapons that the player currently has & adjust gen position to prefab settings.
    //start ContinuousWeaponFire coro if continuousWeapon
    public void WeaponPrefabSwap(WeaponSO.WeaponType newWeaponType, bool isUnarmed = false)
    {
        if (!IsUnarmed)
        {
            currWeaponPrefab.SetActive(false);
        }
        else
        {
            IsUnarmed = false;
        }

        isHoldingFire = false;
        EquippedWeaponBool(newWeaponType);
        InventoryController.instance.OnWeaponSwap();

        switch (newWeaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                currWeaponPrefab = hitScanWeapon;
                gunEmptyObj.localPosition = hitScanWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = hitScanWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon1;
                break;
            case WeaponSO.WeaponType.Projectile:
                currWeaponPrefab = projectileWeapon;
                gunEmptyObj.localPosition = projectileWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = projectileWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon2;
                break;
            case WeaponSO.WeaponType.Continuous:
                currWeaponPrefab = continuousWeapon;
                gunEmptyObj.localPosition = continuousWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = continuousWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon3;
                StartCoroutine(ContinuousWeaponFire());
                break;
        }
        if (!currWeaponPrefab.activeInHierarchy)
        {
            currWeaponPrefab.SetActive(true);
        }
        OnSwapWeapon?.Invoke();
    }

    //Controls bools for what weapon is currently equipped
    public void EquippedWeaponBool(WeaponSO.WeaponType weaponType)
    {
        switch (weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                _isHitScan = true;
                _isProjectile = false;
                _isContinuous = false;
                break;
            case WeaponSO.WeaponType.Projectile:
                _isHitScan = false;
                _isProjectile = true;
                _isContinuous = false;
                break;
            case WeaponSO.WeaponType.Continuous:
                _isHitScan = false;
                _isProjectile = false;
                _isContinuous = true;
                break;
        }
    }

    //Updates player stats with current ammo count
    public void AmmoStatUpdater()
    {
        //eventually implement switch for weapontype, add weapontype param and get
        //weapon type from pickup for specific gun ammo packs
        if (_weapon1 != null)
        {
            _playerStatsScript.hitScanWeaponAmmo = _weapon1.ammoCount;
        }
        if (_weapon2 != null)
        {
            _playerStatsScript.projectileWeaponAmmo = _weapon2.ammoCount;
        }
        if (_weapon3 != null)
        {
            _playerStatsScript.continuousWeaponAmmo = _weapon3.ammoCount;
        }
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }

    public void OnLoadGameData(bool[] bArray)
    {
        IsHitScan = bArray[0];
        IsProjectile = bArray[1];
        IsContinuous = bArray[2];
        HasHitScan = bArray[3];
        HasProjectile = bArray[4];
        HasContinuous = bArray[5];
        IsUnarmed = bArray[6];
        
        LoadGameWeaponSwap();
    }

    public void LoadGameWeaponSwap()
    {
        isHoldingFire = false; //maybe delete?

        if (currWeaponPrefab != null)
        {
            currWeaponPrefab.SetActive(false);
        }

        //ammo adjust and weapon swap based on bools
        if (HasHitScan)
        {
            //prefab spawning if weapons are null on load
            if (hitScanWeapon == null)
            {
                //maybe find better way to do this
                hitScanWeapon = GameObject.Instantiate(weaponSOArray[0].weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                hitScanWeapon.transform.parent = gunSetPoint.transform;
                _weapon1 = new HitScanGun(weaponSOArray[0]) { shootPoint = shootPoint };
                hitScanWeapon.SetActive(false);
            }

            if (IsHitScan)
            {
                currWeaponPrefab = hitScanWeapon;

                gunEmptyObj.localPosition = hitScanWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = hitScanWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon1;
            }

            _weapon1.ammoCount = PlayerStatsScript.instance.hitScanWeaponAmmo;
        }
        if (HasProjectile)
        {
            //prefab spawning if weapons are null on load
            if (projectileWeapon == null)
            {
                //maybe find better way to do this
                projectileWeapon = GameObject.Instantiate(weaponSOArray[1].weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                projectileWeapon.transform.parent = gunSetPoint.transform;
                _weapon2 = new ProjectileGun(weaponSOArray[1]) { shootPoint = shootPoint };
                projectileWeapon.SetActive(false);
            }

            if (IsProjectile)
            {
                currWeaponPrefab = projectileWeapon;

                gunEmptyObj.localPosition = projectileWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = projectileWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon2;
            }
            _weapon2.ammoCount = PlayerStatsScript.instance.projectileWeaponAmmo;
        }
        if (HasContinuous)
        {
            //prefab spawning if weapons are null on load
            if (continuousWeapon == null)
            {
                //maybe find better way to do this
                continuousWeapon = GameObject.Instantiate(weaponSOArray[2].weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                continuousWeapon.transform.parent = gunSetPoint.transform;
                _weapon3 = new ContinuousGun(weaponSOArray[2]) { shootPoint = shootPoint };
                continuousWeapon.SetActive(false);
            }

            if (IsContinuous)
            {
                currWeaponPrefab = continuousWeapon;

                gunEmptyObj.localPosition = continuousWeapon.transform.Find("GunEmptySetPoint").localPosition;
                gunEmptyObj.localRotation = continuousWeapon.transform.Find("GunEmptySetPoint").localRotation;
                _myGun = _weapon3;
                StartCoroutine(ContinuousWeaponFire());
            }
            _weapon3.ammoCount = PlayerStatsScript.instance.continuousWeaponAmmo;
        }

        InventoryController.instance.OnWeaponSwap();
        InventoryController.instance.UIUpdateEvent?.Invoke();

        if (currWeaponPrefab != null && !currWeaponPrefab.activeInHierarchy)
        {
            currWeaponPrefab.SetActive(true);
        }
        OnSwapWeapon?.Invoke();
    }
}