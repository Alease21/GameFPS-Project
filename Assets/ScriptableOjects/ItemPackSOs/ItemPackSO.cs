using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemPack", menuName = "NewItem/NewItemPack")]
public class ItemPackSO : ScriptableObject
{
    public enum ItemPackType
    {
        HealthPack,
        HOTPack,
        ShieldPack,
        AmmoPack
    }
    public string itemPackName;
    public int packAmount;
    public ItemPackType itemPackType;

    // edit inspector to show only on iRechargableItem items?
    public float rechargeTime;

    //hots only
    public float tickTime;
    public int numTicks;
}
