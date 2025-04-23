using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyWeaponController))]
public class EnemyScript : MonoBehaviour
{
    [Header("Gizmo Bools:")]
    [Space(5)]
    //GizmoBools to flip in inspector
    public bool showFOV;
    public bool showPatPath;
    //
    [Space(20)]

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

    //public bool isTakeDOT;

    //Gizmo to visualize enemy sight range(FOV) in scene editor
#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (showFOV)
        {
            UnityEditor.Handles.color = Color.red * new Color(1f, 1f, 1f, 0.3f);
            Vector3 rotatedForward = Quaternion.Euler(0, -enemySO.enemyFOV / 2, 0) * transform.forward;
            UnityEditor.Handles.DrawSolidArc(transform.position, transform.up, rotatedForward, enemySO.enemyFOV, enemySO.enemyViewDistance);
        }
        if (showPatPath)
        {
            for (int i = 0; i < GetComponent<EnemyFSM>().patrolPoints.Length; i++)
            {
                int next = i + 1;
                if (next == GetComponent<EnemyFSM>().patrolPoints.Length)
                {
                    next = 0;
                }
                Gizmos.color = Color.red * new Color(1f, 1f, 1f, 0.3f);

                Gizmos.DrawLine(GetComponent<EnemyFSM>().patrolPoints[i].transform.position, GetComponent<EnemyFSM>().patrolPoints[next].transform.position);
            }
        }

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

    //Fix me, i don't continue running for some reason after triggers
    public IEnumerator TakeDOTDamage(int damage, int ticks, float tickTime)
    {
        Debug.Log("Enemy DOT coro started");

        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(tickTime);
            TakeDamage(damage);
            ticks--;
            Debug.Log("Enemy Damage Tick. ticks: " + ticks);
        }
    }
    public void OnEnemyDeath()
    {
        Destroy(gameObject);
    }
}