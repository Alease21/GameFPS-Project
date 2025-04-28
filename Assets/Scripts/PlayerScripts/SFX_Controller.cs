using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Controller : MonoBehaviour
{
    public AudioClip hitScanGunFire,
        projectileGunFire,
        continuousGunFire,
        noAmmoFire,
        explosion,
        smokeBombAudio,
        ammoPickUp,
        healthPickUp,
        shieldPickUp,
        weaponPickUp;

    public AudioClip confirmedHitAudio,
        walkAudio;

    public AudioClip[] playerHitSounds,
        enemySpottedSounds,
        gameMusic;
}
