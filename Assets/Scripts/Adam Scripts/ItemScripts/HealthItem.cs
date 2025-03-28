using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;

public class HealthItem : ItemBase, iRechargableItem
{
    public HealthItem()
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
        Debug.Log("health item recharged");
        isRecharging = false;
    }
}
