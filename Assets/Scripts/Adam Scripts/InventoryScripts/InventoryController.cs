using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryController : MonoBehaviour
{
    private PlayerStatsScript playerStatsScript;
    private WeaponController weaponController;
    private ThrowableController throwableController;
    private PlayerMovement playerMovement;

    //Singleton setup
    public static InventoryController instance;
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

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject healthShieldCanvas;
    
    //find better solution
    [SerializeField] private GameObject grenadeAlphaScreen,
        SmokeBombAlphaScreen;

    [SerializeField] private GameObject hitScanWeaponIcon,
                                        projWeapIcon,
                                        contWeapIcon,
                                        hitScanAmmoIcon,
                                        projAmmoIcon,
                                        contAmmoIcon,
                                        grenadeIcon,
                                        smokeBombIcon;

    [SerializeField] private UnityEngine.UI.Image weaponInGameIcon,
                                                  grenadeInGameIcon,
                                                  smokeBombInGameIcon,
                                                  dashIcon,
                                                  dashRedCircle;

    //learn how to propperly call without coupling 
    [SerializeField] private Sprite[] spriteArray;

    [SerializeField] private TextMeshProUGUI hitScanAmmoCount,
                                             projAmmoCount,
                                             contAmmoCount,
                                             grenadeCount,
                                             smokeBombCount,
                                             currGunAmmoCount,
                                             grenadeCountIG,
                                             smokeBombCountIG;

[SerializeField] private TextMeshProUGUI healthTextInv,
                                         shieldTextInv;

    [SerializeField] private UnityEngine.UI.Slider healthBar,
                                                   shieldBar,
                                                   dashBar;
    public UnityEvent UIUpdateEvent;

    private void Start()
    {
        playerStatsScript = PlayerStatsScript.instance;
        weaponController = WeaponController.instance;
        throwableController = ThrowableController.instance;

        playerMovement = playerStatsScript.gameObject.GetComponent<PlayerMovement>();

        dashBar.value = 1;

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
        if (!throwableController.hasGrenade)
        {
            grenadeCount.gameObject.SetActive(false);
            grenadeCountIG.gameObject.SetActive(false);
        }
        if (!throwableController.hasSmokeBomb)
        {
            smokeBombCount.gameObject.SetActive(false);
            smokeBombCountIG.gameObject.SetActive(false);
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
    public IEnumerator OnDash()
    {
        dashIcon.color = Color.red * new Color(1f, 1f, 1f, 0.5f);
        //dashRedCircle.gameObject.SetActive(true);
        dashBar.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.red;
        for (float timer = 0f; timer < playerMovement.DashCoolDown; timer += Time.deltaTime)
        {
            dashBar.value = (playerMovement.DashCoolDown - timer) / playerMovement.DashCoolDown;
            yield return null;
        }

        dashIcon.color = Color.white;
        //dashRedCircle.gameObject.SetActive(false);
        dashBar.GetComponentInChildren<UnityEngine.UI.Image>().color = Color.green;
        for (float timer = 0f; timer < 0.3f; timer += Time.deltaTime)
        {                           // hard coded in float value for green bar "recharge"
            dashBar.value = timer / 0.3f;
            yield return null;
        }
    }
    public void OnWeaponSwap()
    {
        InGameWeaponIconStuff();
        InGameThrowableStuff();

        AmmoTextUpdate();
    }
    public void InGameThrowableStuff()
    {
        if (throwableController.hasGrenade && !grenadeCountIG.gameObject.activeInHierarchy)
        {
            grenadeCountIG.gameObject.SetActive(true);
        }
        if (throwableController.hasSmokeBomb && !smokeBombCountIG.gameObject.activeInHierarchy)
        {
            smokeBombCountIG.gameObject.SetActive(true);
        }

        if (throwableController.isGrenade)
        {
            grenadeAlphaScreen.SetActive(false);
            SmokeBombAlphaScreen.SetActive(true);
        }
        else if (throwableController.isSmokeBomb)
        {
            grenadeAlphaScreen.SetActive(true);
            SmokeBombAlphaScreen.SetActive(false);
        }
    }
    public void InGameWeaponIconStuff()
    {
        if (!weaponInGameIcon.gameObject.activeInHierarchy && weaponController.myGun != null)
        {
            weaponInGameIcon.gameObject.SetActive(true);
        }
        if (!currGunAmmoCount.gameObject.activeInHierarchy && weaponController.myGun != null)
        {
            currGunAmmoCount.gameObject.SetActive(true);
        }

        if (weaponController.isHitScan)
        {
            weaponInGameIcon.sprite = spriteArray[0];
        }
        else if (weaponController.isProjectile)
        {
            weaponInGameIcon.sprite = spriteArray[1];
        }
        else if (weaponController.isContinuous)
        {
            weaponInGameIcon.sprite = spriteArray[2];
        }

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
        if (throwableController.hasGrenade)
        {
            grenadeIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            grenadeCount.gameObject.SetActive(true);
        }
        if (throwableController.hasSmokeBomb)
        {
            smokeBombIcon.transform.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            smokeBombCount.gameObject.SetActive(true);
        }
    }

    //UI update of Health and Shields
    public void HealthAndShieldUpdate()
    {
        healthBar.value = (float)playerStatsScript.Health / (float)playerStatsScript.MaxHealth;
        if (healthBar.value == 0)
        {
            healthBar.GetComponentInChildren<UnityEngine.UI.Image>().color *= new Color(1f,1f,1f,0f);
        }
        else
        {
            healthBar.GetComponentInChildren<UnityEngine.UI.Image>().color += new Color(0f, 0f, 0f, 1f);
        }

        shieldBar.value = (float)playerStatsScript.Shield / (float)playerStatsScript.MaxShield;
        if (shieldBar.value == 0)
        {
            shieldBar.GetComponentInChildren<UnityEngine.UI.Image>().color *= new Color(1f,1f,1f,0f);
        }
        else
        {
            shieldBar.GetComponentInChildren<UnityEngine.UI.Image>().color += new Color(0f, 0f, 0f, 1f);
        }

        //healthTextGame.text = $"{playerStatsScript.Health} / {playerStatsScript.MaxHealth}";
        //shieldTextGame.text = $"{playerStatsScript.Shield} / {playerStatsScript.MaxShield}";
        healthTextInv.text = $"{playerStatsScript.Health} / {playerStatsScript.MaxHealth}";
        shieldTextInv.text = $"{playerStatsScript.Shield} / {playerStatsScript.MaxShield}";
    }

    //UI update of ammo counts
    public void AmmoTextUpdate()
    {
        hitScanAmmoCount.text = $"{playerStatsScript.hitScanWeaponAmmo} / {playerStatsScript.maxHitscanAmmo}";
        projAmmoCount.text = $"{playerStatsScript.projectileWeaponAmmo} / {playerStatsScript.maxProjectileAmmo}";
        contAmmoCount.text = $"{playerStatsScript.continuousWeaponAmmo} / {playerStatsScript.maxContinuousAmmo}";

        if(weaponController.myGun != null)
        {
            currGunAmmoCount.text = $"{weaponController.myGun.ammoCount} / {weaponController.myGun.ammoMax}";
        }

        grenadeCount.text = $"{playerStatsScript.grenadeCount} / {playerStatsScript.maxGrenades}";
        smokeBombCount.text = $"{playerStatsScript.smokeBombCount} / {playerStatsScript.maxSmokeBombs}";

        grenadeCountIG.text = $"{playerStatsScript.grenadeCount}\n{playerStatsScript.maxGrenades}";
        smokeBombCountIG.text = $"{playerStatsScript.smokeBombCount}\n{playerStatsScript.maxSmokeBombs}";

        if (playerStatsScript.grenadeCount == 0)
        {
            grenadeCountIG.color = Color.red;
            grenadeCountIG.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            grenadeCountIG.color = Color.white;
            grenadeCountIG.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
        if (playerStatsScript.smokeBombCount == 0)
        {
            smokeBombCountIG.color = Color.red;
            smokeBombCountIG.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        }
        else
        {
            smokeBombCountIG.color = Color.white;
            smokeBombCountIG.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
        }
    }
}
