using UnityEngine;

namespace Raider.Game.Cameras
{
    /// <summary>
    /// Constrained flycam for spectators.
    /// </summary>
    public class NoClipCameraController : ThirdPersonCameraController
    {
        NoClipCameraController()
        {
            camStartingPos = new Vector3(0, 0, 0);
            pointStartingPos = new Vector3(0, 0, 0);
        }

        public override void Setup()
        {
            base.Setup();

            preventMovement = true;
            overrideWalking = false;
        }

        // Update is called once per frame
        void Update()
        {
            RotateCamera();
            MoveCamera();
            LockCamPointZRotation();
            LockCamZRotation();
        }

        new void RotateCamera()
        {
            //Looking up and down, needs to be inverted for some reason...
            float _yRot = Input.GetAxisRaw("Mouse X");
            float _xRot = -Input.GetAxisRaw("Mouse Y");

            //If the camera is set to inverted mode, invert the rotation.
            if (CameraModeController.singleton.firstPersonCamSettings.inverted)
            {
                _xRot = -_xRot;
            }

            Vector3 _camPointRotation = new Vector3(_xRot, _yRot, 0f) * CameraModeController.singleton.firstPersonCamSettings.lookSensitivity;

            _camPointRotation = ApplyXBufferToRotation(cam.transform.eulerAngles, _camPointRotation);

            //Apply rotation
            camPoint.transform.Rotate(_camPointRotation);
        }

        void MoveCamera()
        {
            float _movX = Input.GetAxis("Horizontal");
            float _movZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(_movX, 0, _movZ);
            //Slow it down a little.
            movement *= 0.5f;

            camPoint.transform.Translate(movement);
        }
    }
}