using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableController : MonoBehaviour
{
    //Singleton setup
    public static ThrowableController instance;
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

    private bool _hasGrenade = false,
                 _hasSmokeBomb = false,
                 _isGrenade = false,
                 _isSmokeBomb = false;

    //at some point make all calls through property (private stuff sets field atm)
    public bool HasGrenade { get { return _hasGrenade; } private set { _hasGrenade = value; } }
    public bool HasSmokeBomb { get { return _hasSmokeBomb; } private set { _hasSmokeBomb = value; } }
    public bool IsGrenade { get { return _isGrenade; } private set { _isGrenade = value; } }
    public bool IsSmokeBomb { get { return _isSmokeBomb; } private set { _isSmokeBomb = value; } }

    public Transform throwPoint;

    private ThrowableBase _throwable1,
                          _throwable2,
                          _currThrowable;


    public Action OnThrowableUse;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && _hasGrenade)
        {
            _currThrowable = _throwable1;
            _isGrenade = true;
            _isSmokeBomb = false;
            InventoryController.instance.OnWeaponSwap();
            Debug.Log("Grenade Equipped");
        }
        if (Input.GetKeyDown(KeyCode.E) && _hasSmokeBomb)
        {
            _currThrowable = _throwable2;
            _isGrenade = false;
            _isSmokeBomb = true;
            InventoryController.instance.OnWeaponSwap();
            Debug.Log("Smoke Bomb Equipped");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if ((_hasGrenade || _hasSmokeBomb) && _currThrowable.throwableCount > 0)
            {
                _currThrowable?.Use(throwPoint.transform);
                ThrowableCountUpdater();
            }
            else
            {
                Debug.Log("No throwables to throw.");
            }
        }
    }

    //check if player has Grenade "weapon" picked up, if false instantiate it. if true add 'ammo' to it's count 
    public bool ThrowableGet(WeaponSO weaponSO)
    {
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.Grenade:
                if (_throwable1 == null)
                {
                    _throwable1 = new GrenadeThrowable(weaponSO);
                    _currThrowable = _throwable1;
                    _hasGrenade = true;
                    _isGrenade = true;
                    _isSmokeBomb = false;
                    return true;
                }
                else
                {
                    if (_throwable1.CountGet(weaponSO.ammoCount))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case WeaponSO.WeaponType.SmokeBomb:
                if (_throwable2 == null)
                {
                    _throwable2 = new SmokeBombThrowable(weaponSO);
                    _currThrowable = _throwable2;
                    _hasSmokeBomb = true;
                    _isGrenade = false;
                    _isSmokeBomb = true;
                    return true;
                }
                else
                {
                    if (_throwable2.CountGet(weaponSO.ammoCount))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            default:
                return false;
        }
    }

    public void ThrowableCountUpdater()
    {
        if (_throwable1 != null)
        {
            PlayerStatsScript.instance.grenadeCount = _throwable1.throwableCount;
            PlayerStatsScript.instance.maxGrenades = _throwable1.throwableMax;
        }
        if (_throwable2 != null)
        {
            PlayerStatsScript.instance.smokeBombCount = _throwable2.throwableCount;
            PlayerStatsScript.instance.maxSmokeBombs = _throwable2.throwableMax;
        }
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }

    public void OnLoadGameData(bool[] bArray)
    {
        HasGrenade = bArray[0];
        HasSmokeBomb = bArray[1];
        IsGrenade = bArray[2];
        IsSmokeBomb = bArray[3];
    }
}