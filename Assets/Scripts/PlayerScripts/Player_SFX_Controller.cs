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

    public AudioClip hitScanGunFire,
        projectileGunFire,
        continuousGunFire,//edit me, my clip is very long
        noAmmoFire,
        explosion,
        //smokeBombAudio,
        ammoPickUp,
        healthPickUp,
        shieldPickUp,
        weaponPickUp,
        confirmedHitAudio,
        walkAudio;

    public AudioClip[] playerHitSounds;
        //enemySpottedSounds,
        //gameMusic;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        WeaponController.instance.OnFireWeapon += OnFireGun;
        PlayerStatsScript.instance.OnTakeDamage += OnTakeDamage;
    }

    //player gun SFX for player to hear. expand into sound system the enemies hear based on dist?
    public void OnFireGun()//subbed to weaponcontroller onfireweapon
    {
        if (WeaponController.instance.myGun.ammoCount == 0)
        {
            audioSource.clip = noAmmoFire;
            Debug.Log("no ammo in waepon");//swap to have diff sounds for each weapon?
        }
        else if (WeaponController.instance.isHitScan)
        {
            audioSource.clip = hitScanGunFire;
            Debug.Log("hitscan shot");
        }
        else if (WeaponController.instance.isProjectile)
        {
            audioSource.clip = projectileGunFire;
            Debug.Log("proj shot");
        }
        else if (WeaponController.instance.isContinuous)
        {
            //audioSource.clip = continuousGunFire;
            Debug.Log("cont shot");
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
        audioSource.clip = healthPickUp;
        audioSource.Play();
    }
    public void OnWeaponPickup(WeaponSO weaponSO)
    {
        audioSource.clip = ammoPickUp;
        audioSource.Play();
    }
}
