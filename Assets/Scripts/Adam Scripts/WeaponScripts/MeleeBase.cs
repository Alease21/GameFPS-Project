using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MeleeBase
{
    public float meleeRange;
    public IMeleeBehavior meleeBehavior;
    public abstract void Use();
}
