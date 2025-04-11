using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScripts : MonoBehaviour
{
    public enum ProjectileType
    {
        Projectile,
        Fire,
        HitScan
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
            case ProjectileType.Projectile:
                projectileDamage = 20;
                break;
            case ProjectileType.Fire:
                projectileDamage = 1;
                initialPos = transform.position;
                break;
            case ProjectileType.HitScan:
                StartCoroutine(hitScanShotDestroyer());
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
    public IEnumerator hitScanShotDestroyer()
    {
        float fadeDur = 0.5f;
        Color mat = gameObject.GetComponent<Renderer>().material.color;

        for (float timer = 0f; timer < fadeDur; timer += Time.deltaTime)
        {
            Debug.Log("test");
            float lerpRatio = timer / fadeDur;
            gameObject.GetComponent<Renderer>().material.color = Color.Lerp(mat,new Color(1f, 1f, 1f, 0f), lerpRatio);
            yield return null;
        }
        Destroy(gameObject);
    }
}
