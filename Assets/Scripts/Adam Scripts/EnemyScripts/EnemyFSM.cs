using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }
    public EnemyState enemyState;

    private void Start()
    {
        enemyState = EnemyState.Idle;
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                Debug.Log("Idle State");
                break;
            case EnemyState.Patrol:
                Debug.Log("Patrol State");
                break;
            case EnemyState.Chase:
                Debug.Log("Chase State");
                break;
        }
    }
}
