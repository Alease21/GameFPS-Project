using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//This script is used to call an event on any enemy death to cause every instance of
//"InRangeSphereCollider" on explosive barrels to clean up their InRangeColliders array
public class EnemyDeathManager : MonoBehaviour
{
    //Singleton setup
    public static EnemyDeathManager instance;
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

    public Action onEnemyDeath;
}
