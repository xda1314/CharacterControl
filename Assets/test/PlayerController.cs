using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Zas
{
    public class PlayerController : MonoBehaviour
    {
        #region 变量
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float rotationoSpeed = 500f;

        [Header("地面检测设置")]
        [SerializeField] float groundCheckRadius = 0.2f;
        [SerializeField] Vector3 groundCheckOffset;
        [SerializeField] LayerMask groundLayer;

        bool isGrunded;
        float ySpeed;
        Quaternion targetRotation;

        CameraController camController;
        CharacterController characterController;
        #endregion

        void Awake()
        {
            camController = Camera.main.GetComponent<CameraController>();
            characterController = GetComponent<CharacterController>();
        }

      

        void Update()
        {
            Debug.Log("Is Grounded: " + characterController.velocity);
               

            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
            var moveInput = (new Vector3(h, 0, v)).normalized;
            var moveDir = camController.PlanarRotation * moveInput;
            GroundCheck();

            if (isGrunded)
            {
                ySpeed = -0.5f;
            }
            else
            {
                ySpeed += Physics.gravity.y * Time.deltaTime;
            }

            var velocity = moveDir * moveSpeed;
            velocity.y = ySpeed;
            characterController.Move(velocity * Time.deltaTime);
            if (moveAmount > 0f)
            {

                //transform.position += moveDir * moveSpeed * Time.deltaTime;
                targetRotation = Quaternion.LookRotation(moveDir);
            }
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                rotationoSpeed * Time.deltaTime);
        }

        void GroundCheck()
        {
            isGrunded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
            Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
        }
    }
}

