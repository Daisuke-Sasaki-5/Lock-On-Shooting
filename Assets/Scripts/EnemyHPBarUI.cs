using UnityEngine;
using UnityEngine.UI;

public class EnemyHPBarUI : MonoBehaviour
{
    [Header("EnemyHP")]
    [SerializeField] private EnemyHealth enemyHealth;

    [Header("前面HPバー")]
    [SerializeField] private Image frontUI;

    [Header("メインカメラ")]
    [SerializeField] private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // HP割合更新
        frontUI.fillAmount = (float)enemyHealth.CurrentHP / enemyHealth.MaxHP;

        // カメラ方向を向く
        transform.forward = mainCamera.transform.forward;
    }
}
