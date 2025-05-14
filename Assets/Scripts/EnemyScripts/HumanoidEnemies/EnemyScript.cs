using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(EnemyWeaponController), typeof(MyGUID))]
public class EnemyScript : MonoBehaviour, ICanDie
{
    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }
    }

    //Gizmo bools to flip in inspector
    [Header("Gizmo Bools:")]
    [Space(5)]
    [SerializeField] private bool showFOV;
    [SerializeField] private bool showPatPath;
    [Space(10)]//not sure why this space stopped working
    //

    private EnemyWeaponController e_WepControl;
    private EnemyFSM enemyFSM;
    private EnemySFXController e_SFXController;
    private Animator animator;

    public EnemySO enemySO;
    public WeaponSO weaponSO;
    
    public string enemyName;
    public float enemyHealth;
    public float enemyDamage;
    public float enemyRotateSpeed;
    //public float enemySpeed; //adjust navmeshagent speed?
    public float enemyFOV;
    public float enemyViewDist;

    public bool hasDied = false;//bool for save/load

    public Action OnPlayerSpotted;
    public Action OnEnemyAttack;
    public Action OnMeleeHit;

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
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(GetComponent<EnemyFSM>().patrolPoints[i].transform.position, .1f);

                Gizmos.color = Color.red * new Color(1f, 1f, 1f, 0.3f);
                Gizmos.DrawLine(GetComponent<EnemyFSM>().patrolPoints[i].transform.position, GetComponent<EnemyFSM>().patrolPoints[next].transform.position);
            }
        }

    }
#endif

    private void Start()
    {
        e_WepControl = GetComponent<EnemyWeaponController>();
        enemyFSM = GetComponent<EnemyFSM>();
        e_SFXController = GetComponent<EnemySFXController>();
        animator = GetComponent<Animator>();

        enemyName = enemySO.enemyName;
        enemyHealth = enemySO.enemyHealth;
        enemyFOV = enemySO.enemyFOV;
        enemyViewDist = enemySO.enemyViewDistance;
        enemyRotateSpeed = enemySO.enemyRotateSpeed;
        enemyDamage = weaponSO.damage;

        if (enemySO)
        {
            e_WepControl.WeaponPrefabSpawn(weaponSO);
        }
        else
        {
            Debug.Log($"No enemySO attached to {gameObject.name}'s enemyScript");
        }
    }

    public void TakeDamage(float damage, bool useDOTDamage = false)
    {
        if (useDOTDamage)
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));//hard coded in values for ticks & tick time
            }
        }
        if (enemyHealth > 0 && enemyHealth > damage)
        {
            enemyHealth -= damage;
        }
        else
        {
            enemyHealth = 0;
            OnDeath();
        }
        enemyFSM.gotShot = true;
    }

    public IEnumerator TakeDOTDamage(float damage, int ticks, float tickTime)
    {
        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(tickTime);
            TakeDamage(damage);
            ticks--;
        }
    }

    public void OnDeath()
    {
        //add ondeath animation? (setactive false after animation play)

        hasDied = true;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    public void OnLoadGameData(float _enemyHealth, float _enemyDamage, bool _hasdied)
    {
        enemyHealth = _enemyHealth;
        enemyDamage = _enemyDamage;
        hasDied = _hasdied;
        gameObject.SetActive(hasDied ? false : true);
    }
}