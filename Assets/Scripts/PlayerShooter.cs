using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{
    // Player視点のカメラ
    [SerializeField] private Camera mainCamera;

    // 弾を管理するオブジェクトプール
    [SerializeField] private BulletPool bulletPool;

    [Header("弾の生成位置(銃口)")]
    [SerializeField] private Transform firePoint;

    [Header("射程距離")]
    [SerializeField] private float shootDistance = 100f;

    [Header("弾速")]
    [SerializeField] private float bulletSpeed = 50f;

    [Header("連射間隔")]
    [SerializeField] private float fireRate = 0.1f;

    private PlayerInputActions inputActions;

    private bool isShooting;
    private float nextFireTime;

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

        // Shoot入力が押されたときにOnShoot呼ぶ
        inputActions.Player.Shoot.performed += OnShootStarted;
        inputActions.Player.Shoot.canceled += OnShootCanceled;
    }

    /// <summary>
    /// 無効化
    /// </summary>
    private void OnDisable()
    {
        // イベント解除
        inputActions.Player.Shoot.performed -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;

        inputActions.Disable();
    }

    private void Update()
    {
        // 押しっぱなし中
        if(isShooting)
        {
            if(Time.time > nextFireTime)
            {
                nextFireTime = Time.time + fireRate;
                Shoot();
            }
        }
    }

    private void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;
    }

    private void OnShootCanceled(InputAction.CallbackContext context)
    {
        isShooting = false;
    }

    /// <summary>
    /// 発射処理
    /// </summary>
    private void Shoot()
    {
        // ==== 画面中央からRayを飛ばす ====
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));

        Vector3 targetPoint;

        // ==== RayCast判定 ====

        // Rayが何に当たったか
        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance))
        {
            targetPoint = hit.point;
            Debug.Log("Hit : " + hit.collider.name);

            EnemyHealth enemy = hit.collider.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                enemy.TakeHit();
            }
        }
        // 何にも当たらなかった場合
        // 最大距離をターゲットにする
        else
        {
            targetPoint = ray.origin + ray.direction * shootDistance;
        }

        // ==== 弾の進行方向計算 ====

        // 正規化
        Vector3 direction = (targetPoint - firePoint.position).normalized;

        BulletVisual bullet = bulletPool.GetBullet();

        if(bullet != null)
        {
            bullet.transform.position = firePoint.position;
            bullet.Init(direction, bulletSpeed, 2f);
        }
    }
}
