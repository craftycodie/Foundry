using UnityEngine;

namespace Raider.Game.Cameras
{

    abstract public class PlayerCameraController : CameraController
    {
        public CharacterController characterController;

        public override void Setup()
        {
			//Player camera controllers are fixed to the local player.
            //parent = CameraModeController.singleton.localPlayerGameObject.transform;
            //characterController = CameraModeController.singleton.localPlayerGameObject.GetComponent<CharacterController>();

            base.Setup();
        }

        public void RotatePlayer()
        {
            float _yRot = Input.GetAxisRaw("Mouse X");

            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * CameraModeController.singleton.firstPersonCamSettings.lookSensitivity;

            //Apply rotation
            characterController.transform.Rotate(_rotation);
        }
    }
}