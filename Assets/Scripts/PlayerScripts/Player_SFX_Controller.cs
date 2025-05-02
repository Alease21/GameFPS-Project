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

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponController.instance.OnFireWeapon += OnFireGun;
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

        audioSource.Play();
    }

    public void OnTakeDamage()//subbed to playerstatsscript ontakedamage
    {
        //random choose from array of sounds? add cd?

        // add stuff for shield damage making diff sound?
    }

    public void OnPlayerMove()
    {
        //check bool or velocity of player?
    }

    //call from pickup script
    public void OnItemPickup(ItemPackSO itemPackSO)
    {
        audioSource.clip = SFX_Library.instance.healPickUp;
        audioSource.Play();
    }
    public void OnWeaponPickup(WeaponSO weaponSO)
    {
        //audioSource.clip = SFX_Library.instance;
        //audioSource.Play();
    }
}
