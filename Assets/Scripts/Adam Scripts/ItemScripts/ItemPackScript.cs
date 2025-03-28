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
