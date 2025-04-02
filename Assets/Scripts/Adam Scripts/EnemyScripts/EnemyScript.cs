using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public BaseEnemy enemySelf;
    public WeaponBase enemyWeapon;
    public EnemySO enemySO;
    public WeaponSO weaponSO;
    
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    public float enemySpeed;
    public float enemyFOV;
    public float enemyRotateSpeed;

    private void Start()
    {
        switch (enemySO.enemyType)
        {
            case BaseEnemy.EnemyType.Range:
                enemySelf = new RangeEnemy();
                enemyWeapon = new HitScanGun(weaponSO.ammoMax, weaponSO.ammoCount);
                break;
            case BaseEnemy.EnemyType.Melee:
                enemySelf = new MeleeEnemy();
                break;
            default:
                Debug.Log("No enemy type on enemySO (no enemy object made)");
                break;
        }
        enemyName = enemySO.enemyName;
        enemyHealth = enemySO.enemyHealth;
        enemyDamage = enemySO.enemyDamage;
        enemySpeed = enemySO.enemySpeed;
        enemyFOV = enemySO.enemyFOV;
        enemyRotateSpeed = enemySO.enemyRotateSpeed;
    }
}