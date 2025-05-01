using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(MyGUID))]
public class WeaponScript : MonoBehaviour
{
    public bool isPickedUp;

    private void OnEnable()
    {
        if (!GetComponent<MyGUID>())
        {
            gameObject.AddComponent<MyGUID>();
        }
    }

    public WeaponSO weaponSO;

    public void OnPickUp()
    {
        //setactive to false instead of destroy for save/loading purposes
        gameObject.SetActive(false);
        isPickedUp = true;
    }

    public void OnLoadGameData(bool _isPickedUp)
    {
        gameObject.SetActive(_isPickedUp ? false : true);
        isPickedUp = _isPickedUp;
    }
}