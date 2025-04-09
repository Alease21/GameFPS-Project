using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyFSM : MonoBehaviour

{
    //filler until fov system set up to see player
    [Header("FOV Filler Bool")]
    public GameObject playerTest;
    ///********************

    public EnemyScript enemyScript;

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
    public bool isIdle;
    public bool isPatroling;
    public bool isChasing;
    public int patrolIndex = 0; //find better solution

    public GameObject playerTarget;
    public bool playerSeen = false;
    //public UnityEvent OnPlayerSpotted;
    //public UnityEvent OnPlayerGone;

    private void Start()
    {
        enemyState = EnemyState.Idle;
        enemyScript = GetComponent<EnemyScript>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnDrawGizmos()
    {
        EnemySO enemySO = enemyScript.enemySO;

        UnityEditor.Handles.color = Color.red * new Color(1f,1f,1f,0.3f);
        Vector3 rotatedForward = Quaternion.Euler(0,-enemySO.enemyFOV / 2,0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up,rotatedForward , enemySO.enemyFOV, enemySO.enemyViewDistance);
    }
    private void Update()
    {
        Vector3 playerTargetDir = playerTarget.transform.position - transform.position;
        float playerTargetDist = playerTargetDir.magnitude;

        if (playerTargetDist < enemyScript.enemyViewDist && 
            Vector3.Angle(transform.forward, playerTargetDir.normalized) < enemyScript.enemyFOV / 2)
        {
            Debug.Log("test");
            playerSeen = true;
            if (isIdle)
            {
                isIdle = false;
            }
            if (isPatroling)
            {
                isPatroling = false;
            }
            enemyState = EnemyState.Chase;
        }
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
        }
    }
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
            isChasing = true;
        }

        //set dest as snapshot player pos
        //constanly check for player in sight, if in sight, snapshot post and set dest again
        //if reach snapshot point and no player in sight, then start patrol at last patrol point
        if (playerSeen)
        {
            Vector3 playerSnapShot = playerTest.transform.position;
            navMeshAgent.SetDestination(playerSnapShot);
            playerSeen = false;
        }
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

    /* Unused coroutines *\
    public IEnumerator PatrolCoroutine()
    {
        yield return null;
    }
    public IEnumerator ChaseCoroutine()
    {
        yield return null;
    }
    */
}
