using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class MyGUID : MonoBehaviour
{
    public enum GUIDObjectType
    {
        Player,
        Enemy,
        Weapon,
        ItemPack,
        EnvironEnemy,
        Projectile
    }
    private GUIDObjectType objType;
    [SerializeField] private string _GUID = "";
    public string GUID { get { return _GUID; } }
    private Tuple<GUIDObjectType, Transform> _objTuple;

    private void OnEnable()
    {
        EvaluateObjectType();
        EvaluateGUID();
    }

    private void EvaluateObjectType()
    {
        if (GetComponent<PlayerStatsScript>())
        {
            objType = GUIDObjectType.Player;
        }
        else if (GetComponent<EnemyScript>())
        {
            objType = GUIDObjectType.Enemy;
        }
        else if (GetComponent<WeaponScript>())
        {
            objType = GUIDObjectType.Weapon;
        }
        else if (GetComponent<ItemPackScript>())
        {
            objType = GUIDObjectType.ItemPack;
        }
        else if (GetComponent<BarrelScript>()) // change if ever add more environ enemies
        {
            objType = GUIDObjectType.EnvironEnemy;
        }
        else if (GetComponent<ProjectileScripts>())
        {
            objType = GUIDObjectType.Projectile;
        }
        else
        {
            Debug.Log("ObjType not set for " + this.gameObject.name);
        }
        _objTuple = Tuple.Create(objType, transform);
    }

    private void EvaluateGUID()
    {
        if (_GUID == string.Empty)
        {
            GenerateGUID();
        }
    }

    private void Start()
    {
        EvaluateGUID();

        if (Application.isPlaying == false)
        {
            return;
        }
        GUIDRegistry.Register(_GUID, _objTuple);
    }

    //makes identifier
    public void GenerateGUID()
    {
        _GUID = System.Guid.NewGuid().ToString();
    }
}
