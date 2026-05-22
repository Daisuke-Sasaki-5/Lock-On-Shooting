using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private BulletPool bulletPool;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float shootDistance = 100f;
    [SerializeField] private float bulletSpeed = 50f;

    private PlayerInputActions inputActions;

    /// <summary>
    /// 初期化
    /// </summary>
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    /// <summary>
    /// 有効化
    /// </summary>
    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Shoot.performed += OnShoot;
    }

    /// <summary>
    /// 無効化
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Shoot.performed -= OnShoot;

        inputActions.Disable();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    /// <param name="context"></param>
    private void OnShoot(InputAction.CallbackContext context)
    {
        Shoot();
    }

    private void Shoot()
    {
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Vector3 targetPoint;

        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance))
        {
            targetPoint = hit.point;
            Debug.Log("Hit : " + hit.collider.name);
        }
        else
        {
            targetPoint = ray.origin + ray.direction * shootDistance;
        }

        // 正規化(ターゲットから銃口の間）
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        BulletVisual bullet = bulletPool.GetBullet();

        if(bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.Init(direction, bulletSpeed, 2f);
        }
    }
}
