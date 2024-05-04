using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CameraSpace
{
    public class CameraManager : MonoBehaviour
    {

        [SerializeField] Camera[] cameras;
        [SerializeField] int followingCamera = 0;
        [SerializeField] GameObject toFollow;
        [SerializeField] float sharpness = 1.0f;
        protected int currentCamera;
        // Start is called before the first frame update
        void Start()
        {
            currentCamera = 0;
            for (int x = 1; x < cameras.Length; x++)
            {
                cameras[x].enabled = false;
            }
        }
        private void LateUpdate()
        {
            if (currentCamera == followingCamera)
            {
                FollowObject(cameras[currentCamera]);
            }
        }

        protected void FollowObject(Camera followingCamera)
        {
            Vector3 toTarget = toFollow.transform.position - followingCamera.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(toTarget);
            followingCamera.transform.rotation = Quaternion.Lerp(followingCamera.transform.rotation,targetRotation, sharpness);
        }
        public void OnChangeCamera(InputAction.CallbackContext context)
        {
            int oldCamera = currentCamera;
            currentCamera++;
            if (currentCamera > cameras.Length - 1)
            {
                currentCamera = 0;
            }
            cameras[currentCamera].enabled = true;
            cameras[oldCamera].enabled = false;
        }

    }
}
