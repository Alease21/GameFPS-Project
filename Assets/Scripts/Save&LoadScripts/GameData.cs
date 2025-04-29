using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

[System.Serializable]
public class GameData
{
    private List<GUIDObjectToken> _guidObjects = new List<GUIDObjectToken>();
    public List<GUIDObjectToken> GetGUIDObjects { get { return _guidObjects; } }

    public GameData()
    {
        foreach (KeyValuePair<string, GameObject> token in GUIDRegistry.GetRegistry)
        {
            _guidObjects.Add(new GUIDObjectToken(token.Key, token.Value));
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
    private bool _isActive;//general is dead/consumed bool for objects
    private float _rechargeTimer;

    public string GetGUID { get { return _guid; } }
    public Vector3 GetPosition { get { return _position.GetVector; } }
    public Vector3 GetRotation { get { return _rotation.GetVector; } }

    public GUIDObjectToken(string guid, GameObject go)
    {
        _guid = guid;
        _position = new Vector3Token(go.transform.position);
        _rotation = new Vector3Token(go.transform.rotation.eulerAngles);
        _isActive = GOBoolChecker(go); //recharge timers handled inside
    }

    //Check if gameobject is rechargeable item (if true - grab recharge time with RechargeChecker()),
    //if object is consumed/inactive
    public bool GOBoolChecker(GameObject go)
    {
        if (go.GetComponent<ItemPackScript>())
        {
            RechargeChecker(go);//condense into 1 call? if isconsumed, return bool and call RechargeChecker()
            return go.GetComponent<ItemPackScript>().isConsumed ? false : true;
        }
        else
        {
            return go.activeInHierarchy ? true : false;
        }
    }
    public void RechargeChecker(GameObject go)
    {
        ItemPackScript objItemScript = go.GetComponent<ItemPackScript>();

        if (objItemScript is iRechargeableItem && objItemScript.isConsumed)
        {
            _rechargeTimer = objItemScript.rechargeTimeRemaining;
        }
    }

    public void LoadGUID()
    {
        GameObject obj = GUIDRegistry.GetGameObjectFromKey(_guid);
        obj.transform.position = _position.GetVector;
        obj.transform.rotation = Quaternion.Euler(_rotation.GetVector);
        //apply bool? setactive
        //apply recharge timer?
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
