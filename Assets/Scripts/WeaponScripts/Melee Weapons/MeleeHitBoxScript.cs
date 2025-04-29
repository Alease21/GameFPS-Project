using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeHitBoxScript : MonoBehaviour
{
    public EnemyScript enemyScript;
    public Animator animator;
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
        if (timer > 1.14f)//hard coded time in, make more dynamic?
        {
            Destroy(gameObject);
        }
    }

    // On collision with player, deal damage and update player stats
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStatsScript>())
        {
            Debug.Log("test dmg: " + enemyScript.weaponSO.damage);
            other.GetComponent<PlayerStatsScript>().TakeDamage(enemyScript.weaponSO.damage);
            //other.GetComponent<PlayerStatsScript>().UiStatUpdate?.Invoke();
        }
    }
}
