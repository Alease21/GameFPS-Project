using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Events;

public class PlayerStatsScript : MonoBehaviour
{
    //Singleton setup
    public static PlayerStatsScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] private int _health;
    [SerializeField] private int _shield;
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _maxShield = 50;
    
    public int Health { get { return _health; } protected set { } }
    public int MaxHealth { get { return _maxHealth; } protected set { } }
    public int Shield { get { return _shield; } protected set { } }
    public int MaxShield { get { return _maxShield; } protected set { } }

    public int hitScanWeaponAmmo,
               projectileWeaponAmmo,
               continuousWeaponAmmo,
               maxHitscanAmmo,
               maxProjectileAmmo,
               maxContinuousAmmo;

    //public bool startupDone = false;

    private void Start()
    {
        _health = _maxHealth;
        _shield = 0;

        InventoryController.instance.UIUpdateEvent?.Invoke();
    }

    //Check if player can be healed, if amount to be healed > maxHealth, cap health at maxHealth
    //return true if player can be healed and increment health by amount, else false
    public bool GetHealed(int amount)
    {
        if (_health == _maxHealth)
        {
            return false;
        }
        else if (_health > _maxHealth || amount > (_maxHealth - _health))
        {
            _health = _maxHealth;
        }
        else
        {
            _health += amount;
        }
        InventoryController.instance.UIUpdateEvent?.Invoke();
        return true;

    }

    //Check if player can be shielded, if amount to added to shield > maxShield, cap shield at maxShield
    //return true if player can be shielded and increment shield by amount, else false
    public bool GetShielded(int amount)
    {
        if (_shield == MaxShield)
        {
            return false;
        }
        else if (_shield > _maxShield || amount >= (_maxShield - _shield))
        {
            _shield = _maxShield;
        }
        else
        {
            _shield += amount;
        }
        InventoryController.instance.UIUpdateEvent?.Invoke();
        return true;
    }

    //Decrement health by amount if health > 0, else set health to 0 and display debug message
    public void TakeDamage(int amount)
    {
        if (_shield > 0 && amount < _shield)
        {
            _shield-= amount;
        }
        else if (_shield > 0 && amount >= _shield)
        {
            _health -= amount - _shield;
            _shield = 0;
        }
        else if (_health > 0 && (_health - amount) > 0)
        {
            _health -= amount;
        }
        else
        {
            _health = 0;
            Debug.Log("health is 0 now :(");
        }
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }
}
