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

    public ThrowableBase throwable1;
    public ThrowableBase currThrowable;

    public bool hasGrenade = false;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (hasGrenade || PlayerStatsScript.instance.grenadeCount != 0)
            {
                throwable1.Use(throwPoint.transform);//add ammo--
            }
            else
            {
                Debug.Log("No throwables to throw.");
            }
        }
    }

    //check if player has Grenade "weapon" picked up, if false instantiate it. if true add 'ammo' to it's count 
    public bool ThrowableGet(int count, int countMax, int damage, float range, float timer)
    {
        if (throwable1 == null)
        {
            throwable1 = new GrenadeThrowable(throwablePrefabs[0], count, countMax, damage, range, timer);
            hasGrenade = true;
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
    }

    public void ThrowableCountUIUpdater()
    {
        if (throwable1 != null)
        {
            PlayerStatsScript.instance.grenadeCount = throwable1.throwableCount;
        }
    }
}