using UnityEngine;

namespace Raider.Game.Cameras
{

    public class FollowCameraController : ThirdPersonCameraController
    {
        bool allowYRotation = true;

        //override position and rotation in construct.
        FollowCameraController()
        {
            camStartingPos = new Vector3(0, 0, -5f);
            pointStartingPos = new Vector3(0, 2f, 0);
        }

        // Update is called once per frame
        void Update()
        {
            RotateCamera();
            LockCamPointZRotation();
            RotatePlayer();
            UpdateCameraDistance();
        }

        new public void RotateCamera()
        {
            //Get the X and Y Inputs.
            float yRotation = Input.GetAxisRaw("Mouse X");
            float xRotation = Input.GetAxisRaw("Mouse Y");

            if (xRotation != 0 || yRotation != 0 || walking)
            {
                //If the player is walking, or y rotation is disabled, ignore the y input.
                if (!allowYRotation || walking)
                    yRotation = 0;

                //If the camera is inverted, invert the x input.
                if (CameraModeController.singleton.thirdPersonCamSettings.inverted)
                    xRotation = -xRotation;

                //Multiply the inputs by look sensetivity.
                Vector3 camPointRotate = new Vector3(xRotation, yRotation, 0) * CameraModeController.singleton.thirdPersonCamSettings.lookSensitivity;

                //Apply the maximum and minimum rotation.
                camPointRotate = ApplyXBufferToRotation(camPoint.transform.eulerAngles, camPointRotate);
                //Make sure the new rotation doesn't collide with anything.
                KeepCameraRotationWithinWalls(camPoint.transform.eulerAngles, camPointRotate);

                //Apply rotation
                camPoint.transform.Rotate(camPointRotate);
            }
        }

        new void RotatePlayer()
        {
            float _yRot = Input.GetAxis("Horizontal");

            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * CameraModeController.singleton.firstPersonCamSettings.lookSensitivity;

            //Apply rotation
            characterController.transform.Rotate(_rotation);
        }
    }
}