using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    public void OnTakeDamage(int damage);
    public void OnDestroyed();
}
