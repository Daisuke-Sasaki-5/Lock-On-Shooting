using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private PlayerShooter shooter;
    [SerializeField] private Image redloadBar;
    void Update()
    {
        redloadBar.fillAmount = shooter.ReloadProgress;
        redloadBar.gameObject.SetActive(shooter.IsReloading);
    }
}
