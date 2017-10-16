using Foundry;
using UnityEngine;

namespace Raider.Game.Cameras
{
    /// <summary>
    /// Constrained flycam for spectators.
    /// </summary>
    public class FlyCameraController : ThirdPersonCameraController
    {
        FlyCameraController()
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
			if (Session.Paused)
				return;

            //Looking up and down, needs to be inverted for some reason...
            float _yRot = Input.GetAxis("Mouse X");
            float _xRot = -Input.GetAxis("Mouse Y");

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
			if (Session.Paused)
				return;

			float _movX = Input.GetAxis("Horizontal");
            float _movZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(_movX, 0, _movZ);
            //Slow it down a little.
            movement *= 0.5f;

            movement = KeepCameraInsideWalls(movement);
            camPoint.transform.Translate(movement);
        }

        new Vector3 KeepCameraInsideWalls(Vector3 _movement)
        {
            Vector3 desiredCamPointPos = camPoint.transform.position + _movement;

            RaycastHit objectHitInfo;
            bool hitwall = Physics.Linecast(camPoint.transform.position, desiredCamPointPos, out objectHitInfo, ~CameraModeController.singleton.thirdPersonCamSettings.transparent);
            if (hitwall)
            {
                _movement = Vector3.zero;
            }

            return _movement;
        }
    }
}