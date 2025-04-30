using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleScript : MonoBehaviour
{
    //Singleton setup
    public static ReticleScript instance;
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
    //

    public Image gunReticle;
    public Vector2 firingRecticleSize = new Vector2(17.6287f, 17.6287f);
    public Vector2 initReticleSize = new Vector2(9.9423f, 9.9423f);

    //fix me please, bad way to call. figure out resource loading
    public Sprite[] spriteArray;
    //
    private void Start()
    {
        WeaponController.instance.OnSwapWeapon += ReticleSwap;
    }

    public void ReticleSwap()
    {
        Debug.Log("test ret swap");
        if (WeaponController.instance.IsProjectile)
        {
            if (gunReticle.sprite != spriteArray[1])
            {
                gunReticle.sprite = spriteArray[1];
            }
        }
        else if (WeaponController.instance.IsContinuous)
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

    //Change me to unity event on waepon swap please
    void Update()
    {
        gunReticle.rectTransform.sizeDelta = (WeaponController.instance.IsHoldingFire && WeaponController.instance.IsContinuous) ? 
            firingRecticleSize : initReticleSize;
    }
}
