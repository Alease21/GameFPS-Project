using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    private int health;
    public int Health => health;

    public int hitScanWeaponAmmo,
               projectileWeaponAmmo,
               continuousWeaponAmmo;

    private void Start()
    {
        health = 100;
    }
}
