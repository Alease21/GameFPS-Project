using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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
                    _guidObjects.Add(new GUIDPlayerToken(token.Key, token.Value.Item2));
                    break;
                case MyGUID.GUIDObjectType.Enemy:
                    _guidObjects.Add(new GUIDEnemyToken(token.Key, token.Value.Item2));
                    break;
                case MyGUID.GUIDObjectType.Weapon:
                    _guidObjects.Add(new GUIDWeaponToken(token.Key, token.Value.Item2));
                    break;
                case MyGUID.GUIDObjectType.ItemPack:
                    _guidObjects.Add(new GUIDItemPackToken(token.Key, token.Value.Item2));
                    break;
                case MyGUID.GUIDObjectType.EnvironEnemy:
                    _guidObjects.Add(new GUIDEnvironEnemyToken(token.Key, token.Value.Item2));
                    break;
            }
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

    public virtual void LoadGUID()
    {
        Tuple<MyGUID.GUIDObjectType, Transform> obj = GUIDRegistry.GetTupleFromKey(_guid); //item 2 is the transform in tuple

        obj.Item2.transform.position = _position.GetVector;
        obj.Item2.transform.rotation = Quaternion.Euler(_rotation.GetVector);
    }
}

[System.Serializable]
public class GUIDPlayerToken : GUIDObjectToken
{
    //Player Stats data
    private float _health,
                  _maxHealth,
                  _shield,
                  _maxShield;
    float[] PSSFloatArray;

    int _hitScanWeaponAmmo,
        _projectileWeaponAmmo,
        _continuousWeaponAmmo,
        _maxHitscanAmmo,
        _maxProjectileAmmo,
        _maxContinuousAmmo,
        _grenadeCount,
        _maxGrenades,
        _smokeBombCount,
        _maxSmokeBombs;
    int[] PSSIntArray;

    // weaponcontroller data
    private bool _isHitScan,
                 _isProjectile,
                 _isContinuous,
                 _hasHitScan,
                 _hasProjectile,
                 _hasContinuous,
                 _isUnarmed;
    bool[] WCBoolArray;

    //throwablecontroller data
    private bool _hasGrenade,
                 _hasSmokeBomb,
                 _isGrenade,
                 _isSmokeBomb;
    bool[] TCBoolArray;

    //cam data
    private Vector3Token _camRotation;
    private float _camMouseX;
    private float _camMouseY;

    public GUIDPlayerToken(string guid, Transform t) : base(guid, t)
    {
        PlayerStatsScript ps = PlayerStatsScript.instance;
        WeaponController wc = WeaponController.instance;
        ThrowableController tc = ThrowableController.instance;
        CameraMovement cm = t.GetComponent<CameraMovement>();
        Transform ct = t.GetComponentInChildren<Camera>().transform;

        _health = ps.Health;
        _maxHealth = ps.MaxHealth;
        _shield = ps.Shield;
        _maxShield = ps.MaxShield;
        PSSFloatArray = new float[] { _health,
                                      _maxHealth,
                                      _shield,
                                      _maxShield};

        _hitScanWeaponAmmo = ps.hitScanWeaponAmmo;
        _projectileWeaponAmmo = ps.projectileWeaponAmmo;
        _continuousWeaponAmmo = ps.continuousWeaponAmmo;
        _maxHitscanAmmo = ps.maxHitscanAmmo;
        _maxProjectileAmmo = ps.maxProjectileAmmo;
        _maxContinuousAmmo = ps.maxContinuousAmmo;
        _grenadeCount = ps.grenadeCount;
        _maxGrenades = ps.maxGrenades;
        _smokeBombCount = ps.smokeBombCount;
        _maxSmokeBombs = ps.maxSmokeBombs;
        //this is horrible please make me better
        PSSIntArray = new int[] { _hitScanWeaponAmmo, _projectileWeaponAmmo, _continuousWeaponAmmo,
                                  _maxHitscanAmmo, _maxProjectileAmmo, _maxContinuousAmmo,
                                  _grenadeCount, _maxGrenades,
                                  _smokeBombCount, _maxSmokeBombs };

        _isHitScan = wc.IsHitScan;
        _isProjectile = wc.IsProjectile;
        _isContinuous = wc.IsContinuous;
        _hasHitScan = wc.HasHitScan;
        _hasProjectile = wc.HasProjectile;
        _hasContinuous = wc.HasContinuous;
        _isUnarmed = wc.IsUnarmed;
        //this is horrible please make me better
        WCBoolArray = new bool[] { _isHitScan, _isProjectile, _isContinuous,
                                   _hasHitScan, _hasProjectile, _hasContinuous,
                                   _isUnarmed};

        _hasGrenade = tc.HasGrenade;
        _hasSmokeBomb = tc.HasSmokeBomb;
        _isGrenade = tc.IsGrenade;
        _isSmokeBomb = tc.IsSmokeBomb;
        //this is horrible please make me better
        TCBoolArray = new bool[] { _hasGrenade, _hasSmokeBomb,
                                   _isGrenade, _isSmokeBomb};

        _camRotation = new Vector3Token(ct.rotation.eulerAngles.x, ct.rotation.eulerAngles.y, ct.rotation.eulerAngles.z);
        _camMouseX = cm.XRotate;
        _camMouseY = cm.YRotate;
    }
    public override void LoadGUID()
    {
        base.LoadGUID();
        Transform objTrans = GUIDRegistry.GetTupleFromKey(GetGUID).Item2;

        //call variable assigning method in each script to avoid protection level issues
        objTrans.GetComponent<PlayerStatsScript>().OnLoadGameData(PSSFloatArray, PSSIntArray);
        objTrans.GetComponent<WeaponController>().OnLoadGameData(WCBoolArray);
        objTrans.GetComponent<ThrowableController>().OnLoadGameData(TCBoolArray);
        objTrans.GetComponentInChildren<CameraMovement>().OnLoadGameData(_camMouseX, _camMouseY);
    }

}

