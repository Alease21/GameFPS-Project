using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GUIDRegistry
{
    private static Dictionary<string, Tuple<MyGUID.GUIDObjectType, Transform>> _registry = new Dictionary<string, Tuple<MyGUID.GUIDObjectType, Transform>>();
    public static Dictionary<string, Tuple<MyGUID.GUIDObjectType, Transform>> GetRegistry { get { return _registry; } }

    public static void Register(string key, Tuple<MyGUID.GUIDObjectType, Transform> valueTuple)
    {
        if (_registry.ContainsKey(key))
        {
            _registry[key] = valueTuple;
        }
        else
        {
            _registry.Add(key, valueTuple);
        }
    }

    public static Tuple<MyGUID.GUIDObjectType, Transform> GetTransformFromKey(string key)
    {
        if (_registry.ContainsKey(key))
        {
            return _registry[key];
        }
        return null;
    }
}
