using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponBase myWeapon;
    public WeaponBase weapon1,
                      weapon2,
                      weapon3;

    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform gunSetPoint;

    private GameObject hitScanWeapon,
                       projectileWeapon,
                       continuousWeapon;
    [SerializeField] private GameObject[] weaponPrefabs;//change to grab from weaponSO with projectile/fire prefabs?
    [SerializeField] private GameObject currWeapon;
    [SerializeField] private GameObject projectilePreFab;
    [SerializeField] private GameObject fireVisualPrefab;

    [SerializeField] private float continuousTickRate;//possible to grab from weaponSO upon item pickup?
    public bool isHoldingFire = false;

    private bool isHitScan,
                 isProjectile,
                 isContinuous;

    //set up to never flip false after true, could change for dropping weapons in future.
    public bool hasHitScan = false,
                hasProjectile = false,
                hasContinuous = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
