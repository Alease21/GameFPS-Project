using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GUIDRegistry
{
    private static Dictionary<string, GameObject> _registry = new Dictionary<string, GameObject>();

    public static Dictionary<string, GameObject> GetRegistry { get { return _registry; } }

    public static void Register(Dictionary<string,GameObject> tempRegistry)
    {
        for (int i = 0; i < tempRegistry.Count; i++)
        {
            if (_registry.ContainsKey(tempRegistry.ElementAt(i).Key))
            {
                _registry[tempRegistry.ElementAt(i).Key] = tempRegistry.ElementAt(i).Value;
            }
            else
            {
                _registry.Add(tempRegistry.ElementAt(i).Key, tempRegistry.ElementAt(i).Value);
            }
        }
    }

    public static GameObject GetGameObjectFromKey(string key)
    {
        if (_registry.ContainsKey(key))
        {
            return _registry[key];
        }
        return null;
    }
}
