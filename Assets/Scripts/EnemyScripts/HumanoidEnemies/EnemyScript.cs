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
    [Space(20)]
    //

    public EnemySO enemySO;
    public WeaponSO weaponSO;
    private EnemyWeaponController e_WepControl;
    private Animator animator;
    private EnemyFSM enemyFSM;
    
    public string enemyName;
    public float enemyHealth;
    public float enemyDamage;
    public float enemyRotateSpeed;
    //public float enemySpeed;
    public float enemyFOV;
    public float enemyViewDist;

    public bool hasDied = false;//bool for save/load

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
                e_WepControl.WeaponPrefabSpawn(weaponSO);
                break;
            case EnemySO.EnemyType.Melee:
                e_WepControl.WeaponPrefabSpawn(weaponSO);
                break;
            default:
                Debug.Log("No enemy type on enemySO");
                break;
        }
    }

    public void TakeDamage(float damage, bool useDOTDamage = false)
    {
        if (useDOTDamage)
        {
            StartCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));//hard coded in ticks & tick time
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

    //Fix me, i don't continue running for some reason after triggers
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
        //add ondeath animation? setactive false after animation play

        //setactive to false instead of destroy for save/loading
        hasDied = true;
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