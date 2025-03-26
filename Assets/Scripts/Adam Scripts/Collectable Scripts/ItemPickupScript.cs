using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    public WeaponController weaponController;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "ItemPickup")
        {
            // wire to inventory
            Destroy(collision.gameObject);
        }
        if (collision.transform.tag == "WeaponPickup")
        {
            weaponController.HaveWeaponChecker(collision.transform.GetComponent<TestWeaponScript>().weaponSO.weaponType,
                                      collision.transform.GetComponent<TestWeaponScript>().weaponPrefab);
            weaponController.GeneralSpawnWeapon(collision.transform.GetComponent<TestWeaponScript>().weaponSO.weaponType);

            Destroy(collision.gameObject);
        }
    }
}
