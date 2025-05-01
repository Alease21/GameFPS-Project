using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationManager : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Animator animator;
    private NavMeshAgent navMeshAgent;

    private void OnEnable()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyScript = GetComponent<EnemyScript>();
    }

    void Update()
    {
        float horizontal = navMeshAgent.velocity.normalized.x;
        float vertical = navMeshAgent.velocity.normalized.z;

        //figure this out, unsure why - on the horizontal works and not positive for correct animations
        Vector3 offset =  vertical * transform.forward + -horizontal * transform.right;

        animator.SetFloat("Horizontal", offset.x);
        animator.SetFloat("Vertical", offset.z);
    }

    public void OnAttackAnimation()
    {
        animator.SetBool("IsAttacking", true);
        StartCoroutine(AnimationCoro());

        switch (enemyScript.enemySO.enemyType)
        {
            case EnemySO.EnemyType.Range:
                animator.Play("AttackRanged");
                break;
            case EnemySO.EnemyType.Melee:
                animator.Play("AttackMelee");
                break;
        }
    }
    public IEnumerator AnimationCoro()
    {
        yield return new WaitForSecondsRealtime(enemyScript.weaponSO.attackAnimation.length);
        animator.SetBool("IsAttacking", false);
    }
    public void OnLoadGameData()
    {
        StopAllCoroutines();
        animator.Play("DefaultMove");//stop current animation instantly
        animator.SetBool("IsAttacking", false);
    }
}
