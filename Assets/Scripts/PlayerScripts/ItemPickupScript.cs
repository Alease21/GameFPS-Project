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

        if (other.GetComponent<ItemPackScript>())
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
                            //Player_SFX_Controller.instance.OnItemPickup(itemPackPickUp.itemPackSO);
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
                            //Player_SFX_Controller.instance.OnItemPickup(itemPackPickUp.itemPackSO);
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
                            //Player_SFX_Controller.instance.OnItemPickup(itemPackPickUp.itemPackSO);
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
                        //Player_SFX_Controller.instance.OnItemPickup(itemPackPickUp.itemPackSO);
                    }
                    break;
            }

            //update ui every item collision, whether item is consumed or not.
            InventoryController.instance.UIUpdateEvent?.Invoke();
        }
        if (other.GetComponent<WeaponScript>())
        {
            WeaponSO weaponPickUpSO = other.transform.GetComponent<WeaponScript>().weaponSO;

            //switch statement to assign hasWeapon properly based on collected weapons
            bool hasWeapon = false;
            switch (weaponPickUpSO.weaponType)
            {
                case WeaponSO.WeaponType.HitScan:
                    hasWeapon = weaponController.hasHitScan;
                    break;
                case WeaponSO.WeaponType.Projectile:
                    hasWeapon = weaponController.hasProjectile;
                    break;
                case WeaponSO.WeaponType.Continuous:
                    hasWeapon = weaponController.hasContinuous;
                    break;
            }

            //If weapon pickup is of certain Weapontype (cases), check if player has this weapon or not. If not, call weaponPrefabSpawn() to instantiate the weapon and
            //destroy item on successful pickup
            switch (weaponPickUpSO.weaponType)
            {
                case WeaponSO.WeaponType.HitScan:
                case WeaponSO.WeaponType.Projectile:
                case WeaponSO.WeaponType.Continuous:
                    Debug.Log(hasWeapon ? "I already have this weapon." : $"{weaponPickUpSO.weaponType} weapon picked up");
                    if (!hasWeapon)
                    {
                        weaponController.WeaponPrefabSpawn(weaponPickUpSO);
                        //Player_SFX_Controller.instance.OnWeaponPickup(weaponPickUpSO);
                        other.GetComponent<WeaponScript>().OnPickUp();
                    }
                    break;
                case WeaponSO.WeaponType.Grenade:
                case WeaponSO.WeaponType.SmokeBomb:
                    if (throwableController.ThrowableGet(weaponPickUpSO))
                    {
                        throwableController.ThrowableCountUpdater();
                        InventoryController.instance.OnWeaponSwap();
                        //Player_SFX_Controller.instance.OnWeaponPickup(weaponPickUpSO);
                        other.GetComponent<WeaponScript>().OnPickUp();
                    }
                    else
                    {
                        Debug.Log("I don't need any of those throwables");
                    }
                    break;
            }
            //update ui every weapon collision, whether weapon is consumed or not.
            InventoryController.instance.UIUpdateEvent?.Invoke();
        }
    }
}
