using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickupScript : MonoBehaviour
{
    public WeaponController weaponController;
    public PlayerStatsScript playerStatsScript;

    private void OnTriggerEnter(Collider other)
    {
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
                        //just for testing so ammo pack affects all guns player currently has
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
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.HitScan, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount);
                        Destroy(other.gameObject);
                    }
                    break;
                case WeaponSO.WeaponType.Projectile:
                    Debug.Log(weaponController.hasProjectile ? "I already have this weapon." : "Projectile weapon picked up");
                    if (!weaponController.hasProjectile)
                    {
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.Projectile, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount);
                        Destroy(other.gameObject);
                    }
                    break;
                case WeaponSO.WeaponType.Continuous:
                    Debug.Log(weaponController.hasContinuous ? "I already have this weapon." : "Continuous weapon picked up");
                    if (!weaponController.hasContinuous)
                    {
                        weaponController.WeaponPrefabSpawn(WeaponSO.WeaponType.Continuous, weaponPickUpSO.ammoMax, weaponPickUpSO.ammoCount);
                        Destroy(other.gameObject);
                    }
                    break;
            }
        }
        //update ui stuff every collision, whether item is consumed or not.
        playerStatsScript.UiStatUpdate?.Invoke();
    }
}
