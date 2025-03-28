using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : ItemBase
{
    public AmmoItem()
    {

    }

    public override void OnPackConsume(GameObject itemGameobject)
    {
        //not sure how to destroy from here
        packPrefab.gameObject.SetActive(false);
    }

    public override void RechargeLink(float rechargeTime)
    {
        //does not recharge
    }
}
