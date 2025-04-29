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
            if (gunReticle.sprite != spriteArray[1])
            {
                gunReticle.sprite = spriteArray[1];
                transform.localPosition = new Vector3(0, -13.572f, 0);
            }
        }
        else if (WeaponController.instance.isContinuous)
        {
            if (gunReticle.sprite != spriteArray[2])
            {
                gunReticle.sprite = spriteArray[2];
                transform.localPosition = Vector3.zero;
            }
        }
        else 
        {
            if (gunReticle.sprite != spriteArray[0])
            {
                gunReticle.sprite = spriteArray[0];
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
