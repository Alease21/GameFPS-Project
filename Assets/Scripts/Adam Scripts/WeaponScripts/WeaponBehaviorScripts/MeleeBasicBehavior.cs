using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeBasicBehavior : IMeleeBehavior
{
    public Animation swingAnimation;

    //add arc hitbox?
    //base on animation found?
    //play animation?
    //woudl need to send animator/animation to melee object thru method
    public void SwingWeapon()
    {
        Debug.Log("Weapon Swanged");
    }
}
