using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthItem : ItemBase, iRechargeableItem
{
    //Basic constructor (can initialize variables if ever needed)
    public HealthItem()
    {

    }

    //Loop through every child of itemGameObject and swap layer to 6 (Invis layer culled by main cam)
    //then flip isRecharging bool true
    public override void OnPackConsume(GameObject itemGameObject)
    {
        packPrefab = itemGameObject;
       
        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 6; //invis layer
        }

        isRecharging = true;
    }

    //Method to call RechargeItem method
    //(band-aid fix as I couldn't figure out call RechargeItem() from ItemPickUpScript,
    //because it is not in the itemBase class, but is instead inherited from iRechargable)
    public override void RechargeLink()
    {
        RechargeItem();
    }

    //Loop through each child of itemGameObject and set layer back to 0 (default),
    //then flip isRecharging bool false
    public void RechargeItem()
    {
        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 0;
        }
        isRecharging = false;
    }
}
