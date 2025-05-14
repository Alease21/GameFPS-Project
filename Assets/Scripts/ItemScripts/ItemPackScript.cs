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

    [HideInInspector] public ItemBase item;

    [SerializeField] private ItemPackSO _itemPackSO;
    [SerializeField] private GameObject _itemPackPrefab;
    private bool isConsumed = false;//maybe just use item.isrecharging?
    private float rechargeTimeRemaining;

    public ItemPackSO ItemPackSO { get { return _itemPackSO; } private set { _itemPackSO = value; } }
    public GameObject ItemPackPrefab { get { return _itemPackPrefab; } private set { _itemPackPrefab = value; } }
    public bool IsConsumed { get { return isConsumed; } private set { isConsumed = value; } }
    public float RechargeTimeRemaining { get { return rechargeTimeRemaining; } private set { rechargeTimeRemaining = value; } }

    private void Start()
    {
        //Instantiate new itemBase based of the itempacktype of the attached itemPackSO.
        //In case of HealhPack or ShieldPack ItemPackTypes, start ItemRechargeCoro().
        switch (_itemPackSO.itemPackType)
        {
            case ItemPackSO.ItemPackType.HOTPack:
            case ItemPackSO.ItemPackType.HealthPack:
                item = new HealthItem { packAmount = _itemPackSO.packAmount, packPrefab = _itemPackPrefab };

                StartCoroutine(ItemRechargeCoro());
                break;
            case ItemPackSO.ItemPackType.ShieldPack:
                item = new ShieldItem { packAmount = _itemPackSO.packAmount, packPrefab = _itemPackPrefab };

                StartCoroutine(ItemRechargeCoro());
                break;
            case ItemPackSO.ItemPackType.AmmoPack:
                item = new AmmoItem { packAmount = _itemPackSO.packAmount, packPrefab = _itemPackPrefab };

                StartCoroutine(ItemDisableCoro());
                break;
        }
    }

    //Coroutine to wait until instantiated item bool isRecharging is true, then
    //wait rechargeTime(from attached itemSO) seconds before calling RechargeLink
    //from item and restarting the coroutine
    public IEnumerator ItemRechargeCoro(float timer = 0f)//optional param for loading based on rechargeTimeRemaining value, default to 0
    {
        rechargeTimeRemaining = 0f;

        yield return new WaitUntil(() => item.isRecharging);
        isConsumed = true;

        for ( ; timer < _itemPackSO.rechargeTime; timer += Time.deltaTime)
        {
            rechargeTimeRemaining = timer;// for save/load value grabbing
            yield return null;
        }
        item.RechargeLink();
        isConsumed = false;
        StartCoroutine(ItemRechargeCoro());
    }
    public IEnumerator ItemDisableCoro()
    {
        yield return new WaitUntil(() => item.isConsumed);
        isConsumed = true;
    }
    public void ItemEnableOnLoad()
    {
        if (!isConsumed)
        {
            item.RechargeLink();
        }
    }
    public void OnLoadGameData(bool _isConsumed, float remainingRechargeTime = 0f)
    {
        StopAllCoroutines();
        isConsumed = _isConsumed;
        if (isConsumed)
        {
            if (item is iRechargeableItem)
            {
                this.item.OnPackConsume(this.gameObject);//turn invis
                StartCoroutine(ItemRechargeCoro(remainingRechargeTime));
            }
        }
        else
        {
            this.item.RechargeLink(); //turn visible
            StartCoroutine((item is iRechargeableItem) ? ItemRechargeCoro() : ItemDisableCoro());
            ItemEnableOnLoad();
        }
    }
}
