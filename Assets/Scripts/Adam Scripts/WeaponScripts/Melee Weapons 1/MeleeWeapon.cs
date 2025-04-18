using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MeleeBase
{
    // Constructor initializes meleeSetPoint, hitBox, and weaponRange based on params
    public MeleeWeapon(Transform meleeSetPoint, GameObject hitbox, float wepRange)
    {
        meleeBehavior = new MeleeBasicBehavior { meleeSetPoint = meleeSetPoint, hitBox = hitbox, weaponRange = wepRange};
    }
    public override void Use()
    {
        meleeBehavior.SwingWeapon();
    }
}
