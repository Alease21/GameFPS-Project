using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public WeaponSO weaponSO;
    public GameObject weaponPrefab;

    public void OnPickUp()
    {
        //setactive to false instead of destroy for save/loading
        gameObject.SetActive(false);

        /*
        gameObject.layer = 6;
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            GameObject child = gameObject.transform.GetChild(i).gameObject;
            child.layer = 6; //invis layer
        }
        */
    }
}