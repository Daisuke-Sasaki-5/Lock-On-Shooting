using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private PlayerShooter shooter;
    [SerializeField] private Image redloadBar;

    [Header("PlayerHP")]
    [SerializeField] private Image hpFillImage;

    [Header("Player")]
    [SerializeField] private PlayerHealth player;

    [Header("Ammo")]
    [SerializeField] private TMP_Text ammoText;

    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        UpdateReloadUI();
        UpdateAmmoUI();
    }

    private void UpdateAmmoUI()
    {
        ammoText.text = shooter.CurrentAmmo + " / " + shooter.MaxAmmo;
    }

    private void UpdateReloadUI()
    {
        redloadBar.fillAmount = shooter.ReloadProgress;
        redloadBar.gameObject.SetActive(shooter.IsReloading);
    }

    // PlayerHP UIçXêV
    public void UpdateHPUI()
    {
        if (hpFillImage != null && player != null)
        {
            hpFillImage.fillAmount = (float)player.CurrentHP / player.MaxHP;
        }
    }
}
