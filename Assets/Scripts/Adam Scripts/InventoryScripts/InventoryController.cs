using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    public PlayerStatsScript playerStatsScript;
    public WeaponController weaponController;

    public GameObject hitScanWeaponIcon;
    public GameObject projWeapIcon;
    public GameObject contWeapIcon;
    public GameObject hitScanAmmoIcon;
    public GameObject projAmmoIcon;
    public GameObject contAmmoIcon;

    public TextMeshProUGUI hitScanAmmoCount;
    public TextMeshProUGUI projAmmoCount;
    public TextMeshProUGUI contAmmoCount;

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI shieldText;

    public KeyDownEvent InventoryOpen;

    private void Start()
    {
        if (!weaponController.hasHitScan)
        {
            hitScanAmmoCount.gameObject.SetActive(false);
        }
        if (!weaponController.hasProjectile)
        {
            projAmmoCount.gameObject.SetActive(false);
        }
        if (!weaponController.hasContinuous)
        {
            contAmmoCount.gameObject.SetActive(false);
        }

    }

    public void ItemGetColorChange()
    {
        if (weaponController.hasHitScan)
        {
            hitScanWeaponIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            hitScanAmmoIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            hitScanAmmoCount.gameObject.SetActive(true);
        }
        if (weaponController.hasProjectile)
        {
            projWeapIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            projAmmoIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            projAmmoCount.gameObject.SetActive(true);
        }
        if (weaponController.hasContinuous)
        {
            contWeapIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            contAmmoIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            contAmmoCount.gameObject.SetActive(true);
        }
    }

    public void HealthAndShieldUpdate()
    {
        healthText.text = $"{playerStatsScript.Health} / {playerStatsScript.MaxHealth}";
        shieldText.text = $"{playerStatsScript.Shield} / {playerStatsScript.MaxShield}";
    }

    public void AmmoTextUpdate()
    {
        hitScanAmmoCount.text = $"{playerStatsScript.hitScanWeaponAmmo} / {playerStatsScript.maxHitscanAmmo}";
        projAmmoCount.text = $"{playerStatsScript.projectileWeaponAmmo} / {playerStatsScript.maxProjectileAmmo}";
        contAmmoCount.text = $"{playerStatsScript.continuousWeaponAmmo} / {playerStatsScript.maxContinuousAmmo}";
    }
}
