using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestWeaponScript : MonoBehaviour
{
    public WeaponSO weaponSO;
    public GameObject weaponPrefab;

    private void Start()
    {
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                break;
            case WeaponSO.WeaponType.Projectile:
                break;
        }
    }
}
