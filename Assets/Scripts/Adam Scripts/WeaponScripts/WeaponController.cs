using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    public WeaponBase myWeapon;
    public Transform shootPoint;
    public GameObject projectilePreFab;

    public UnityEvent hitScanSwap;
    public UnityEvent projectileSwap;

    void Start()
    {
        myWeapon = new Gun(new RaycastBehavior());
        myWeapon.shootPoint = shootPoint;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hitScanSwap?.Invoke();

            Debug.Log("HitScan Weapon Loaded");
            myWeapon.SetWeaponBehavior(new RaycastBehavior());
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            projectileSwap?.Invoke();

            Debug.Log("Projectile Weapon Loaded");
            myWeapon.SetWeaponBehavior(new ProjectileBehavior { projectilePrefab = projectilePreFab });
        }
        if (Input.GetMouseButtonDown(0))
        {
            myWeapon.Use();
        }
    }
}