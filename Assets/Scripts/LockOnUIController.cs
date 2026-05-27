using System;
using UnityEngine;

public class LockOnUIController : MonoBehaviour
{
    [Header("ロックオン管理")]
    [SerializeField] private EnemyLockOn lockOn;

    [Header("ロックオンUI")]
    [SerializeField] private RectTransform lockOnUI;

    [Header("メインカメラ")]
    [SerializeField] private Camera mainCamera;

    private void LateUpdate()
    {
        UpdateUI();
    }

    /// <summary>
    /// ロックオンUI更新
    /// </summary>
    /// <exception cref="NotImplementedException"></exception>
    private void UpdateUI()
    {
        // ターゲット無し
        if(lockOn.CurrentTarget == null)
        {
            lockOnUI.gameObject.SetActive(false);
            return;
        }

        lockOnUI.gameObject.SetActive(true);

        // ワールド座標→スクリーン座標
        Vector3 screenPos = mainCamera.WorldToScreenPoint(lockOn.CurrentTarget.transform.position);

        // UI位置更新
        lockOnUI.position = screenPos;
    }
}
