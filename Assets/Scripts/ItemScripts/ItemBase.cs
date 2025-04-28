using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base abstract class for an item pack
public abstract class ItemBase
{
    public int packAmount;
    public GameObject packPrefab;
    public bool isRecharging = false;

    public abstract void OnPackConsume(GameObject itemGameObject);

    //using this to trigger the rechargeitem method from iRechargableItem interface.
    //(probably change/fix this?)
    public abstract void RechargeLink();
}
