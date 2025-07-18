using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 基于CharacterController 用力驱动的第一人称角色控制器
namespace Demo.第一人称角色控制器
{
    public class FirstPlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [Tooltip("基础移动速度")]
        [SerializeField] private float walkSpeed;
        [Tooltip("跑步运动速度系数")]
        [SerializeField] private float runMultiplier;
        [Tooltip("跳跃力")]
        [SerializeField] private float jumpForce;
        [Tooltip("重力")]
        [SerializeField] private float gravity = -9.81f;

        [Header("镜头旋转控制")]
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float mouseSensivity;
        [SerializeField] private float mouseVerticalClamp;

        [Header("按键绑定")]
        [SerializeField] private KeyCode jumpKey = KeyCode.Space;
        [SerializeField] private KeyCode runKey = KeyCode.LeftShift;


        // 角色移动相关变量
        private float _horizontalMovement;
        private float _verticalMovement;
        private float _currentSpeed;
        private Vector3 _moveDirection;
        public Vector3 _velocity;
        private CharacterController _characterController;
        private bool _isRunning;

        // 相机镜头相关变量
        private float _verticalRotation;
        private float _yAxis;
        private float _xAxis;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Movement();
            MouseLook();
        }

        #region 移动和旋转
        // 移动控制
        private void Movement()
        {
            // 地面维持向下的速度模拟重力
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }

            // 跳跃给个向上的力
            if (Input.GetKey(jumpKey) && _characterController.isGrounded)
            {
                // 负负得正
                _velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
            }

            _horizontalMovement = Input.GetAxis("Horizontal");
            _verticalMovement = Input.GetAxis("Vertical");

            _moveDirection = transform.forward * _verticalMovement + transform.right * _horizontalMovement;

            _isRunning = Input.GetKey(runKey);
            _currentSpeed = walkSpeed * (_isRunning ? runMultiplier : 1f);
            // 水平移动
            _characterController.Move(_moveDirection * _currentSpeed * Time.deltaTime);

            // 垂直方向每帧都收到重力影响
            _velocity.y += gravity * Time.deltaTime;
            // 垂直移动，只用到 y分量
            _characterController.Move(_velocity * Time.deltaTime);

        }

        // 转向控制
        private void MouseLook()
        {
            _xAxis = Input.GetAxis("Mouse X");
            _yAxis = Input.GetAxis("Mouse Y");

            // 垂直方向旋转限定
            _verticalRotation += -_yAxis * mouseSensivity;
            _verticalRotation = Mathf.Clamp(_verticalRotation, -mouseVerticalClamp, mouseVerticalClamp);

            // 垂直方向由相机控制，水平方向由相机父物体控制
            playerCamera.transform.localRotation = Quaternion.Euler(_verticalRotation, 0, 0);
            transform.rotation *= Quaternion.Euler(0, _xAxis * mouseSensivity, 0);
        }
        #endregion
    }
}


