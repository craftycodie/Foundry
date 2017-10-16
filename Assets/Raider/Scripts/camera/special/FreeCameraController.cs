using UnityEngine;

namespace Raider.Game.Cameras
{
    /// <summary>
    /// Switch between follow and fly.
	/// Debug camera, not fully implemented.
    /// </summary>
    public class FreeCameraController : ThirdPersonCameraController
    {
        FreeCameraController()
        {
            camStartingPos = new Vector3(0, 0, 0);
            pointStartingPos = new Vector3(0, 0, 0);
        }

        public override void Setup()
        {
            base.Setup();
            //GUI.UserFeedback.LogError("FreeCam Enabled, press SLASH to switch between camera and player control.");
        }

        bool controllingPlayer = false;
        bool ControllingCamera { get { return !controllingPlayer; } set { controllingPlayer = !value; } }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                controllingPlayer = !controllingPlayer;
                //GUI.UserFeedback.LogError("FreeCam: Switched Controls.");
            }

            if (ControllingCamera)
            {
                MoveCamera();
                RotateCamera();
                preventMovement = true;
                overrideWalking = true;
            }

            if(controllingPlayer)
            {
                RotatePlayer();
                preventMovement = false;
                overrideWalking = false;
            }

            LockCamPointZRotation();
            LockCamZRotation();
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