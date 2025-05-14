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

        bool gotConsumed = false;

        if (other.GetComponent<ItemPackScript>())
        {
            ItemPackScript itemPackPickUp = other.transform.GetComponent<ItemPackScript>();
            
            switch (itemPackPickUp.ItemPackSO.itemPackType)
            {
                //If item is of ItemPackType HealthPack or ShieldPack: check if item is currently recharging, check if player can be healed/shielded.
                //if true then consume item and call OnPackConsume().
                case ItemPackSO.ItemPackType.HealthPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        if (playerStatsScript.GetHealed(itemPackPickUp.ItemPackSO.packAmount))
                        {
                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            //Debug.Log("Health Pack used");

                            gotConsumed = true;
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.HOTPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        //send initial heal ticks, if player can be healed then start coro heal the remainder of HOT amount over numTicks intervals
                        //int initHeal = itemPackPickUp.itemPackSO.packAmount / itemPackPickUp.itemPackSO.numTicks;
                        if (playerStatsScript.GetHealed(itemPackPickUp.ItemPackSO.packAmount))
                        {
                            StartCoroutine(playerStatsScript.HOTHealCoro(itemPackPickUp.ItemPackSO.tickTime, 
                                itemPackPickUp.ItemPackSO.numTicks - 1, // -1 to numTicks b/c of initial heal done in header
                                itemPackPickUp.ItemPackSO.packAmount));

                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            //Debug.Log("HOT Pack used");
                            gotConsumed = true;
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.ShieldPack:
                    if (!itemPackPickUp.item.isRecharging)
                    {
                        if (playerStatsScript.GetShielded(itemPackPickUp.ItemPackSO.packAmount))
                        {
                            itemPackPickUp.item.OnPackConsume(other.gameObject);
                            //Debug.Log("Shield Pack used");
                            gotConsumed = true;
                        }
                    }
                    break;

                //If item is of ItemPackType ammoPack: check if each weapon (if player currently has) ammoCount < ammoMax.
                //if true, call onPackConsume(), then call AmmoGet() for each weapon and update ammo stats
                case ItemPackSO.ItemPackType.AmmoPack:
                    if (weaponController.Weapon1?.ammoCount < weaponController.Weapon1?.ammoMax ||
                        weaponController.Weapon2?.ammoCount < weaponController.Weapon2?.ammoMax ||
                        weaponController.Weapon3?.ammoCount < weaponController.Weapon3?.ammoMax)
                    {
                        //ammo pack affects all guns player currently has
                        weaponController.Weapon1?.AmmoGet(itemPackPickUp.ItemPackSO.packAmount);
                        weaponController.Weapon2?.AmmoGet(itemPackPickUp.ItemPackSO.packAmount);
                        weaponController.Weapon3?.AmmoGet(itemPackPickUp.ItemPackSO.packAmount);
                        //

                        itemPackPickUp.item.OnPackConsume(other.gameObject);
                        weaponController.AmmoStatUpdater();
                        //Debug.Log("Ammo Pack used");

                        gotConsumed = true;
                    }
                    break;
            }

            if (gotConsumed)
            {
                //Send itemSO to play specfic sound on pick up
                Player_SFX_Controller.instance.OnItemPickup(itemPackPickUp.ItemPackSO);

                InventoryController.instance.UIUpdateEvent?.Invoke();            
            }
        }
        if (other.GetComponent<WeaponScript>())
        {
            WeaponSO weaponPickUpSO = other.transform.GetComponent<WeaponScript>().weaponSO;

            //switch statement to assign hasWeapon properly based on collected weapons
            bool hasWeapon = false;
            switch (weaponPickUpSO.weaponType)
            {
                case WeaponSO.WeaponType.HitScan:
                    hasWeapon = weaponController.HasHitScan;
                    break;
                case WeaponSO.WeaponType.Projectile:
                    hasWeapon = weaponController.HasProjectile;
                    break;
                case WeaponSO.WeaponType.Continuous:
                    hasWeapon = weaponController.HasContinuous;
                    break;
            }

            //If weapon pickup is of certain Weapontype (cases), check if player has this weapon or not. If not, call weaponPrefabSpawn() to instantiate the weapon and
            //destroy item on successful pickup
            switch (weaponPickUpSO.weaponType)
            {
                case WeaponSO.WeaponType.HitScan:
                case WeaponSO.WeaponType.Projectile:
                case WeaponSO.WeaponType.Continuous:
                    if (!hasWeapon)
                    {
                        weaponController.WeaponPrefabSpawn(weaponPickUpSO);
                        other.GetComponent<WeaponScript>().OnPickUp();

                        gotConsumed = true;
                    }
                    break;
                case WeaponSO.WeaponType.Grenade:
                case WeaponSO.WeaponType.SmokeBomb:
                    if (throwableController.ThrowableGet(weaponPickUpSO))
                    {
                        throwableController.ThrowableCountUpdater();
                        InventoryController.instance.OnWeaponSwap();
                        other.GetComponent<WeaponScript>().OnPickUp();

                        gotConsumed = true;
                    }
                    break;
            }

            if (gotConsumed)
            {
                Player_SFX_Controller.instance.OnWeaponPickup(weaponPickUpSO);
                InventoryController.instance.UIUpdateEvent?.Invoke();
            }
        }
    }
}
