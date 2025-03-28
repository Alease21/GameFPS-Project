using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                case ItemPackSO.ItemPackType.HealthPack:
                    if (playerStatsScript.Health != playerStatsScript.MaxHealth)
                    {
                        if (!itemPackPickUp.item.isRecharging)
                        {
                            if (playerStatsScript.GetHealed(itemPackPickUp.itemPackSO.packAmount))
                            {

                                itemPackPickUp.item.OnPackConsume(other.gameObject);
                                Debug.Log("Health Pack used");
                            }
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.ShieldPack:
                    if (playerStatsScript.Shield != playerStatsScript.MaxShield)
                    {
                        if (!itemPackPickUp.item.isRecharging)
                        {
                            if (playerStatsScript.GetShielded(itemPackPickUp.itemPackSO.packAmount))
                            {
                                itemPackPickUp.item.OnPackConsume(other.gameObject);
                                Debug.Log("Shield Pack used");
                            }
                        }
                    }
                    break;
                case ItemPackSO.ItemPackType.AmmoPack:
                    //just for testing so ammo pack affects all guns player has
                    weaponController.weapon1?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                    weaponController.weapon2?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                    weaponController.weapon3?.AmmoGet(itemPackPickUp.itemPackSO.packAmount);
                    //

                    itemPackPickUp.item.OnPackConsume(other.gameObject);
                        weaponController.AmmoStatUpdater();
                        Debug.Log("Ammo Pack used");
                    break;
            }
        }
        if (other.tag == "WeaponPickup")
        {
            WeaponSO weaponPickUpSO = other.transform.GetComponent<WeaponScript>().weaponSO;

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
    }
}
