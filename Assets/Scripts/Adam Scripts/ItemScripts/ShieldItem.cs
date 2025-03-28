using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ItemBase, iRechargableItem
{
    public ShieldItem()
    {

    }

    public override void OnPackConsume(GameObject itemGameobject)
    {
        packPrefab = itemGameobject;

        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 6; //invis layer
        }

        isRecharging = true;
    }

    public override void RechargeLink(float rechargeTime)
    {
        RechargeItem(rechargeTime);
    }

    public void RechargeItem(float rechargeTime)
    {
        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 0;
        }
        Debug.Log("Shield item recharged");
        isRecharging = false;
    }
}
