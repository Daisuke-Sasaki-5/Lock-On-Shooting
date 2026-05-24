using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

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

    [Header("最大装弾数")]
    [SerializeField] private int maxAmmo = 30;

    [Header("リロード時間")]
    [SerializeField] private float reloadTime = 2f;

    private int currentAmmo;
    private bool isReloding;

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

        inputActions.Player.Reload.performed += OnReload;
    }

    /// <summary>
    /// 無効化
    /// </summary>
    private void OnDisable()
    {
        // イベント解除
        inputActions.Player.Shoot.performed -= OnShootStarted;
        inputActions.Player.Shoot.canceled -= OnShootCanceled;

        inputActions.Player.Reload.canceled -= OnReload;

        inputActions.Disable();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;
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

    private void OnReload(InputAction.CallbackContext context)
    {
        StartReload();
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
        // リロード中は撃てない
        if(isReloding)
        {
            return;
        }

        // 弾切れ
        if(currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        currentAmmo--;

        Debug.Log(currentAmmo + " / " + maxAmmo);

        // ==== 画面中央からRayを飛ばす ====
        Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 2f);

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

    private void StartReload()
    {
        // 既にリロード中なら無視
        if (isReloding)
        {
            return;
        }

        // 満タンなら不要
        if(currentAmmo >= maxAmmo)
        {
            return;
        }

        StartCoroutine(RealodCoroutine());
    }

    private System.Collections.IEnumerator RealodCoroutine()
    {
        isReloding = true;

        Debug.Log("Reload Start");

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;

        isReloding = false;

        Debug.Log("Reload Complete");
    }
}
