using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGunBehavior
{
    void FireGun(Transform shootPoint, int damage, float range);
}