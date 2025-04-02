using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "NewEnemy/NewBaseEnemy")]
public class EnemySO : ScriptableObject
{
    public BaseEnemy.EnemyType enemyType;
    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;
    public int enemySpeed;
}