[System.Serializable]
public class GUIDEnemyToken : GUIDObjectToken
{
    //enemyscript data
    private float _enemyHealth,
                 _enemyDamage;
    private bool _hasDied;

    //FSM data
    private int _patrolIndex,
                _stateIndex;
    private int[] FSMIntArray;

    private bool _isIdle,
                 _isPatroling,
                 _isAttacking,
                 _playerSeen;
    //add gotshot? revamp that system?
    private bool[] FSMBoolArray;

    //animation control data
    //private int _animState;
    //private bool _isAttackAnim;

    public GUIDEnemyToken(string guid, Transform t) : base(guid, t)
    {
        EnemyScript es = t.GetComponent<EnemyScript>();
        EnemyFSM efsm = t.GetComponent<EnemyFSM>();
        EnemyAnimationManager eam = t.GetComponent<EnemyAnimationManager>();

        _enemyHealth = es.enemyHealth;
        _enemyDamage = es.enemyDamage;
        _hasDied = es.hasDied;

        _patrolIndex = efsm.PatrolIndex;
        _stateIndex = (int)efsm._EnemyState; //cast as int to default to enum index val
        FSMIntArray = new int[] { _patrolIndex, _stateIndex};

        _isIdle = efsm.IsIdle;
        _isPatroling = efsm.IsPatroling;
        _isAttacking = efsm.IsAttacking;
        _playerSeen = efsm.PlayerSeen;
        //this is horrible please make me better
        FSMBoolArray = new bool[] { _isIdle, _isPatroling, _isAttacking, _playerSeen };

        //figure this out? how to grab current anim state index?
        //_animState = (int)eam.GetComponent<Animator>().GetCurrentAnimatorStateInfo(1).IsName()
    }
    public override void LoadGUID()
    {
        base.LoadGUID();
        Transform objTrans = GUIDRegistry.GetTupleFromKey(GetGUID).Item2;

        objTrans.GetComponent<EnemyScript>().OnLoadGameData(_enemyHealth, _enemyDamage, _hasDied);
        objTrans.GetComponent<EnemyFSM>().OnLoadGameData(FSMIntArray, FSMBoolArray);
        objTrans.GetComponent<EnemyAnimationManager>().OnLoadGameData();//just flips animator bool false, no data stored in save
    }
}

[System.Serializable]
public class GUIDWeaponToken : GUIDObjectToken
{
    private bool _isPickedUp;

    public GUIDWeaponToken(string guid, Transform t) : base(guid, t)
    {
        _isPickedUp = t.GetComponent<WeaponScript>().isPickedUp;
    }
    public override void LoadGUID()
    {
        base.LoadGUID();
        Transform objTrans = GUIDRegistry.GetTupleFromKey(GetGUID).Item2;

        objTrans.GetComponent<WeaponScript>().OnLoadGameData(_isPickedUp);
    }
}

[System.Serializable]
public class GUIDItemPackToken : GUIDObjectToken
{
    private bool _isConsumed;
    private float _rechargeTimeRemaining;

    public GUIDItemPackToken(string guid, Transform t) : base(guid, t)
    {
        _isConsumed = t.GetComponent<ItemPackScript>().isConsumed;

        if (_isConsumed)
        {
            _rechargeTimeRemaining = t.GetComponent<ItemPackScript>().rechargeTimeRemaining;
        }
    }
    public override void LoadGUID()
    {
        base.LoadGUID();
        Transform objTrans = GUIDRegistry.GetTupleFromKey(GetGUID).Item2;

        //better way to do this?
        if (_isConsumed)
        {
            objTrans.GetComponent<ItemPackScript>().OnLoadGameData(_isConsumed, _rechargeTimeRemaining);
        }
        else
        {
            objTrans.GetComponent<ItemPackScript>().OnLoadGameData(_isConsumed);
        }
    }
}

[System.Serializable]
public class GUIDEnvironEnemyToken : GUIDObjectToken
{
    float _environhealth;
    bool _hasExploded;

    public GUIDEnvironEnemyToken(string guid, Transform t) : base(guid, t)
    {
        _environhealth = t.GetComponent<BarrelScript>().Health;
        _hasExploded = t.GetComponent<BarrelScript>().HasExploded;
    }
    public override void LoadGUID()
    {
        base.LoadGUID();
        Transform objTrans = GUIDRegistry.GetTupleFromKey(GetGUID).Item2;

        objTrans.GetComponent<BarrelScript>().OnLoadGameData(_environhealth, _hasExploded);
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
