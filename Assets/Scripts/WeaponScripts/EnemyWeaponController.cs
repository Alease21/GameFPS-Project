using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    private EnemyScript enemyScript;

    [SerializeField] private GunBase myGun;
    public GunBase weapon1,
                   weapon2,
                   weapon3;
    [SerializeField] private MeleeBase myMelee;
    public MeleeBase weapon4;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;
    [SerializeField] private Transform meleeSetPoint;
    [SerializeField] private Transform meleeSwingPoint;

    private GameObject hitScanWeapon,
                       //projectileWeapon,
                       //continuousWeapon,
                       meleeWeapon;
    //[SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private GameObject currWeapon;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject fireVisualPrefab;
    [SerializeField] private GameObject hitScanShotPrefab;
    private ParticleSystem e_HitScanParticleSystem;

    [SerializeField] private float continuousTickRate;
    //private bool isHoldingFire = false;

    private bool hasHitScan = false;
    //hasProjectile = false,
    //hasContinuous = false;

    private void Start()
    {
        enemyScript = GetComponent<EnemyScript>();
        e_HitScanParticleSystem = gunSetPoint.GetComponentInChildren<ParticleSystem>();
        enemyScript.OnEnemyAttack += EnemyAttack;
    }
    public void EnemyAttack()
    {
        myGun?.Use();
        e_HitScanParticleSystem?.Play();
        myMelee?.Use();
    }

    //Initial gun object and gameobject instantiation based on weaponType param. New gun instantiated with weaponSO stats (in constructor)
    public void WeaponPrefabSpawn(WeaponSO weaponSO)
    {
        if (currWeapon != null)
        {
            currWeapon.SetActive(false);
        }

        //Based on weaponType, check if player has that weapon (bool), if false then instantiate gameobject and gun object and flip appropriate bool true
        switch (weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                if (!hasHitScan)
                {
                    hitScanWeapon = GameObject.Instantiate(weaponSO.weaponPrefab, gunSetPoint.position, gunSetPoint.transform.rotation);
                    hitScanWeapon.transform.parent = gunSetPoint.transform;
                    hitScanWeapon.layer = 8; //Enemy layer to avoid seeing object through walls due to prefab being on FPS_Elements layer
                    for (int i = 0; i < hitScanWeapon.transform.childCount; i++)
                    {
                        Transform child1 = hitScanWeapon.transform.GetChild(i);
                        child1.gameObject.layer = 8;

                        if (hitScanWeapon.transform.GetChild(i).childCount >= 1)
                        {
                            for (int j = 0; j < child1.childCount; j++)
                            {
                                child1.GetChild(j).gameObject.layer = 8;
                            }
                        }
                    }
                    currWeapon = hitScanWeapon;

                    weapon1 = new HitScanGun(weaponSO, 9999) { shootPoint = shootPoint }; //float is "cheatAmmo" maybe find better solution?
                    myGun = weapon1;

                    hasHitScan = true;
                }
                break;
            case WeaponSO.WeaponType.Melee:
                meleeWeapon = GameObject.Instantiate(weaponSO.weaponPrefab, meleeSetPoint.position, meleeSetPoint.transform.rotation);
                meleeWeapon.transform.parent = meleeSetPoint.transform;
                meleeWeapon.layer = 8; //Enemy layer to avoid seeing object through walls due to prefab being on FPS_Elements layer
                for (int i = 0; i < meleeWeapon.transform.childCount; i++)
                {
                    meleeWeapon.transform.GetChild(i).gameObject.layer = 8;
                }
                currWeapon = meleeWeapon;

                weapon4 = new MeleeWeapon(meleeSetPoint, weaponSO.hitBoxPrefab);
                myMelee = weapon4;
                break;

                /* Commented out other enemy weapons as just hitscan is implemented fully
                 * 
                case WeaponSO.WeaponType.Projectile:
                    if (!hasProjectile)
                    {
                        projectileWeapon = GameObject.Instantiate(weaponPrefabs[1], gunSetPoint.position, gunSetPoint.transform.rotation);
                        projectileWeapon.transform.parent = transform;
                        projectileWeapon.layer = 8;
                        for (int i = 0; i < projectileWeapon.transform.childCount; i++)
                        {
                            projectileWeapon.transform.GetChild(i).gameObject.layer = 8;
                        }
                        //currWeapon = projectileWeapon;

                        weapon2 = new ProjectileGun(projectilePreFab, initialAmmoMax, initialAmmoCount, damage, range) { shootPoint = shootPoint };
                        myGun = weapon2;

                        hasProjectile = true;
                        //Debug.Log("Enemy projectile weapon instantiated");
                    }
                    break;
                case WeaponSO.WeaponType.Continuous:
                    if (!hasContinuous)
                    {
                        continuousWeapon = GameObject.Instantiate(weaponPrefabs[2], gunSetPoint.position, gunSetPoint.transform.rotation);
                        continuousWeapon.transform.parent = transform;
                        continuousWeapon.layer = 8;
                        for (int i = 0; i < continuousWeapon.transform.childCount; i++)
                        {
                            continuousWeapon.transform.GetChild(i).gameObject.layer = 8;
                        }
                        currWeapon = continuousWeapon;

                        //StartCoroutine(ContinuousWeaponFire());

                        weapon3 = new ContinuousGun(fireVisualPrefab, initialAmmoMax, initialAmmoCount, damage) { shootPoint = shootPoint };
                        myGun = weapon3;

                        hasContinuous = true;
                        //Debug.Log("Enemy continuous weapon instantiated");
                    }
                    break;
                */
        }
    }
}
