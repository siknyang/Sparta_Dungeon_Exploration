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
}
