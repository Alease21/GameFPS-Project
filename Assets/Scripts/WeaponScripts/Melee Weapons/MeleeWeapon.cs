using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MeleeBase
{
    // Constructor initializes meleeSetPoint, hitBox, and weaponRange based on params
    public MeleeWeapon(Transform meleeSetPoint, GameObject hitbox)
    {
        meleeBehavior = new MeleeBasicBehavior(meleeSetPoint, hitbox);
    }
    public override void Use()
    {
        meleeBehavior.SwingWeapon();
    }
}
