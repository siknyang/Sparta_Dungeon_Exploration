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
    public bool canLook = true;    // ȭ�� ������

    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;    // ���콺 Ŀ�� �����
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
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;    // �� �� �� �� �̵�
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;    // �������� ���� ��ȭ�� �ֵ��� �⺻ ���� 0���� ����
        _rigidbody.velocity = dir;    // �Է� ������ �� ��ȭ
    }

    private void CameraLook()
    {
        // ���� ȸ�� ����
        camCurXRot += mouseDelta.y * lookSensitivity;   // x���� �������� ���� ȸ��
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);    // camCurXRot ���� �ּ� ���� �Ѿ�� �ּ� ���� ��ȯ�ϰ�, �ִ� ���� �Ѿ�� �ִ� ���� ��ȯ
        cameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);    // ���콺�� ���� �����̸� ���� �ٶ󺸰�, �Ʒ��� �����̸� �Ʒ��� �ٶ�

        // ���� ȸ�� ����
        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);    // y���� �������� �¿� ȸ��
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

    void ToggleCursor()    // Ŀ�� ǥ�� �¿���
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;    // Ŀ�� ǥ�� �������� = ��� ����: �κ��丮 â �����ְ�, ���콺 �����ӿ� ���� ȭ���� ������
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;    // ǥ�ð� �����ִٸ� ���ְ�, �����ִٸ� ����
        canLook = !toggle;    // ȭ���� ������ �� �ִ� �� ���콺 Ŀ�� ǥ�ð� ������ ��
                              // �κ��丮 â�� ������ ���콺 Ŀ�� ǥ�� ������, �κ��丮 â�� ������ ���콺 Ŀ�� ǥ�� ����
    }
}
