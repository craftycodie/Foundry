using UnityEngine;

namespace Raider.Game.Cameras
{
	/// <summary>
	/// This camera controller doesn't move at all.
	/// It was used in testing and may not still work.
	/// </summary>
    public class StaticCameraController : ThirdPersonCameraController
    {
        //Why are these here?
        //Vector3 cameraPosition;
        //Quaternion cameraRotation;

        StaticCameraController()
        {
            camStartingPos = new Vector3(0, 1.8f, 0);
            pointStartingPos = Vector3.zero;
            pointStartingRot = Vector3.zero;
            camStartingRot = Vector3.zero;
            overrideWalking = false;
            parent = null;
        }

        public override void Setup()
        {
            base.Setup();

            ////I shouldn't have to change the camera parent here, I should change the parentTransform variable.
            //CameraModeController.singleton.CameraParent = null;
        }

        void Update()
        {
            RotatePlayer();
        }
    }
}
