using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelInRangeScript : MonoBehaviour
{
    private BarrelScript barrelScript;

    private void Start()
    {
        barrelScript = GetComponentInParent<BarrelScript>();
        EnemyDeathManager.instance.onEnemyDeath += InRangeCleanup;
    }
    public void InRangeCleanup()
    {
        // Check for destroyed/null elements in list, then delete if any are found
        for (int i = 0; i < barrelScript.inRangeColliders.Count; i++)
        {
            if (barrelScript.inRangeColliders[i] == null || barrelScript.inRangeColliders[i].IsDestroyed())
            {
                barrelScript.inRangeColliders.Remove(barrelScript.inRangeColliders[i]);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (!barrelScript.inRangeColliders.Contains(other.gameObject))
            {
                barrelScript.inRangeColliders.Add(other.gameObject);
            }
            Debug.Log($"objs in range: " + barrelScript.inRangeColliders.Count);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" || other.tag == "Enemy")
        {
            if (barrelScript.inRangeColliders.Contains(other.gameObject))
            {
                barrelScript.inRangeColliders.Remove(other.gameObject);
            }
        }
    }
}
