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

    public GameObject[] throwablePrefabs;
    public Transform throwPoint;

    public ThrowableBase throwable1,
                         throwable2;
    public ThrowableBase currThrowable;

    public bool hasGrenade = false;
    public bool hasSmokeBomb = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currThrowable = throwable1;
            Debug.Log("Grenade Equipped");
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currThrowable = throwable2;
            Debug.Log("Smoke Bomb Equipped");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if ((hasGrenade || hasSmokeBomb) && currThrowable.throwableCount > 0)
            {
                currThrowable?.Use(throwPoint.transform);
            }
            else
            {
                Debug.Log("No throwables to throw.");
            }
        }
    }

    //check if player has Grenade "weapon" picked up, if false instantiate it. if true add 'ammo' to it's count 
    public bool ThrowableGet(WeaponSO.WeaponType throwableType, int count, int countMax, int damage, float range, float timer)
    {
        switch (throwableType)
        {
            case WeaponSO.WeaponType.Grenade:
                if (throwable1 == null)
                {
                    throwable1 = new GrenadeThrowable(throwablePrefabs[0], count, countMax, damage, range, timer);
                    hasGrenade = true;
                    currThrowable = throwable1;
                    return true;
                }
                else
                {
                    if (throwable1.CountGet(count))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            case WeaponSO.WeaponType.SmokeBomb:
                if (throwable2 == null)
                {
                    throwable2 = new SmokeBombThrowable(throwablePrefabs[1], count, countMax, range, timer);
                    currThrowable = throwable2;
                    hasSmokeBomb = true;
                    return true;
                }
                else
                {
                    if (throwable1.CountGet(count))
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

    public void ThrowableCountUIUpdater()
    {
        if (throwable1 != null)
        {
            PlayerStatsScript.instance.grenadeCount = throwable1.throwableCount;
        }
    }
}