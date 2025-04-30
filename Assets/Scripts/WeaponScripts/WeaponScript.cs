using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponSO weaponSO;

    public void OnPickUp()
    {
        //setactive to false instead of destroy for save/loading purposes
        gameObject.SetActive(false);
    }
}