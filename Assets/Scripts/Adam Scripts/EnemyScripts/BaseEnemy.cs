using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy
{
    public enum EnemyType
    {
        Range,
        Melee
    }
    public EnemyType enemyType;

    public string enemyName;
    public int enemyHealth;
    public int enemyDamage;

    public float enemySpeed;
    public float enemyFOV;
    public float rotateSpeed;

    public abstract void DamageDeal();
    public abstract void TakeDamage();
    public abstract void Attack();
}
