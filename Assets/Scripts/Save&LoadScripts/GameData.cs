using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private List<GUIDObjectToken> _guidObjects = new List<GUIDObjectToken>();
    public List<GUIDObjectToken> GetGUIDObjects { get { return _guidObjects; } }

    public GameData()
    {
        foreach (KeyValuePair<string, Tuple<MyGUID.GUIDObjectType, Transform>> token in GUIDRegistry.GetRegistry)
        {
            //add type sorting here
            switch (token.Value.Item1)
            {
                case MyGUID.GUIDObjectType.Player:
                    break;
                case MyGUID.GUIDObjectType.Enemy:
                    break;
                case MyGUID.GUIDObjectType.Weapon:
                    break;
                case MyGUID.GUIDObjectType.ItemPack:
                    break;
                case MyGUID.GUIDObjectType.EnvironEnemy:
                    break;
            }
            _guidObjects.Add(new GUIDObjectToken(token.Key, token.Value.Item2));
        }
    }

    public void LoadData()
    {
        foreach (GUIDObjectToken token in _guidObjects)
        {
            token.LoadGUID();
        }
    }
}

[System.Serializable]
public class GUIDObjectToken
{
    private string _guid;
    private Vector3Token _position;
    private Vector3Token _rotation;

    public string GetGUID { get { return _guid; } }
    public Vector3 GetPosition { get { return _position.GetVector; } }
    public Vector3 GetRotation { get { return _rotation.GetVector; } }

    public GUIDObjectToken(string guid, Transform t)
    {
        _guid = guid;
        _position = new Vector3Token(t.position);
        _rotation = new Vector3Token(t.rotation.eulerAngles);
    }

    public void LoadGUID()
    {
        Transform obj = GUIDRegistry.GetTransformFromKey(_guid).Item2; //item 2 is the transform in tuple
        obj.transform.position = _position.GetVector;
        obj.transform.rotation = Quaternion.Euler(_rotation.GetVector);
    }
}

public class GUIDPlayerToken : GUIDObjectToken
{
    //Statsscript data
    private float _health,
                  _maxHealth,
                  _shield,
                  _maxShield;

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
    // weaponcontroller data
    private bool _isHitScan,
                 _isProjectile,
                 _isContinuous,
                 _hasHitScan,
                 _hasProjectile,
                 _hasContinuous,
                 _isUnarmed;
    //throwablecontroller data
    private bool _hasGrenade,
                 _hasSmokeBomb,
                 _isGrenade,
                 _isSmokeBomb;
    //cam data
    private Vector3Token camPos; 
    private Vector3Token camRot; 

    GUIDPlayerToken(string guid, Transform t) : base(guid,t)
    {
        PlayerStatsScript ps = PlayerStatsScript.instance;
        WeaponController wc = WeaponController.instance;
        ThrowableController tc = ThrowableController.instance;
        Transform camTrans = t.GetComponent<Camera>().transform;

        _health = ps.Health;
        _maxHealth = ps.MaxHealth;
        _shield = ps.Shield;
        _maxShield = ps.MaxShield;

        hitScanWeaponAmmo = ps.hitScanWeaponAmmo;
        projectileWeaponAmmo = ps.projectileWeaponAmmo;
        continuousWeaponAmmo = ps.continuousWeaponAmmo;
        maxHitscanAmmo = ps.maxHitscanAmmo;
        maxProjectileAmmo = ps.maxProjectileAmmo;
        maxContinuousAmmo = ps.maxContinuousAmmo;
        grenadeCount = ps.grenadeCount;
        maxGrenades = ps.maxGrenades;
        smokeBombCount = ps.smokeBombCount;
        maxSmokeBombs = ps.maxSmokeBombs;

        _isHitScan = wc.IsHitScan;
        _isProjectile = wc.IsProjectile;
        _isContinuous = wc.IsContinuous;
        _hasHitScan = wc.HasHitScan;
        _hasProjectile = wc.HasProjectile;
        _hasContinuous = wc.HasContinuous;
        _isUnarmed = wc.IsUnarmed;

        _hasGrenade = tc.HasGrenade;
        _hasSmokeBomb = tc.HasSmokeBomb;
        _isGrenade = tc.IsGrenade;
        _isSmokeBomb = tc.IsSmokeBomb;

        //camPos = new Vector3Token(camTrans.)
    }
}
public class GUIDEnemyToken : GUIDObjectToken
{
    //enemyscript data
    public float enemyHealth,
                 enemyDamage;
    public bool hasDied;

    GUIDEnemyToken(string guid, Transform t) : base(guid,t)
    {
        enemyHealth = t.GetComponent<EnemyScript>().enemyHealth;
        enemyDamage = t.GetComponent<EnemyScript>().enemyDamage;
        hasDied = t.GetComponent<EnemyScript>().hasDied;
    }
}
public class GUIDWeaponToken : GUIDObjectToken
{
    public bool isPickedUp;

    GUIDWeaponToken(string guid, Transform t) : base(guid,t)
    {
        isPickedUp = t.GetComponent<WeaponScript>().isPickedUp;
    }
}
public class GUIDItemPackToken : GUIDObjectToken
{
    public bool isConsumed;
    public float rechargeTimeRemaining;

    GUIDItemPackToken(string guid, Transform t) : base(guid,t)
    {
        isConsumed = t.GetComponent<ItemPackScript>().isConsumed;
        if (isConsumed)
        {
            rechargeTimeRemaining = t.GetComponent<ItemPackScript>().rechargeTimeRemaining;
        }
    }
}
public class GUIDEnvironEnemyToken : GUIDObjectToken
{
    public float health;
    public bool hasExploded;

    GUIDEnvironEnemyToken(string guid, Transform t) : base(guid,t)
    {
        health = t.GetComponent<BarrelScript>().Health;
        hasExploded = t.GetComponent<BarrelScript>().HasExploded;
    }
}
[System.Serializable]
public class Vector3Token
{
    private float _x;
    private float _y;
    private float _z;

    public Vector3 GetVector { get { return new Vector3(_x, _y, _z); } }

    public Vector3Token(float x, float y, float z)
    {
        _x = x;
        _y = y;
        _z = z;
    }

    public Vector3Token(Vector3 vector)
    {
        _x = vector.x;
        _y = vector.y;
        _z = vector.z;
    }
}
