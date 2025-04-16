using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxScript : MonoBehaviour
{
    public EnemyScript enemyScript;
    public Animator animator;

    private void Start()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        // Auto destroy this object once attack is completed
        if (!animator.GetBool("IsAttacking"))
        {
            Destroy(gameObject);
        }
    }

    // On collision with player, deal damage and update player stats
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerStatsScript>().TakeDamage(enemyScript.weaponSO.damage);
            //other.GetComponent<PlayerStatsScript>().UiStatUpdate?.Invoke();
        }
    }
}
