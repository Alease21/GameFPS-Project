using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScripts : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log($"Boom!(Projectile Exploded) on {collision.gameObject.name}");
    }
}
