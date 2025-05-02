using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFX_Library : MonoBehaviour
{
    //Singleton setup
    public static SFX_Library instance;
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

    public AudioClip[] playerDmgTakeClips;

    public AudioClip footSteps1,
                     healPickUp,
                     hitScanWepFire,
                     projWepFire,
                     contWepFire,
                     meleeBarHit,
                     explosion1,
                     throwableThrow,
                     outAmmo,
                     wepSwap;
}