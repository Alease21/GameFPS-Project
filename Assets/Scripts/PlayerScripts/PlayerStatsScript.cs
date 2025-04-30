using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MyGUID))]
public class PlayerStatsScript : MonoBehaviour
{
    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }
    }

    //Singleton setup
    public static PlayerStatsScript instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    [SerializeField] private float _health,
                                   _shield,
                                   _maxHealth = 100,
                                   _maxShield = 50;
    
    public float Health { get { return _health; } private set { _health = value; } }
    public float MaxHealth { get { return _maxHealth; } private set { _maxHealth = value; } }
    public float Shield { get { return _shield; } private set { _shield = value; } }
    public float MaxShield { get { return _maxShield; } private set { _maxShield = value; } }

    public int hitScanWeaponAmmo,
               projectileWeaponAmmo,
               continuousWeaponAmmo,
               maxHitscanAmmo,
               maxProjectileAmmo,
               maxContinuousAmmo,
               grenadeCount,
               maxGrenades,
               smokeBombCount,
               maxSmokeBombs;

    public bool isHidden;

    public Action OnTakeDamage;
    public Action OnPlayerDeath;

    private void Start()
    {
        _health = _maxHealth;
        _shield = 0;

        StartCoroutine(StartupCoro());
    }

    //Bandaid fix for startup error (error wasn't causing any issues though)
    public IEnumerator StartupCoro()
    {
        yield return new WaitForEndOfFrameUnit();
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }

    //Check if player can be healed, if amount to be healed > maxHealth, cap health at maxHealth
    //return true if player can be healed and increment health by amount, else false
    public bool GetHealed(float amount)
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
    public IEnumerator HOTHealCoro(float hotTimer, int numTicks, float amount)
    {
        while (numTicks > 0)
        {
            yield return new WaitForSecondsRealtime(hotTimer);
            GetHealed(amount);
            numTicks--;
        }
    }

    //Check if player can be shielded, if amount to added to shield > maxShield, cap shield at maxShield
    //return true if player can be shielded and increment shield by amount, else false
    public bool GetShielded(float amount)
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
    public void TakeDamage(float damage, bool useDOTDamage = false)
    {
        if (useDOTDamage)
        {
            StopCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));
            StartCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));//hard coded in ticks & tick time
        }
        if (_shield > 0 && damage < _shield)
        {
            _shield-= damage;
        }
        else if (_shield > 0 && damage >= _shield)
        {
            _health -= damage - _shield;
            _shield = 0;
        }
        else if (_health > 0 && (_health - damage) > 0)
        {
            _health -= damage;
        }
        else
        {
            _health = 0;
            Debug.Log("health is 0 now :(");
        }
        OnTakeDamage?.Invoke();
        InventoryController.instance.UIUpdateEvent?.Invoke();
    }

    public IEnumerator TakeDOTDamage(float damage, int ticks, float tickTime)
    {
        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(tickTime);
            TakeDamage(damage);
            ticks--;
        }
    }
}
