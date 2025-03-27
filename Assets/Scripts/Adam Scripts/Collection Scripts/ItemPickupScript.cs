using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    public WeaponController weaponController;

    private void OnTriggerEnter(Collider other)
    {
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
