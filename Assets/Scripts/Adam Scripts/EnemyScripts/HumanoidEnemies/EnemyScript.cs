using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyWeaponController))]
public class EnemyScript : MonoBehaviour
{
    public EnemySO enemySO;
    public WeaponSO weaponSO;
    private EnemyWeaponController e_WepControl;
    private Animator animator;
    private EnemyFSM enemyFSM;
    
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    public float enemyRotateSpeed;
    //public float enemySpeed;
    public float enemyFOV;
    public float enemyViewDist;

    //Gizmo to visualize enemy sight range(FOV) in scene editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        UnityEditor.Handles.color = Color.red * new Color(1f, 1f, 1f, 0.3f);
        Vector3 rotatedForward = Quaternion.Euler(0, -enemySO.enemyFOV / 2, 0) * transform.forward;
        UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, rotatedForward, enemySO.enemyFOV, enemySO.enemyViewDistance);
    }
#endif

    private void Start()
    {
        e_WepControl = GetComponent<EnemyWeaponController>();
        animator = GetComponent<Animator>();
        enemyFSM = GetComponent<EnemyFSM>();

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
        enemyFSM.gotShot = true;
    }
    public void OnEnemyDeath()
    {
        Destroy(gameObject);
    }
}