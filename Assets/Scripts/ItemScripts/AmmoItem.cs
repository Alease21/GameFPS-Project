using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : ItemBase
{
    //Basic constructor (can initialize variables if ever needed)
    public AmmoItem()
    {

    }

    //Deactivates item once picked consumed
    public override void OnPackConsume(GameObject itemGameobject)
    {
        isConsumed = true;

        packPrefab = itemGameobject;
        packPrefab.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 6;
        }
    }
    public void OnEnablePack()
    {
        packPrefab.GetComponent<BoxCollider>().enabled = true;

        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 0;
        }
    }

    public override void RechargeLink()
    {
        OnEnablePack();
    }
}
