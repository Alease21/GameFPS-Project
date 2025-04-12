using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyWeaponController))]
public class EnemyScript : MonoBehaviour
{
    public EnemySO enemySO;
    public WeaponSO weaponSO;
    public EnemyWeaponController e_WepControl;
    public Animator animator;
    
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    public float enemyRotateSpeed;
    //public float enemySpeed;
    public float enemyFOV;
    public float enemyViewDist;

    private void Start()
    {
        e_WepControl = GetComponent<EnemyWeaponController>();
        animator = GetComponent<Animator>();

        enemyName = enemySO.enemyName;
        enemyHealth = enemySO.enemyHealth;
        enemyFOV = enemySO.enemyFOV;
        enemyViewDist = enemySO.enemyViewDistance;
        enemyRotateSpeed = enemySO.enemyRotateSpeed;
        //enemySpeed = enemySO.enemySpeed; //adjust navmeshagent speed?

        enemyDamage = weaponSO.damage;

        switch (enemySO.enemyType)
        {
            case EnemySO.EnemyType.Range:
                e_WepControl.WeaponPrefabSpawn(weaponSO.weaponType, 9999, 9999, enemyDamage);
                break;
            case EnemySO.EnemyType.Melee:
                e_WepControl.WeaponPrefabSpawn(weaponSO.weaponType, weaponSO.hitBoxPrefab, weaponSO.range);
                break;
            default:
                Debug.Log("No enemy type on enemySO");
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (enemyHealth > 0 && enemyHealth > damage)
        {
            enemyHealth -= damage;
        }
        else
        {
            enemyHealth = 0;
            OnEnemyDeath();
        }
    }
    public void OnAttack()
    {
        switch (enemySO.enemyType)
        {
            case EnemySO.EnemyType.Range:
                animator.Play("AttackRanged");
                break;
            case EnemySO.EnemyType.Melee:
                animator.Play("AttackMelee");
                break;
        }
    }
    public void OnEnemyDeath()
    {
        Destroy(gameObject);
    }
}