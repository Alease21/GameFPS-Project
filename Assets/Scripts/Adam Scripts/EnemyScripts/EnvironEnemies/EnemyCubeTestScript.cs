using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyCubeTestScript : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;
    public int damageToDeal;

    //Mostly for Testing. Walking into cube, player takes dmg
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerStatsScript.TakeDamage(damageToDeal);
            Debug.Log($"{damageToDeal} damage done to player");
        }
    }
}
