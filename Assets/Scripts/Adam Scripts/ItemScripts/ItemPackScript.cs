using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPackScript : MonoBehaviour
{
    public ItemPackSO itemPackSO;
    public GameObject itemPackPrefab;

    public ItemBase item;

    private void Start()
    {
        //Instantiate new itemBase based of the itempacktype of the attached itemPackSO.
        //In case of HealhPack or ShieldPack ItemPackTypes, start ItemRechargeCoro().
        switch (itemPackSO.itemPackType)
        {
            case ItemPackSO.ItemPackType.HealthPack:
                item = new HealthItem { packAmount = itemPackSO.packAmount, packPrefab = itemPackPrefab };

                StartCoroutine(ItemRechargeCoro());
                break;
            case ItemPackSO.ItemPackType.ShieldPack:
                item = new ShieldItem { packAmount = itemPackSO.packAmount, packPrefab = itemPackPrefab };

                StartCoroutine(ItemRechargeCoro());
                break;
            case ItemPackSO.ItemPackType.AmmoPack:
                item = new AmmoItem { packAmount = itemPackSO.packAmount, packPrefab = itemPackPrefab };
                break;
        }
    }

    //Coroutine to wait until instantiated item bool isRecharging is true, then
    //wait rechargeTime(from attached itemSO) seconds before calling RechargeLink
    //from item and restarting the coroutine
    public IEnumerator ItemRechargeCoro()
    {
        yield return new WaitUntil(() => item.isRecharging);

        for (float timer = 0f; timer < itemPackSO.rechargeTime; timer += Time.deltaTime)
        {
            yield return null;
        }
        item.RechargeLink(itemPackSO.rechargeTime);

        StartCoroutine(ItemRechargeCoro());
    }
}
