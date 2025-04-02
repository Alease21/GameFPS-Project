using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    //Keep as objects or have seperate monobehavior scripts for rang/melee?
    public BaseEnemy enemySelf;
    public EnemySO enemySO;

    private void Start()
    {
        switch (enemySO.enemyType)
        {
            case BaseEnemy.EnemyType.Range:
                enemySelf = new RangeEnemy(enemySO);
                break;
            case BaseEnemy.EnemyType.Melee:
                enemySelf = new MeleeEnemy(enemySO);
                break;
            default:
                Debug.Log("No enemy type on enemySO (no enemy object made)");
                break;
        }
    }
}