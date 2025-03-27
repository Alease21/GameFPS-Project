using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponSO weaponSO;
    public GameObject weaponPrefab;
    public IWeaponBehavior weaponBehavior;

    private void Start()
    {
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                weaponBehavior = new HitScanBehavior();
                break;
            case WeaponSO.WeaponType.Projectile:
                weaponBehavior = new ProjectileBehavior();
                break;
            case WeaponSO.WeaponType.Continuous:
                weaponBehavior = new ContinuousBehavior();
                break;
        }
    }
}
