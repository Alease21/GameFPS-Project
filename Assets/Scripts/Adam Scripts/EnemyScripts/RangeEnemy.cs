using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : BaseEnemy, IRangeEnemy
{
    public RangeEnemy(EnemySO enemySO)
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

    public void TestMethodRange()
    {
        throw new System.NotImplementedException();
    }
}
