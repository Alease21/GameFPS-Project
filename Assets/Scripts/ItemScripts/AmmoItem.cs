using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoItem : ItemBase
{
    //Basic constructor (can initialize variables if ever needed)
    public AmmoItem()
    {

    }

    //Sets gameobject & children to invis layer (so player cannot interact/see)
    public override void OnPackConsume(GameObject itemGameobject)
    {
        isConsumed = true;

        packPrefab = itemGameobject;
        packPrefab.GetComponent<BoxCollider>().enabled = false;

        for (int i = 0; i < packPrefab.transform.childCount; i++)
        {
            GameObject child = packPrefab.transform.GetChild(i).gameObject;
            child.layer = 6;//invis layer
        }
    }
    //Sets gameobject & children to default layer (so player can interact/see)
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
