using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemBase
{
    public int packAmount;
    public GameObject packPrefab;
    public bool isRecharging = false;

    public abstract void OnPackConsume(GameObject itemGameobject);

    //using this to trigger the rechargeitem method from iRechargableItem interface.
    //probably fix this?
    public abstract void RechargeLink(float rechargeTime);
}
