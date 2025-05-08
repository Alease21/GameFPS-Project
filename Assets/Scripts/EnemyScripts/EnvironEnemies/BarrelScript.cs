using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

[RequireComponent(typeof(MyGUID))]
public class BarrelScript : MonoBehaviour, IDestructable, IAffectSurroundings, IDealDamageEnviron
{
    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }
        if (!GetComponent<AudioSource>())
        {
            gameObject.AddComponent<AudioSource>();
        }
    }

    [SerializeField] private EnvironmentalEnemySO environEnemySO;
    private BarrelInRangeScript inRangeScript;
    private SphereCollider explodeSphere;
    private AudioSource audioSource;

    [Range(1,10)] public float explodeRange;

    [SerializeField] private float health;
    [SerializeField] private float damage;
    public float Health { get { return health; } private set { health = value; } }
    public float Damage { get { return damage; } private set { damage = value; } }

    public List<GameObject> inRangeColliders;
    [SerializeField]private bool hasExploded = false;
    public bool HasExploded { get { return hasExploded; } private set { hasExploded = value; } }

    public GameObject explodeSphereVisual;
    public ParticleSystem explosionParticleSystem;

    public float explodeDur = .2f; // constant value for explode duration

    void Start()
    {
        explodeSphere = GetComponentInChildren<SphereCollider>();

        explosionParticleSystem = GetComponentInChildren<ParticleSystem>();
        explosionParticleSystem.gameObject.SetActive(false);
        
        inRangeScript = GetComponentInChildren<BarrelInRangeScript>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = SFX_Library.instance.explosion1;

        explodeSphere.radius = explodeRange / 2;
        health = environEnemySO.health;
        damage = environEnemySO.damage;
    }

    public void OnTakeDamage(float damage, bool useDOTDamage = false)
    {
        if (gameObject.activeInHierarchy && useDOTDamage)
        {
            StopCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));
            StartCoroutine(TakeDOTDamage(damage / 5, 5, 1.5f));//hard coded in ticks & tick time
        }
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
                OnDestroyed();
            }
        }
    }
    public IEnumerator TakeDOTDamage(float damage, int ticks, float tickTime)
    {
        while (ticks > 0)
        {
            yield return new WaitForSecondsRealtime(tickTime);
            OnTakeDamage(damage);
            ticks--;
        }
    }

    public void OnDestroyed()
    {
        // clean up gameobject list from trigger sphere before dealing dmg
        inRangeScript.InRangeCleanup();

        OnDealDamage();
        OnAffectSurrounding();

        StartCoroutine(DestroyCoro());
    }

    // Coroutine expands sphere gameobject to visualize explosion. (probably change me)
    public IEnumerator DestroyCoro()
    {
        audioSource.Play();
        explosionParticleSystem.gameObject.SetActive(true);
        explosionParticleSystem.Play();

        //setactive to false instead of destroy for save/loading
        StartCoroutine(PlayAudioAfterDestroy.SoundAfterDisable(gameObject, audioSource.clip.length));

        //wait for whichever duration is longer (no no animation or sound cutoff)
        yield return new WaitForSecondsRealtime(explosionParticleSystem.main.duration > audioSource.clip.length ? 
            explosionParticleSystem.main.duration : audioSource.clip.length);

        hasExploded = true;
    }
    public void OnDealDamage()
    {
        for (int i = 0; i < inRangeColliders.Count; i++)
        {
            if (inRangeColliders[i] != null)
            {
                if (inRangeColliders[i]?.GetComponent<PlayerStatsScript>())
                {
                    inRangeColliders[i].GetComponent<PlayerStatsScript>().TakeDamage(damage, true);
                }
                else if (inRangeColliders[i]?.GetComponent<EnemyScript>())
                {
                    inRangeColliders[i].GetComponent<EnemyScript>().TakeDamage(damage, true);
                }
            }
        }
        hasExploded = false;

        Debug.Log($"Barrel exploded!");
    }
    public void OnAffectSurrounding()
    {
        // add in kockback, directly away from gameobject (raycast from other.position using on triggerstay?)
        // maybe add fire on ground after explosion

        //Debug.Log("This is where I'd knock you back, if i had the code");
    }
    public void OnLoadGameData(float _health, bool _hasExploded)
    {
        StopAllCoroutines();

        health = _health;
        gameObject.SetActive(_hasExploded ? false : true);
        PlayAudioAfterDestroy.EnableVisualOnGameLoad(gameObject);

        explosionParticleSystem?.gameObject.SetActive(false);

        hasExploded = _hasExploded;
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
