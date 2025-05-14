using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour
{
    private EnemyScript enemyScript;
    private NavMeshAgent navMeshAgent;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        GotShot
    }
    private EnemyState enemyState;
    public EnemyState _EnemyState { get { return enemyState; } private set { enemyState = value; } }

    public GameObject[] patrolPoints;
    public GameObject playerTarget;

    [SerializeField ]private bool isIdle,
                                  isPatroling,
                                  isAttacking;
    private int patrolIndex = 0; //find better solution?
    [SerializeField] private bool playerSeen = false;

    private Vector3 playerTargetDir;
    private float playerTargetDist;

    private Vector3 raycastOffset = new Vector3(0, 1, 0); // for to shoot the raycast from center of enemy


    public bool IsIdle { get { return isIdle; } private set { isIdle = value; } }
    public bool IsPatroling { get { return isPatroling; } private set { isPatroling = value; } }
    public bool IsAttacking { get { return isAttacking; } private set { isAttacking = value; } }
    public int PatrolIndex { get { return patrolIndex; } private set { patrolIndex = value; } }
    public bool PlayerSeen { get { return playerSeen; } private set { playerSeen = value; } }

    [HideInInspector] public bool gotShot;

    private void Start()
    {
        isIdle = false;
        enemyState = EnemyState.Idle;
        enemyScript = GetComponent<EnemyScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        // continuously calculate player direction and distance with respect to the enemy
        playerTargetDir = playerTarget.transform.position - transform.position;
        playerTargetDist = playerTargetDir.magnitude;

        Vector3 raycastStart = transform.position + raycastOffset;
        RaycastHit hit;
        Physics.Raycast(raycastStart, playerTargetDir.normalized, out hit);
        //Debug.DrawRay(raycastStart, playerTargetDir.normalized, Color.green, 1);

        if (gotShot)
        {
            var rotation = Quaternion.LookRotation(playerTargetDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

            enemyState = EnemyState.Idle;
            gotShot = false;
        }
        // If raycast hits player, and player is within enemy FOV, rotate to face player and swap state to attack
        if (hit.transform != null)
        {
            if (hit.transform.GetComponent<PlayerStatsScript>() && !PlayerStatsScript.instance.isHidden)
            {
                if (playerTargetDist < enemyScript.enemyViewDist &&
                    Vector3.Angle(transform.forward, playerTargetDir.normalized) < enemyScript.enemyFOV / 2)
                {
                    playerSeen = true;

                    var rotation = Quaternion.LookRotation(playerTargetDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 1);

                    if (enemyState != EnemyState.Attack)
                    {
                        enemyState = EnemyState.Chase;
                        enemyScript.OnPlayerSpotted?.Invoke();
                    }
                    if (isIdle)
                    {
                        isIdle = false;
                    }
                    if (isPatroling)
                    {
                        isPatroling = false;
                    }
                }
            }
        }

        //Depending on current state, perform relevant actions
        switch (enemyState)
        {
            case EnemyState.Idle:
                IdleActions();
                break;
            case EnemyState.Patrol:
                PatrolActions();
                break;
            case EnemyState.Chase:
                ChaseActions();
                break;
            case EnemyState.Attack:
                AttackActions();
                break;
            case EnemyState.GotShot:
                GotShotActions();
                break;
        }
    }

    // Start idle coroutine to wait specific seconds before swapping state to patrol.
    // If player is seen while idle, swap state to chase
    public void IdleActions()
    {
        if (!isIdle)
        {
            StartCoroutine(IdleCoroutine());
            isIdle = true;
        }

        if (playerSeen)
        {
            enemyState = EnemyState.Chase;
            isIdle = false;
        }
    }
    public IEnumerator IdleCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        if (enemyState == EnemyState.Idle)
        {
            enemyState = EnemyState.Patrol;
            isIdle = false;
        }
    }

    // Patrol along a set path of points (patrolPoints[]), swapping state to
    // idle once a point is reached and incrementing the patrolIndex.
    // If player is seen during patrol, swap state to chase
    public void PatrolActions()
    {
        if (isPatroling)
        {
            if (!navMeshAgent.hasPath)
            {
                enemyState = EnemyState.Idle;
                isPatroling = false;
                patrolIndex++;
                if (patrolIndex >= patrolPoints.Length)
                {
                    patrolIndex = 0;
                }
            }
        }
        else
        {
            navMeshAgent.SetDestination(patrolPoints[patrolIndex].transform.position);
            isPatroling = true;
        }

        if (playerSeen)
        {
            enemyState = EnemyState.Chase;
            isPatroling = false;
        }
    }

    // Snapshot player position when seen and set as destination, then swap
    // state to attack if within weapon range, or swap state to idle if player
    // isnt found
    public void ChaseActions()
    {
        if (playerSeen)
        {
            Vector3 playerSnapShot = playerTarget.transform.position;

            if ((playerSnapShot - transform.position).magnitude < enemyScript.weaponSO.range)
            {
                enemyState = EnemyState.Attack;
            }
            else
            {
                navMeshAgent.SetDestination(playerSnapShot);
                playerSeen = false;
            }
        }
        else if (!playerSeen && !navMeshAgent.hasPath)
        {
            enemyState = EnemyState.Idle;
        }
    }

    //Start AttackCoro
    public void AttackActions()
    {
        if (!isAttacking)
        {
            StartCoroutine(AttackCoro());
            isAttacking = true;
        }
    }

    // Coroutine to trigger OnEnemyAttack event, clear navmesh path, then resets
    // IsAttacking and playerSeen bools to false before swapping state to chase
    public IEnumerator AttackCoro()
    {
        enemyScript.OnEnemyAttack?.Invoke();
        navMeshAgent.ResetPath();

        yield return new WaitForSecondsRealtime(enemyScript.weaponSO.attackAnimation.length);
        isAttacking = false;
        playerSeen = false;
        enemyState = EnemyState.Chase;
    }

    public void GotShotActions()
    {

    }
    public void OnLoadGameData(int[] iArray, bool[] bArray)
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            navMeshAgent.ResetPath();//fix this so playersnapshot is properly set
                                     //(rework enemy raycast?)
        }

        enemyState = (EnemyState)iArray[1];//casting to use int as index

        //fix how these coros are calling in idle/attack actions so this isnt required?
        if (enemyState == EnemyState.Idle)
        {
            StartCoroutine(IdleCoroutine());
        }
        else if (enemyState == EnemyState.Patrol)
        {
            navMeshAgent.SetDestination(patrolPoints[patrolIndex].transform.position);
        }
        else if (enemyState == EnemyState.Attack)
        {
            StartCoroutine(AttackCoro());
        }
        playerSeen = bArray[3];
        patrolIndex = iArray[0];
        IsIdle = bArray[0];
        IsPatroling = bArray[1];
        IsAttacking = bArray[2];
    }
}
