using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickupScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var weaponController = WeaponController.instance;
        var playerStatsScript = PlayerStatsScript.instance;
        var throwableController = ThrowableController.instance;

        if (other.tag == "ItemPickup")
        {
            ItemPackScript itemPackPickUp = other.transform.GetComponent<ItemPackScript>();
            
            switch (itemPackPickUp.itemPackSO.itemPackType)
            {
                //If item is of ItemPackType HealthPack or ShieldPack: check if item is currently recharging, check if player can be healed/shielded.
                //if true then consume item and call OnPackConsume().
                case ItemPackSO.ItemPackType.HealthPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        if (playerStatsScript.GetHealed(itemPackPickUp.itemPackSO.packAmount))
                        {
                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            Debug.Log("Health Pack used");
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.HOTPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        //send initial heal ticks, if player can be healed then start coro heal the remainder of HOT amount over numTicks intervals
                        //int initHeal = itemPackPickUp.itemPackSO.packAmount / itemPackPickUp.itemPackSO.numTicks;
                        if (playerStatsScript.GetHealed(itemPackPickUp.itemPackSO.packAmount))
                        {
                            StartCoroutine(playerStatsScript.HOTHealCoro(itemPackPickUp.itemPackSO.tickTime, 
                                itemPackPickUp.itemPackSO.numTicks - 1, // -1 to numTicks b/c of initial heal done in header

                                itemPackPickUp.itemPackSO.packAmount));
                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            Debug.Log("HOT Pack used");
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.ShieldPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        if (playerStatsScript.GetShielded(itemPackPickUp.itemPackSO.packAmount))
                        {
                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            Debug.Log("Shield Pack used");
                        }
                    }
                    break;

                //If item is of ItemPackType ammoPack: check if each weapon (if player currently has) ammoCount < ammoMax.
                //if true, call onPackConsume(), then call AmmoGet() for each weapon and update ammo stats
                case ItemPackSO.ItemPackType.AmmoPack:
                    if (weaponController.weapon1?.ammoCount < weaponController.weapon1?.ammoMax ||
                        weaponController.weapon2?.ammoCount < weaponController.weapon2?.ammoMax ||
                        weaponController.weapon3?.ammoCount < weaponController.weapon3?.ammoMax)
                    {
                        //ammo pack affects all guns player currently has
                        weaponController.weapon1?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                        weaponController.weapon2?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                        weaponController.weapon3?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                        //

                        itemPackPickUp.item.OnPackConsume(other.gameObject);
                        weaponController.AmmoStatUpdater();
                        Debug.Log("Ammo Pack used");
                    }
                    break;
            }

            //update ui every item collision, whether item is consumed or not.
            InventoryController.instance.UIUpdateEvent?.Invoke();
        }
        if (other.tag == "WeaponPickup")
        {
            WeaponSO weaponPickUpSO = other.transform.GetComponent<WeaponScript>().weaponSO;

            //If weapon pickup is of certain Weapontype (cases), check if player has this weapon or not. If not, call weaponPrefabSpawn() to instantiate the weapon and
            //destroy item on successful pickup
            switch (weaponPickUpSO.weaponType)
            {
                case WeaponSO.WeaponType.HitScan:
                    Debug.Log(weaponController.hasHitScan ? "I already have this weapon." : "HitScan weapon picked up");
                    if (!weaponController.hasHitScan)
                    {
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.HitScan, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount, weaponPickUpSO.damage, weaponPickUpSO.range);
                    }
                    break;
                case WeaponSO.WeaponType.Projectile:
                    Debug.Log(weaponController.hasProjectile ? "I already have this weapon." : "Projectile weapon picked up");
                    if (!weaponController.hasProjectile)
                    {
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.Projectile, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount, weaponPickUpSO.damage, weaponPickUpSO.range);
                    }
                    break;
                case WeaponSO.WeaponType.Continuous:
                    Debug.Log(weaponController.hasContinuous ? "I already have this weapon." : "Continuous weapon picked up");
                    if (!weaponController.hasContinuous)
                    {
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.Continuous, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount, weaponPickUpSO.damage, weaponPickUpSO.range);
                    }
                    break;
                case WeaponSO.WeaponType.Grenade:
                case WeaponSO.WeaponType.SmokeBomb:
                    if (throwableController.ThrowableGet(weaponPickUpSO.weaponType, weaponPickUpSO.ammoCount, weaponPickUpSO.ammoMax, weaponPickUpSO.damage, weaponPickUpSO.range, weaponPickUpSO.explodeTime))
                    {
                        throwableController.ThrowableCountUpdater();
                        InventoryController.instance.OnWeaponSwap();
                    }
                    else
                    {
                        Debug.Log("I don't need any of those throwables");
                    }
                    break;
            }
            other.GetComponent<WeaponScript>().OnPickUp();//move to weapon base scripts?

            //update ui every weapon collision, whether weapon is consumed or not.
            InventoryController.instance.UIUpdateEvent?.Invoke();
        }
    }
}
