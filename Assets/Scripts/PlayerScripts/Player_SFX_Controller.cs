using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_SFX_Controller : MonoBehaviour
{
    //Singleton setup
    public static Player_SFX_Controller instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioSource audioSource;

    private bool isDmgPlaying = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponController.instance.OnFireWeapon += OnFireGun;
        ThrowableController.instance.OnThrowableThrow += OnThrowThrowable;
        WeaponController.instance.OnSwapWeapon += OnWeaponSwap;
        PlayerStatsScript.instance.OnTakeDamage += OnTakeDamage;
    }

    //player gun SFX for player to hear. expand into sound system the enemies hear based on dist?
    public void OnFireGun()//subbed to weaponcontroller onfireweapon
    {
        if (WeaponController.instance.MyGun.ammoCount == 0)
        {
            audioSource.clip = SFX_Library.instance.outAmmo;//swap to have diff sounds for each weapon?
        }
        else if (WeaponController.instance.IsHitScan)
        {
            audioSource.clip = SFX_Library.instance.hitScanWepFire;
        }
        else if (WeaponController.instance.IsProjectile)
        {
            audioSource.clip = SFX_Library.instance.projWepFire;
        }
        else if (WeaponController.instance.IsContinuous)
        {
            audioSource.clip = SFX_Library.instance.contWepFire;
        }

        //check for audio clip playing on continuous to avoid overlapping
        if (WeaponController.instance.IsContinuous)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Play();
        }
    }
    public void OnThrowThrowable()
    {
        audioSource.clip = SFX_Library.instance.throwableThrow;
        audioSource.Play();
    }

    public void OnTakeDamage()//subbed to playerstatsscript ontakedamage
    {
        AudioClip[] playerSFXArray = SFX_Library.instance.playerDmgTakeClips;

        if (!isDmgPlaying)
        {
            int sfxWeightRand = UnityEngine.Random.Range(0, 100);
            if (sfxWeightRand < 20)
            {
                audioSource.clip = playerSFXArray[UnityEngine.Random.Range(0, 2)];//first 2 elements are "goofy" sounds
            }
            else
            {
                audioSource.clip = playerSFXArray[UnityEngine.Random.Range(3, 5)];
            }
            audioSource.Play();

            DamageSFXCD(audioSource.clip.length);
        }

        //random choose from array of sounds? add cd?

        // add stuff for shield damage making diff sound?
    }
    public IEnumerator DamageSFXCD(float clipLength)
    {
        isDmgPlaying = true;
        yield return new WaitForSecondsRealtime(clipLength);
        isDmgPlaying = false;
    }

    public void OnPlayerMove()
    {
        //check bool or velocity of player?
    }

    //call from pickup script
    public void OnItemPickup(ItemPackSO itemPackSO)
    {
        // add switch for diff pickup sounds
        switch (itemPackSO.itemPackType)
        {
            case ItemPackSO.ItemPackType.HealthPack:
            case ItemPackSO.ItemPackType.HOTPack:
            case ItemPackSO.ItemPackType.ShieldPack:
                audioSource.clip = SFX_Library.instance.healPickUp;
                break;
            case ItemPackSO.ItemPackType.AmmoPack:
                audioSource.clip = SFX_Library.instance.ammoPickup;
                break;
        }
        audioSource.Play();
    }

    //add specific SFX for each gun? otherwise combine some methods here?
    public void OnWeaponPickup(WeaponSO weaponSO)
    {
        switch (weaponSO.weaponType)
        {
            case (WeaponSO.WeaponType.HitScan):
            case (WeaponSO.WeaponType.Projectile):
            case (WeaponSO.WeaponType.Continuous):
                audioSource.clip = SFX_Library.instance.wepPickup;
                break;
            case (WeaponSO.WeaponType.Grenade):
            case (WeaponSO.WeaponType.SmokeBomb):
                audioSource.clip = SFX_Library.instance.ammoPickup;
                break;
        }
        audioSource.Play();
    }

    public void OnWeaponSwap()
    {
        audioSource.clip = SFX_Library.instance.wepSwap;
        audioSource.Play();
    }
}
