using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructable
{
    public void OnTakeDamage(float damage, bool useDOTDamage = false);
    public void OnDestroyed();
}
