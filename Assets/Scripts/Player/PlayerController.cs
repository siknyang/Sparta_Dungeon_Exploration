using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    public LayerMask groundLayerMask;
    private Vector2 curMovementInput;

    [Header("Look")]
    public Transform cameraContainer;
    public float minXLook;
    public float maxXLook;
    public float lookSensitivity;
    private float camCurXRot;
    private Vector2 mouseDelta;
    public bool canLook = true;    // 화면 움직임

    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    // 마우스 커서 숨기기
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        {
            CameraLook();
        }
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;    // 앞 뒤 왼 오 이동
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;    // 점프했을 때만 변화가 있도록 기본 값을 0으로 설정
        _rigidbody.velocity = dir;    // 입력 들어왔을 때 변화
    }

    private void CameraLook()
    {
        // 수직 회전 조절
        camCurXRot += mouseDelta.y * lookSensitivity;   // x축을 기준으로 상하 회전
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);    // camCurXRot 값이 최소 값을 넘어가면 최소 값을 반환하고, 최대 값을 넘어가면 최대 값을 반환
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);    // 마우스가 위로 움직이면 위를 바라보고, 아래로 움직이면 아래를 바라봄

        // 수평 회전 조절
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);    // y축을 기준으로 좌우 회전
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGround())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }

    private bool IsGround()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) +  (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) +  (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) +  (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) +  (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }
        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()    // 커서 표시 온오프
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    // 커서 표시 켜져있음 = 가운데 고정: 인벤토리 창 꺼져있고, 마우스 움직임에 따라 화면이 움직임
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;    // 표시가 켜져있다면 꺼주고, 꺼져있다면 켜줌
        canLook = !toggle;    // 화면이 움직일 수 있는 건 마우스 커서 표시가 꺼졌을 때
                              // 인벤토리 창이 켜지면 마우스 커서 표시 꺼지고, 인벤토리 창이 꺼지면 마우스 커서 표시 켜짐
    }
}
