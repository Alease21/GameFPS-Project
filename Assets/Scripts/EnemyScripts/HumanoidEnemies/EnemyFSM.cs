using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour
{
    private EnemyScript enemyScript;
    private NavMeshAgent navMeshAgent;

    public UnityEvent OnEnemyAttack;

    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        GotShot
    }
    private EnemyState enemyState;

    public GameObject[] patrolPoints;
    private bool isIdle;
    private bool isPatroling;
    private bool isAttacking;
    private int patrolIndex = 0; //find better solution?

    public GameObject playerTarget;
    private bool playerSeen = false;

    private Vector3 playerTargetDir;
    private float playerTargetDist;

    private Vector3 raycastOffset = new Vector3(0, 1, 0);

    public bool gotShot;

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
        OnEnemyAttack?.Invoke();
        navMeshAgent.ResetPath();

        yield return new WaitForSecondsRealtime(enemyScript.weaponSO.attackAnimation.length);
        isAttacking = false;
        playerSeen = false;
        enemyState = EnemyState.Chase;
    }

    public void GotShotActions()
    {

    }
}
