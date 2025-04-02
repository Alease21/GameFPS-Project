using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : BaseEnemy, IMeleeEnemy
{
    public MeleeEnemy(EnemySO enemySO)
    {
        enemyName = enemySO.enemyName;
        enemyHealth = enemySO.enemyHealth;
        enemySpeed = enemySO.enemySpeed;
        // theres more vars but not included for testing
    }

    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void DamageDeal()
    {
        throw new System.NotImplementedException();
    }

    public override void TakeDamage()
    {
        throw new System.NotImplementedException();
    }

    public void TestMethodMelee()
    {
        throw new System.NotImplementedException();
    }
}
