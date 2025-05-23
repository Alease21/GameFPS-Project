using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldItem : ItemBase, iRechargeableItem
{
    //Basic constructor (can initialize variables if ever needed)
    public ShieldItem()
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
    public override void RechargeLink(float rechargeTime)
    {
        RechargeItem(rechargeTime);
    }

    //Loop through each child of itemGameObject and set layer back to 0 (default),
    //then flip isRecharging bool false
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
