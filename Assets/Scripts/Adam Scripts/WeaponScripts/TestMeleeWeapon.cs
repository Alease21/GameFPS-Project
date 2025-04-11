using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMeleeWeapon : MeleeBase
{
    public TestMeleeWeapon(float range)
    {
        meleeBehavior = new MeleeBasicBehavior();
        meleeRange = range;
    }
    public override void Use()
    {
        meleeBehavior.SwingWeapon();
    }
}
