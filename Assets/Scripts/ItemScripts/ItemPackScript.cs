using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(MyGUID))]
public class ItemPackScript : MonoBehaviour
{
    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }
    }

    public ItemPackSO itemPackSO;
    public GameObject itemPackPrefab;

    public ItemBase item;

    public bool isConsumed;//bool for save/load
            //if ^this then set item obj bool to isRecharging as well
                //^set this up on load
    public float rechargeTimeRemaining;

    //add stop coros method to listen for load event?

    private void Start()
    {
        //Instantiate new itemBase based of the itempacktype of the attached itemPackSO.
        //In case of HealhPack or ShieldPack ItemPackTypes, start ItemRechargeCoro().
        switch (itemPackSO.itemPackType)
        {
            case ItemPackSO.ItemPackType.HOTPack:
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
    public IEnumerator ItemRechargeCoro(float timer = 0f)//optional param for loading based on rechargeTimeRemaining value, default to 0
    {
        isConsumed = true;
        yield return new WaitUntil(() => item.isRecharging);

        for (timer = 0f; timer < itemPackSO.rechargeTime; timer += Time.deltaTime)
        {
            rechargeTimeRemaining = itemPackSO.rechargeTime - timer;// for save/load value grabbing
            yield return null;
        }
        item.RechargeLink();
        isConsumed = false;
        StartCoroutine(ItemRechargeCoro());
    }
    public void OnLoadGameData(bool _isConsumed, float remainingRechargeTime = 0f)
    {
        StopAllCoroutines();
        isConsumed = _isConsumed;
        if (isConsumed)
        {
            this.item.OnPackConsume(this.gameObject);//turn invis
            StartCoroutine(ItemRechargeCoro(remainingRechargeTime));
        }
        else
        {
            this.item.RechargeLink(); //turn visible
        }
    }
}
