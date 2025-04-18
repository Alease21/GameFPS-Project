using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ThrowableBase
{
    public GameObject throwablePrefab;
    public Transform throwPoint;
    public int throwableCount;
    public int throwableMax;
    public float throwableSpeed = 20f;

    public abstract bool CountGet(int amount);

    public abstract void Use(Transform throwPoint);
}
