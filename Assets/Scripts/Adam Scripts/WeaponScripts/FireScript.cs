using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireScript : MonoBehaviour
{
    public Vector3 initialPos;
    public float fireDistance = 2.255f;//make this adjustable in inspector?w

    private void Start()
    {
        initialPos = transform.position;
    }

    //Collision with object will destroy fire projectile
    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        Debug.Log($"Fire hitting {other.gameObject.name}");
    }

    //Destroy fire projectile after fireDistance has been travelled
    private void Update()
    {
        Vector3 distanceTravelled = (initialPos - transform.position);

        if (distanceTravelled.magnitude >= fireDistance)
        {
            Destroy(gameObject);
        }
    }
}
