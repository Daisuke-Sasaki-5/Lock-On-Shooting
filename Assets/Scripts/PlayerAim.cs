using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float sensitivity = 0.05f;

    private PlayerInputActions inputActions;
    private Vector2 lookInput;
    private float xRote;

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
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
    }

    /// <summary>
    /// 無効化
    /// </summary>
    private void OnDisable()
    {
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;

        inputActions.Disable();
    }

    private void Start()
    {
        // カーソル固定
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        RotateCamera();
    }

    /// <summary>
    /// 入力処理
    /// </summary>
    /// <param name="context"></param>
    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void RotateCamera()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        // 縦方向
        xRote -= mouseY;
        xRote = Mathf.Clamp(xRote, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRote, 0f, 0f);
        
        // 横方向
        transform.Rotate(Vector3.up * mouseX);
    }
}
