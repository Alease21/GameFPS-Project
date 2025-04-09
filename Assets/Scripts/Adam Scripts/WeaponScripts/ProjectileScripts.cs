using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScripts : MonoBehaviour
{
    public enum ProjectileType
    {
        Explosive,
        Fire
    }
    public ProjectileType projectileType;
    public int projectileDamage;

    private Vector3 initialPos;
    private float fireDistance = 2.255f;//make this adjustable in inspector? and show only on fire type?

    private void Start()
    {
        //grab from SO??
        switch (projectileType)
        {
            case ProjectileType.Explosive:
                projectileDamage = 20;
                break;
            case ProjectileType.Fire:
                projectileDamage = 1;
                initialPos = transform.position;
                break;
        }
    }
    private void Update()
    {
        // maybe move to fire behavior
        if (projectileType == ProjectileType.Fire)
        {
            Vector3 distanceTravelled = (initialPos - transform.position);

            if (distanceTravelled.magnitude >= fireDistance)
            {
                Destroy(gameObject);
            }
        }
    }

    //'Collision' with any collider destroys projectile
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Player":
                other.GetComponent<PlayerStatsScript>().TakeDamage(projectileDamage);
                break;
            case "Enemy":
                other.GetComponent<EnemyScript>().TakeDamage(projectileDamage);
                break;
            case "EnvironEnemy":
                //if(other.GetComponent<EnvironEnemy>() is IDestructable destructable)
                other.GetComponent<BarrelScript>().OnTakeDamage(projectileDamage); // fix me to be able to hit
                                                                                   // multiple environenemies
                                                                                   // (w/ different scripts)
                break;
        }

        Destroy(gameObject);
        //Debug.Log($"{projectileType} projectile collided with {other.gameObject.name}. {projectileDamage} damage done.)");
    }
}
