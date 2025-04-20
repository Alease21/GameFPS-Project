using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BarrelScript : MonoBehaviour, IDestructable, IAffectSurroundings, IDealDamageEnviron
{
    public EnvironmentalEnemySO environEnemySO;
    private BarrelInRangeScript inRangeScript;
    private SphereCollider explodeSphere;

    [Range(1,10)] public float explodeRange;

    [SerializeField] private int health;
    [SerializeField] private int damage;

    public List<GameObject> inRangeColliders;
    private bool hasExploded = false;

    public GameObject explodeSphereVisual;
    public float explodeDur = .2f; // constant value for explode duration

    void Start()
    {
        explodeSphere = GetComponentInChildren<SphereCollider>();
        inRangeScript = GetComponentInChildren<BarrelInRangeScript>();

        explodeSphere.radius = explodeRange / 2;
        health = environEnemySO.health;
        damage = environEnemySO.damage;
    }

    public void OnTakeDamage(int damage)
    {
        // add CD for damage take? (continuous weapon is too quick?) 
        if (!hasExploded)
        {
            if (health > 0 && health > damage)
            {
                health -= damage;
            }
            else if (health != 0 && health <= damage)
            {
                health = 0;
                hasExploded = true;
                OnDestroyed();
            }
        }
    }
    public void OnDestroyed()
    {
        // clean up gameobject list from trigger sphere before dealing dmg
        inRangeScript.InRangeCleanup();

        OnDealDamage();
        OnAffectSurrounding();

        StartCoroutine(DestroyCoro());

        if (inRangeColliders.Count == 0)
        {
            hasExploded = false;
        }
    }

    // Coroutine expands sphere gameobject to visualize explosion. (probably change me)
    public IEnumerator DestroyCoro()
    {
        yield return new WaitUntil(() => !hasExploded);

        Vector3 initSphereScale = explodeSphereVisual.transform.localScale;

        for (float timer = 0f; timer < explodeDur; timer += Time.deltaTime)
        {
            float explodeLerpRatio = timer / explodeDur;

            explodeSphereVisual.transform.localScale = Vector3.Lerp(initSphereScale, new Vector3(explodeRange, explodeRange, explodeRange), explodeLerpRatio);
            yield return null;
        }

        Destroy(gameObject);
    }
    public void OnDealDamage()
    {
        for (int i = 0; i < inRangeColliders.Count; i++)
        {
            switch (inRangeColliders[i].tag)
            {
                case "Player":
                    inRangeColliders[i].GetComponent<PlayerStatsScript>().TakeDamage(damage);
                    break;
                case "Enemy":
                    inRangeColliders[i].GetComponent<EnemyScript>().TakeDamage(damage);
                    break;
            }
        }
        hasExploded = false;

        Debug.Log($"Barrel exploded!");
    }
    public void OnAffectSurrounding()
    {
        // add in kockback, directly away from gameobject (raycast from other.position using on triggerstay?)
        // maybe add fire on ground after explosion
        Debug.Log("This is where I'd knock you back, if i had the code");
    }

#if UNITY_EDITOR
    //Gizmo for explosion radius visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red * new Color(1f, 1f, 1f, 0.5f);
        Gizmos.DrawWireSphere(transform.position, explodeRange / 2);
    }
#endif
}
