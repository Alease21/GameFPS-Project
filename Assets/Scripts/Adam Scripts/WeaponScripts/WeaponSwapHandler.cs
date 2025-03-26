using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwapHandler : MonoBehaviour
{
    public GameObject[] weaponPrefabs;
    public GameObject currWeapon;
    public Transform gunSetPoint;

    void Start()
    {
        // Testing purposes - start with hitscan weapon
        GameObject hitScanWeapon = GameObject.Instantiate(weaponPrefabs[0], gunSetPoint.position, gunSetPoint.transform.rotation);
        currWeapon = hitScanWeapon;
        hitScanWeapon.transform.parent = transform;
    }

    public void SpawnHitScanWeapon()
    {
        Destroy(currWeapon);

        GameObject hitScanWeapon = GameObject.Instantiate(weaponPrefabs[0], gunSetPoint.position, gunSetPoint.transform.rotation);
        hitScanWeapon.transform.parent = transform;
        currWeapon = hitScanWeapon;
    }
    public void SpawnProjectileWeapon()
    {
        Destroy(currWeapon);
        GameObject projectileWeapon = GameObject.Instantiate(weaponPrefabs[1], gunSetPoint.position, gunSetPoint.transform.rotation);
        projectileWeapon.transform.parent = transform;
        currWeapon = projectileWeapon;
    }
}
