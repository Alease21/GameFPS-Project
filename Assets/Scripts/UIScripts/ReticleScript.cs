using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{
    public Image gunReticle;

    //fix me please, bad way to call. figure out resource loading
    public Sprite[] spriteArray;
    //

    //Change me to unity event on waepon swap please
    void Update()
    {
        if (WeaponController.instance.isProjectile)
        {
            gunReticle.sprite = spriteArray[1];
        }
        else if (WeaponController.instance.isContinuous)
        {
            gunReticle.sprite = spriteArray[2];
        }
        else 
        {
            gunReticle.sprite = spriteArray[0];
        }
    }
}
