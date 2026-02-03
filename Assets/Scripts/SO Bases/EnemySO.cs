using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "NewEnemy/NewBaseEnemy")]
public class EnemySO : ScriptableObject
{
    public enum EnemyType
    {
        Range,
        Melee
    }
    public EnemyType enemyType;
    public string enemyName;
    public int enemyHealth;
    public float enemyRotateSpeed;
    public int enemySpeed;
    public float enemyFOV;
    public float enemyViewDistance;
}
