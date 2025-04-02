using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScripts : MonoBehaviour
{
    //Collision with any collider destroys projectile and send message with what was hit
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
        Debug.Log($"Boom!(Projectile Exploded on {collision.gameObject.name})");
    }
}
