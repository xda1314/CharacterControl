using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPlayerController : MonoBehaviour
{
    private float moveSpeed = 3.5f; // �����ƶ��ٶ�
    public Transform cameraTransform; // ������� Transform
    private CharacterController characterController;
    private float gravity = -9.81f; // �������ٶ�
    private float verticalVelocity = 0f; // ��ֱ�ٶ�
    private bool isGrounded; // �Ƿ��ڵ�����

    void Awake()
    {
        // ��ȡ��ɫ�� CharacterController �� Animator ���
        characterController = GetComponent<CharacterController>();
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // ȷ����ɫ��������
        }
    }
    void FixedUpdate()
    {
        HandleMovement(); // ���������ƶ�
    }

    void HandleMovement()
    {
        // ����Ƿ��ڵ�����
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // ȷ����ɫ��������
        }
        // ��ȡ����
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // �����ƶ����򣨻����������
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = vertical * cameraForward + horizontal * cameraTransform.right;
        // Ӧ������
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // �ۼ�����
        }
        else
        {
            verticalVelocity = 0f;
        }

        // ����ֱ�ٶ���ӵ��ƶ�����
        moveDirection.y = verticalVelocity;
        if (moveDirection.magnitude >= 0.1f)
        {
            // ����Ŀ����ת����
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            if (moveDirection.x != 0 && moveDirection.z != 0)
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
            // �ƶ���ɫ
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}