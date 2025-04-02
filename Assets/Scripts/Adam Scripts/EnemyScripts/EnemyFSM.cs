using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour
{
    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }
    public EnemyState enemyState;

    public UnityEvent OnPlayerSpotted;
    public UnityEvent OnPlayerGone;

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
