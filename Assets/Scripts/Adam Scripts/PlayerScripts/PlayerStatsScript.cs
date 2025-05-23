using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsScript : MonoBehaviour
{
    public InventoryController inventoryController;

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
               continuousWeaponAmmo,
               maxHitscanAmmo,
               maxProjectileAmmo,
               maxContinuousAmmo;

    public UnityEvent UiStatUpdate;

    private void Start()
    {
        health = 100;
        shield = 0;
        UiStatUpdate?.Invoke();
        StartCoroutine(InventoryKeyPress());
    }

    public IEnumerator InventoryKeyPress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));
        inventoryController.InventoryToggleEvent?.Invoke();

        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.I));
        StartCoroutine(InventoryKeyPress());
    }

    //Check if player can be healed, if amount to be healed > maxHealth, cap health at maxHealth
    //return true if player can be healed and increment health by amount, else false
    public bool GetHealed(int amount)
    {
        if (health == maxHealth)
        {
            return false;
        }
        else if (health > maxHealth || amount > (maxHealth - health))
        {
            health = maxHealth;
            return true;
        }
        else
        {
            health += amount;
            return true;
        }
    }

    //Check if player can be shielded, if amount to added to shield > maxShield, cap shield at maxShield
    //return true if player can be shielded and increment shield by amount, else false
    public bool GetShielded(int amount)
    {
        if (shield == MaxShield)
        {
            return false;
        }
        else if (shield > maxShield || amount >= (maxShield - shield))
        {
            shield = maxShield;
            return true;
        }
        else
        {
            shield += amount;
            return true;
        }

    }

    //Decrement health by amount if health > 0, else set health to 0 and display debug message
    public void TakeDamage(int amount)
    {
        if (shield > 0 && amount < shield)
        {
            shield-= amount;
        }
        else if (shield > 0 && amount >= shield)
        {
            health -= amount - shield;
            shield = 0;
        }
        else if (health > 0 && (health - amount) > 0)
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
