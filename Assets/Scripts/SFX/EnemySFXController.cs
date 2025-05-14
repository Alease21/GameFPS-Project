using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySFXController : MonoBehaviour
{
    private EnemyScript enemyScript;

    private AudioSource vocalAudioSource,
                       weaponAudioSource;

    private bool hasSpoken = false;
    private float vocalCD = 10f;

    private void Start()
    {
        enemyScript = GetComponentInParent<EnemyScript>();

        vocalAudioSource = GetComponents<AudioSource>()[0];
        weaponAudioSource = GetComponents<AudioSource>()[1];

        enemyScript.OnEnemyAttack += OnUseWeapon;
        enemyScript.OnPlayerSpotted += OnPlayerSpotted;
        enemyScript.OnMeleeHit += OnMeleeHit;

        vocalAudioSource.clip = SFX_Library.instance.playerSpotted;
        switch (enemyScript.weaponSO.weaponType)
        {
            case WeaponSO.WeaponType.HitScan:
                weaponAudioSource.clip = SFX_Library.instance.hitScanWepFire;
                break;
            case WeaponSO.WeaponType.Melee:
                weaponAudioSource.clip = SFX_Library.instance.meleeBarHit;
                break;
        }
    }
    public void OnUseWeapon()
    {
        //if range enemy, play sound instantly
        if (enemyScript.weaponSO.weaponType == WeaponSO.WeaponType.HitScan)
        {
            weaponAudioSource.Play();
        }
        else if (enemyScript.weaponSO.weaponType == WeaponSO.WeaponType.Melee)
        {
            //add in air swing sound?
            //**(melee hit sound played on hitbox collision)**
        }
    }
    public void OnMeleeHit()
    {
        if (enemyScript.weaponSO.weaponType == WeaponSO.WeaponType.Melee)
        {
            weaponAudioSource.Play();
        }
    }
    public void OnPlayerSpotted()
    {
        //add better cd or some kind of management
        if (!hasSpoken)
        {
            StartCoroutine(EnemyVocalCoro());
        }
    }
    public IEnumerator EnemyVocalCoro()
    {
        hasSpoken = true;
        vocalAudioSource.Play();
        yield return new WaitForSecondsRealtime(vocalCD);
        hasSpoken = false;
    }
}
