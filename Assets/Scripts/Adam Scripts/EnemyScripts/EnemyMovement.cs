using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Animator animator;
    //private Rigidbody rb;

    private NavMeshAgent navMeshAgent;
    public GameObject patrolPoint;

    void Start()
    {
        animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<EnemyScript>();

        navMeshAgent.SetDestination(patrolPoint.transform.position);
    }

    void Update()
    {
        float horizontal = navMeshAgent.velocity.normalized.x;
        float vertical = navMeshAgent.velocity.normalized.z;

        //figure this out, unsure why - on the horizontal works and not positive
        Vector3 offset =  vertical * transform.forward + -horizontal * transform.right;

        animator.SetFloat("Horizontal", offset.x);
        animator.SetFloat("Vertical", offset.z);
    }
}
