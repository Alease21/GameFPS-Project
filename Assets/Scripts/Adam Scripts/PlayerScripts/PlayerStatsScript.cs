using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerStatsScript : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int shield;
    [SerializeField] private int maxShield = 50;
    public int Health => health;
    public int MaxHealth => maxHealth;
    public int Shield => shield;
    public int MaxShield => maxShield;

    public int hitScanWeaponAmmo,
               projectileWeaponAmmo,
               continuousWeaponAmmo;

    public bool gotHealed,
                gotShielded;
    private void Start()
    {
        health = 100;
        shield = 0;
    }

    public bool GetHealed(int amount)
    {
        if (health > maxHealth || amount > (maxHealth - health))
        {
            health = maxHealth;
            return true;
        }
        else if (health < maxHealth)
        {
            health += amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    public bool GetShielded(int amount)
    {
        if (shield > maxShield || amount > (maxShield - shield))
        {
            shield = maxShield;
            return true;
        }
        else if (shield < maxShield)
        {
            shield += amount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void TakeDamage(int amount)
    {
        if (health > 0 && (health - amount) > 0)
        {
            health -= amount;
        }
        else
        {
            health = 0;
            Debug.Log("health is 0 now :(");
        }
    }
}
