using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxScript : MonoBehaviour
{
    private EnemyScript enemyScript;
    private Animator animator;
    //private EnemyFSM enemyFSM;

    float timer = 0f;
    private void Start()
    {
        enemyScript = GetComponentInParent<EnemyScript>();
        animator = GetComponentInParent<Animator>();
    }
    private void Update()
    {
        timer += Time.deltaTime;

        // Auto destroy this object once attack is completed
        if (timer > 1.14f)//hard coded swing time in, make more dynamic?
        {
            Destroy(gameObject);
        }
    }

    // On collision with player, deal damage and update player stats
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatsScript>())
        {
            other.GetComponent<PlayerStatsScript>().TakeDamage(enemyScript.weaponSO.damage);
            enemyScript.OnMeleeHit?.Invoke();

            //destroy hitbox when it collides w/ player
            Destroy(gameObject);
        }
    }
}
