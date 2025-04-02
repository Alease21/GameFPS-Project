using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour
{
    [Header("FOV Filler Bool")]
    //filler until fov system set up to see player
    public bool playerSeen = false;
    public GameObject playerTest;
    ///********************


    public enum EnemyState
    {
        Idle,
        Patrol,
        Chase
    }
    [Space(30f)] // add space between fov filler stuff, remove me later
    public EnemyState enemyState;

    public GameObject[] patrolPoints;
    private GameObject currPoint;

    private NavMeshAgent navMeshAgent;
    private bool isIdle;
    private bool isPatroling;
    private bool isChasing;
    private int patrolIndex = 0; //find better solution

    public UnityEvent OnPlayerSpotted;
    public UnityEvent OnPlayerGone;

    private void Start()
    {
        enemyState = EnemyState.Idle;
        navMeshAgent = GetComponent<NavMeshAgent>();

        //navMeshAgent.SetDestination(patrolPoints[0].transform.position);
        //currPoint = patrolPoints[0];
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                IdleActions();
                //Debug.Log("Idle State");
                break;
            case EnemyState.Patrol:
                PatrolActions();
                //Debug.Log("Patrol State");
                break;
            case EnemyState.Chase:
                ChaseActions();
                //Debug.Log("Chase State");
                break;
        }
    }
    public void IdleActions()
    {
        if (!isIdle)
        {
            StartCoroutine(IdleCoroutine());
            isIdle = true;
        }

        //filler until fov system set up to see player
        if (playerSeen)
        {
            enemyState = EnemyState.Chase;
            isIdle = false;
        }
    }

    public void PatrolActions()
    {
        //on update check for player in sight? maybe move out of switch statement
        //otherwise set and keep patrol point destination
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

    public void ChaseActions()
    {
        if (isChasing)
        {
            if (!navMeshAgent.hasPath && !playerSeen)
            {
                enemyState = EnemyState.Idle;
            }
        }
        else
        {
            //StartCoroutine(ChaseCoroutine());
            isChasing = true;
        }

        if (playerSeen)
        {
            Vector3 playerSnapShot = playerTest.transform.position;
            navMeshAgent.SetDestination(playerSnapShot);
        }
        //set dest as snapshot play pos
        //constanly check for player in sight, if in sight, snapshot post and set dest again
        //if reach snapshot point and no player in sight, then start patrol at last patrol point
    }

    public IEnumerator IdleCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);
        if (enemyState == EnemyState.Idle)
        {
            enemyState = EnemyState.Patrol;
        }
        isIdle = false;
    }
    public IEnumerator PatrolCoroutine()
    {
        navMeshAgent.SetDestination(patrolPoints[patrolIndex].transform.position);

        //yield return new WaitUntil(() => navMeshAgent.remainingDistance < destinationAllowance && isPatroling);
        if ( !navMeshAgent.hasPath && isPatroling)
        {
            enemyState = EnemyState.Idle;
            patrolIndex++;
            yield return null;
        }
    }
    public IEnumerator ChaseCoroutine()
    {
        Vector3 playerSnapShot = playerTest.transform.position;
        navMeshAgent.SetDestination(playerSnapShot);

        if (!navMeshAgent.hasPath && !playerSeen)
        {
            enemyState = EnemyState.Patrol;
            yield return null;
        }
    }
}
