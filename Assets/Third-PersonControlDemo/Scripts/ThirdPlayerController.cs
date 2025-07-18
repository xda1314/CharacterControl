using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPlayerController : MonoBehaviour
{
    private float moveSpeed = 3.5f; // 人物移动速度
    public Transform cameraTransform; // 摄像机的 Transform
    private CharacterController characterController;
    private float gravity = -9.81f; // 重力加速度
    private float verticalVelocity = 0f; // 垂直速度
    private bool isGrounded; // 是否在地面上

    void Awake()
    {
        // 获取角色的 CharacterController 和 Animator 组件
        characterController = GetComponent<CharacterController>();
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 确保角色紧贴地面
        }
    }
    void FixedUpdate()
    {
        HandleMovement(); // 控制人物移动
    }

    void HandleMovement()
    {
        // 检测是否在地面上
        isGrounded = characterController.isGrounded;
        if (isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // 确保角色紧贴地面
        }
        // 获取输入
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // 计算移动方向（基于相机朝向）
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 moveDirection = vertical * cameraForward + horizontal * cameraTransform.right;
        // 应用重力
        if (!isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime; // 累加重力
        }
        else
        {
            verticalVelocity = 0f;
        }

        // 将垂直速度添加到移动方向
        moveDirection.y = verticalVelocity;
        if (moveDirection.magnitude >= 0.1f)
        {
            // 计算目标旋转方向
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            if (moveDirection.x != 0 && moveDirection.z != 0)
            {
                transform.rotation = Quaternion.Euler(0, targetAngle, 0);
            }
            // 移动角色
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}