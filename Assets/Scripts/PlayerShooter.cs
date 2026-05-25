using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.VirtualTexturing;

public class PlayerShooter : MonoBehaviour
{
    // Player視点のカメラ
    [SerializeField] private Camera mainCamera;

    [Header("弾の生成位置(銃口)")]
    [SerializeField] private Transform firePoint;

    [Header("射程距離")]
    [SerializeField] private float shootDistance = 100f;

    [Header("連射間隔")]
    [SerializeField] private float fireRate = 0.1f;

    [Header("最大装弾数")]
    [SerializeField] private int maxAmmo = 30;

    [Header("リロード時間")]
    [SerializeField] private float reloadTime = 2f;

    [Header("Enmeyロックオン")]
    [SerializeField] private EnemyLockOn lockOn;

    [Header("マズルフラッシュ")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("AudioSource")]
    [SerializeField] private AudioSource AudioSource;

    private int currentAmmo;
    private bool isReloading;

    private PlayerInputActions inputActions;

    private bool isShooting;
    private float nextFireTime;

    // 外部参照用
    public bool IsReloading => isReloading;
    public float ReloadTime => reloadTime;

    private float reloadTimer;
    public float ReloadProgress => reloadTimer / reloadTime;

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

        inputActions.Player.Reload.performed -= OnReload;

        inputActions.Disable();
    }

    private void Start()
    {
        currentAmmo = maxAmmo;

        muzzleFlash.Stop();
    }

    private void Update()
    {
        // 押しっぱなし中
        if(isShooting)
        {
            if(Time.time > nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    private void OnShootStarted(InputAction.CallbackContext context)
    {
        isShooting = true;

        Shoot();

        nextFireTime = Time.time + fireRate;
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
        if (isReloading)
        {
            return;
        }

        // 弾切れ
        if (currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        muzzleFlash.Play();

        StartCoroutine(StopShotCoroutine());

        AudioSource.Stop();
        AudioSource.time = 0f;
        AudioSource.Play();

        currentAmmo--;

        Debug.Log(currentAmmo + " / " + maxAmmo);

        Vector3 shootDirection;

        if (lockOn.CurrentTarget != null)
        {
            Vector3 targetDirection = (lockOn.CurrentTarget.transform.position - mainCamera.transform.position).normalized;

            shootDirection = Vector3.Lerp(mainCamera.transform.forward, targetDirection, 0.3f).normalized;
        }
        else
        {
            shootDirection = mainCamera.transform.forward;
        }

        // ==== 画面中央からRayを飛ばす ====
        Ray ray = new Ray(mainCamera.transform.position, shootDirection);
        Debug.DrawRay(ray.origin, ray.direction * shootDistance, Color.red, 2f);

        Vector3 targetPoint;

        // ==== RayCast判定 ====

        // Rayが何に当たったか
        if (Physics.Raycast(ray, out RaycastHit hit, shootDistance))
        {
            targetPoint = hit.point;
            Debug.Log("Hit : " + hit.collider.name);

            EnemyHitBox hitBox = hit.collider.GetComponent<EnemyHitBox>();

            if (hitBox != null)
            {
                hitBox.Hit();
            }
            // 何にも当たらなかった場合
            // 最大距離をターゲットにする
            else
            {
                targetPoint = ray.origin + ray.direction * shootDistance;
            }
        }
    }

        private void StartReload()
    {
        // 既にリロード中なら無視
        if (isReloading)
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
        isReloading = true;
        reloadTimer = 0f;

        Debug.Log("Reload Start");

        while (reloadTimer < reloadTime)
        {
            reloadTimer += Time.deltaTime;
            yield return null;
        }

        currentAmmo = maxAmmo;

        isReloading = false;

        Debug.Log("Reload Complete");
    }

    private IEnumerator StopShotCoroutine()
    {
        yield return new WaitForSeconds(0.08f);
        AudioSource.Stop();
    }
}
