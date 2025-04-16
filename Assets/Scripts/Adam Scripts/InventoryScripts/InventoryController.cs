using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    private PlayerStatsScript playerStatsScript;
    private WeaponController weaponController;

    //Singleton setup
    public static InventoryController instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject healthShieldCanvas;

    [SerializeField] private GameObject hitScanWeaponIcon,
                                        projWeapIcon,
                                        contWeapIcon,
                                        hitScanAmmoIcon,
                                        projAmmoIcon,
                                        contAmmoIcon;

    [SerializeField] private TextMeshProUGUI hitScanAmmoCount,
                                             projAmmoCount,
                                             contAmmoCount;

    [SerializeField] private TextMeshProUGUI healthTextInv,
                            shieldTextInv,
                            healthText,
                            shieldText;

    public UnityEvent UIUpdateEvent;

    private void Start()
    {
        playerStatsScript = PlayerStatsScript.instance;
        weaponController = WeaponController.instance;

        //Initial checks for what weapon is equipped. uncollected weapons' ammo
        //counts are disabled until weaopn collected
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

        StartCoroutine(InventoryKeyPress());
    }

    //Coroutine to avoid using update for inventory toggle
    public IEnumerator InventoryKeyPress()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.I));
        UIUpdateEvent?.Invoke();
        InventoryToggle();
        yield return new WaitUntil(() => Input.GetKeyUp(KeyCode.I));
        StartCoroutine(InventoryKeyPress());
    }

    //Activate inventory panel and deactivate other health/shield ui
    public void InventoryToggle()
    {
        inventoryPanel.SetActive(inventoryPanel.activeInHierarchy ? false : true);
        healthShieldCanvas.SetActive(healthShieldCanvas.activeInHierarchy ? false : true);
    }

    //Once weapon is collected change icons' color to white and set ammocount of
    //that weapon active
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

    //UI update of Health and Shields
    public void HealthAndShieldUpdate()
    {
        healthTextInv.text = $"{playerStatsScript.Health} / {playerStatsScript.MaxHealth}";
        shieldTextInv.text = $"{playerStatsScript.Shield} / {playerStatsScript.MaxShield}";
        healthText.text = $"{playerStatsScript.Health} / {playerStatsScript.MaxHealth}";
        shieldText.text = $"{playerStatsScript.Shield} / {playerStatsScript.MaxShield}";
    }

    //UI update of ammo counts
    public void AmmoTextUpdate()
    {
        hitScanAmmoCount.text = $"{playerStatsScript.hitScanWeaponAmmo} / {playerStatsScript.maxHitscanAmmo}";
        projAmmoCount.text = $"{playerStatsScript.projectileWeaponAmmo} / {playerStatsScript.maxProjectileAmmo}";
        contAmmoCount.text = $"{playerStatsScript.continuousWeaponAmmo} / {playerStatsScript.maxContinuousAmmo}";
    }
}
