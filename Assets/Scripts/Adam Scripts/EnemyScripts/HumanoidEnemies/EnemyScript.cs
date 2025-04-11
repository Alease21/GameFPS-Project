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
    
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    //public float enemySpeed;
    public float enemyFOV;
    public float enemyViewDist;

    private void Start()
    {
        e_WepControl = GetComponent<EnemyWeaponController>();
        enemyName = enemySO.enemyName;
        enemyHealth = enemySO.enemyHealth;
        enemyDamage = enemySO.enemyDamage;
        enemyFOV = enemySO.enemyFOV;
        enemyViewDist = enemySO.enemyViewDistance;
        //enemySpeed = enemySO.enemySpeed; //adjust navmeshagent speed?

        switch (enemySO.enemyType)
        {
            case BaseEnemy.EnemyType.Range:
                e_WepControl.WeaponPrefabSpawn(weaponSO.weaponType, weaponSO.ammoMax, weaponSO.ammoCount, enemyDamage);
                break;
            case BaseEnemy.EnemyType.Melee:
                e_WepControl.WeaponPrefabSpawn(weaponSO.weaponType, weaponSO.range);
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
    public void OnEnemyDeath()
    {
        Destroy(gameObject);
    }
}