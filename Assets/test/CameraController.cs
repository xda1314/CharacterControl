using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Zas
{
    public class CameraController : MonoBehaviour
    {
        #region 变量
        [SerializeField] Transform followTarget;

        [SerializeField] float rotationSpeed = 2f;
        [SerializeField] float distance = 5f;
        [SerializeField] float minVerticalAngle = -45f;
        [SerializeField] float maxVerticalAngle = 45f;
        [SerializeField] Vector2 framingOffset;

        [SerializeField] bool invertX;
        [SerializeField] bool invertY;

        float invertXVal;
        float invertYVal;

        float rotationY;
        float rotationX;
        #endregion

     
        void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update()
        {
            invertXVal = invertX ? -1 : 1;
            invertYVal = invertY ? -1 : 1;
            rotationY += Input.GetAxis("Mouse X") * invertYVal * rotationSpeed;
            rotationX += Input.GetAxis("Mouse Y") * invertXVal * rotationSpeed;
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
            var targetRotation = Quaternion.Euler(rotationX, rotationY, 0);
            var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y); ;
            transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
            transform.rotation = targetRotation;
        }

        public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);
    }
}

