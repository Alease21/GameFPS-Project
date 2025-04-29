using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeBasicBehavior : IMeleeBehavior
{
    public Transform meleeSetPoint;
    public GameObject hitBox;
    //public float weaponRange;

    public MeleeBasicBehavior(Transform meleeSetPoint, GameObject hitBoxPrefab)
    {
        this.meleeSetPoint = meleeSetPoint;
        hitBox = hitBoxPrefab;
        //weaponRange = range;
    }
    // Instantiate new object to serve as weapon hitbox
    public void SwingWeapon()
    {
        GameObject MeleeHitBox = GameObject.Instantiate(hitBox, meleeSetPoint.position, meleeSetPoint.rotation);
        MeleeHitBox.transform.parent = meleeSetPoint;
    }
}
