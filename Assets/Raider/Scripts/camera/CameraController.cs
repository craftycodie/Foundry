using UnityEngine;

namespace Raider.Game.Cameras
{
	/// <summary>
	/// A base camera controller class all cameras derive from. Contains general values such as the camera starting position and rotation, and virtual methods for setup.
	/// </summary>
    public abstract class CameraController : MonoBehaviour
    {
        //These values represent local positions and rotations!
        [Header("Camera Point")]
        protected Vector3 pointStartingPos = Vector3.zero;
        protected Vector3 pointStartingRot = Vector3.zero;
        [Header("Camera")]
        protected Vector3 camStartingPos = Vector3.zero;
        protected Vector3 camStartingRot = Vector3.zero;

        [SerializeField]
        public GameObject camPoint;
        public GameObject cam;

        //Not sure why I have this, going to assume it's for execution order problems.
        //I'll leave it for now.
        public Transform parent = null;
        public bool preventMovement = false;

        //Called by the CameraModeController.
        public virtual void Setup()
        {
            CameraModeController.singleton.CameraParent = parent;

            camPoint = CameraModeController.singleton.camPoint;

            //Assign the camera.
            cam = CameraModeController.singleton.cam;

            //I don't see why this is necessary.
            //Activate the camera.
            cam.SetActive(true);

            ResetCamAndCamPointPosAndRot();
        }

        //Sets the position and rotation of the camera and camPoint to the starting values.
        protected void ResetCamAndCamPointPosAndRot()
        {
            cam.transform.localPosition = camStartingPos;
            cam.transform.localEulerAngles = camStartingRot;
            camPoint.transform.localPosition = pointStartingPos;
            camPoint.transform.localEulerAngles = pointStartingRot;
        }

        //Sets the z rotation of camPoint to 0.
        protected void LockCamPointZRotation()
        {
            Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
            camPoint.transform.eulerAngles = new Vector3(_camPointCurrentRot.x, _camPointCurrentRot.y, 0);
        }

        //Sets the z value of cam to 0.
        protected void LockCamZRotation()
        {
            Vector3 _camCurrentRot = cam.transform.eulerAngles;
            cam.transform.eulerAngles = new Vector3(_camCurrentRot.x, _camCurrentRot.y, 0);
        }

        //Sets the y value of camPoint to 0.
        protected void LockCamPointYRotation()
        {
            Vector3 _camPointCurrentRot = camPoint.transform.eulerAngles;
            camPoint.transform.localEulerAngles = new Vector3(_camPointCurrentRot.x, 0, _camPointCurrentRot.z);
        }

        /// <summary>
        /// Prevents the a camera rotation from flipping to the back of the player.
        /// </summary>
        /// <param name="_Rotation">The current rotation of the camera or point.</param>
        /// <param name="_Rotate">The proposed rotate of the camera or point.</param>
        /// <returns>Returns the corrected rotation.</returns>
        protected Vector3 ApplyXBufferToRotation(Vector3 _currentRotation, Vector3 _rotate)
        {
            if (_currentRotation.x + _rotate.x > 90 - CameraModeController.singleton.xAxisBuffer && _currentRotation.x < 270)
                _rotate.x = (90 - CameraModeController.singleton.xAxisBuffer) - _currentRotation.x;
            else if (_currentRotation.x + _rotate.x < 270 + CameraModeController.singleton.xAxisBuffer && _currentRotation.x > 90)
                _rotate.x = (270 + CameraModeController.singleton.xAxisBuffer) - _currentRotation.x;
            return _rotate;
        }
    }
}