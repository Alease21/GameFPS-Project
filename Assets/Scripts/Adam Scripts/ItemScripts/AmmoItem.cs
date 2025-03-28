using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : ItemBase
{
    //Basic constructor (can initialize variables if ever needed)
    public AmmoItem()
    {

    }

    //Deactivates item once picked consumed. (Swap to true destroy somehow?)
    public override void OnPackConsume(GameObject itemGameobject)
    {
        packPrefab.gameObject.SetActive(false);
    }

    public override void RechargeLink(float rechargeTime)
    {
        //does not recharge
    }
}
