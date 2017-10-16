using UnityEngine;

namespace Raider.Game.Cameras
{

    public class ThirdPersonCameraController : PlayerCameraController
    {
        //If override walking is enabled, 
        public bool overrideWalking = true;

        //If the player is walking and overrideWalking is set to true, this will be true.
        public bool walking = false;

        //This variable is used for cameras using the UpdateCameraDistance method.
        public float chosenCamDistance;

        //override position and rotation in construct.
        public ThirdPersonCameraController()
        {
            camStartingPos = new Vector3(0, 0, -5f);
            pointStartingPos = new Vector3(0, 2f, 0);
        }

        // Update is called once per frame
        void Update()
        {
            RotateCamera();
            LockCamPointZRotation();
            UpdateCameraDistance();
        }

        /*This method could be improved.
        Currently, it runs even if there's no output, because it is used to keep the camera inside walls.
        However, the KeepCameraInsideWalls method can be called directly anywhere, and calling that directly on update as well as here would probably be more suitable.
        That way, this method could continue only if there's input.

        For an example of the desired implementation, see commit 235a351 on alex_test*/

        public void RotateCamera()
        {
            float _yRot = Input.GetAxisRaw("Mouse X");
            float _xRot = Input.GetAxisRaw("Mouse Y");

			//Enabling this condition will cause the player to rotate with the camera only if the player is walking.
			
            //if (walking)
            //{
                RotatePlayer(_yRot);
                _yRot = 0;
            //}

            if (CameraModeController.singleton.thirdPersonCamSettings.inverted)
            {
                _xRot = -_xRot;
            }

            Vector3 _camPointRotate = new Vector3(_xRot, _yRot, 0) * CameraModeController.singleton.thirdPersonCamSettings.lookSensitivity;

            _camPointRotate = ApplyXBufferToRotation(camPoint.transform.eulerAngles, _camPointRotate);
            KeepCameraRotationWithinWalls(camPoint.transform.eulerAngles, _camPointRotate);

            //Apply rotation
            camPoint.transform.Rotate(_camPointRotate);
        }

        //Add chosenCamDistance assignment to the inherited start method.
        public override void Setup()
        {
            base.Setup();

            chosenCamDistance = cam.transform.localPosition.z;
        }

        public void RotatePlayer(float _yRot)
        {
            Vector3 _rotation = new Vector3(0f, _yRot, 0f) * CameraModeController.singleton.firstPersonCamSettings.lookSensitivity;

            //Apply rotation
            characterController.transform.Rotate(_rotation);
        }

        public virtual void KeepCameraInsideWalls(Vector3 _castToPos)
        {
            RaycastHit objectHitInfo = new RaycastHit();
            float castDistance = Vector3.Distance(transform.position, _castToPos);

            //This cast checks if anything is inbetween transform.position and _castToPos (the camPoint and the _desiredCam).
            bool hitWall = Physics.Raycast(transform.position, (_castToPos - transform.position).normalized, out objectHitInfo, castDistance, ~CameraModeController.singleton.thirdPersonCamSettings.transparent);
            float newCamDistance = (cam.transform.localPosition.z - (objectHitInfo.distance - castDistance)) * (1 - CameraModeController.singleton.thirdPersonCamSettings.cameraPaddingPercent);
            if (hitWall)
            {
                ChangeCameraDistance(newCamDistance);
            }
            //If no collision is found, cast with infinate distance, to figure out where the camera can go.
            else
            {
                hitWall = Physics.Raycast(transform.position, (_castToPos - transform.position).normalized, out objectHitInfo, ~CameraModeController.singleton.thirdPersonCamSettings.transparent);
                //Update the newCamDistance for the last raycast.
                newCamDistance = -objectHitInfo.distance * (1 - CameraModeController.singleton.thirdPersonCamSettings.cameraPaddingPercent);
                if (hitWall)
                {
                    //If there's more space than the camera needs, just use the chosen distance. (less than because camera distance is negative.)
                    if (newCamDistance <= chosenCamDistance)
                    {
                        ChangeCameraDistance(chosenCamDistance);
                    }
                    //Prevent the camera going ahead of the player.
                    else if (newCamDistance > 0)
                    {
                        ChangeCameraDistance(0);
                    }
                    //If there's still not enough space, use what is available.
                    else
                    {
                        ChangeCameraDistance(newCamDistance);
                    }
                }
                //If neither raycast hit anything, there's enough space to use the chosen cam distance.
                else
                {
                    ChangeCameraDistance(chosenCamDistance);
                }
            }
        }

        //Creates game objects to calculate where a rotation will move the camera and cam point, and uses these objects to call the KeepInWalls method before actually rotating.
        //This prevents the camera from jumping around as it calculates where both position and rotation needs to be.
        public void KeepCameraRotationWithinWalls(Vector3 _currentRotation, Vector3 _rotation)
        {
            //Create a couple of empty gameobjects for calculations.
            GameObject _desiredCamPoint = new GameObject("_desiredCamPoint");
            GameObject _desiredCam = new GameObject("_desiredCam");

            //Change the parents.
            //_desiredCamPoint.transform.parent = CameraModeController.singleton.localPlayerGameObject.transform;
            _desiredCam.transform.parent = _desiredCamPoint.transform;

            //Read the rotation and position from actual gameobjects.
            _desiredCamPoint.transform.position = camPoint.transform.position;
            _desiredCamPoint.transform.rotation = camPoint.transform.rotation;
            _desiredCam.transform.position = cam.transform.position;
            _desiredCam.transform.rotation = cam.transform.rotation;

            //Rotate the empty gameobject by the desired amount.
            _desiredCamPoint.transform.Rotate(_rotation);

            //Call the KeepCameraInsideWalls for the desired position after rotation.
            KeepCameraInsideWalls(_desiredCam.transform.position);

            //Destroy the temporary game objects.
            Destroy(_desiredCamPoint);
        }

        //Centers the campoint on Axis Y.
        public void CenterCamPointAxisY()
        {
            camPoint.transform.localEulerAngles = new Vector3(camPoint.transform.localEulerAngles.x, 0, 0);
        }

        public void ChangeCameraDistance(float newLocation)
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, newLocation);
        }

        //Moves the camera forwards or backwards, within the min and max boundries, when the player scrolls.
        public void UpdateCameraDistance()
        {
            if (Input.GetAxisRaw("Mouse ScrollWheel") != 0)
            {
                float _proposedNewLocation = cam.transform.localPosition.z + Input.GetAxisRaw("Mouse ScrollWheel") * CameraModeController.singleton.thirdPersonCamSettings.distanceMoveSpeed;

                //Camera distances are negative because the camera is behind the player dingus.
                //Check if the new distance is above or below the max/min.

                if (_proposedNewLocation < -CameraModeController.singleton.thirdPersonCamSettings.maxDistance)
                {
                    _proposedNewLocation = -CameraModeController.singleton.thirdPersonCamSettings.maxDistance;
                }
                else if (_proposedNewLocation > -CameraModeController.singleton.thirdPersonCamSettings.minDistance)
                {
                    _proposedNewLocation = -CameraModeController.singleton.thirdPersonCamSettings.minDistance;
                }

                //Now raycast to check if the distance is valid.

                RaycastHit objectHitInfo;
                bool _hitWall = Physics.Raycast(transform.position, (cam.transform.position - transform.position).normalized, out objectHitInfo/*, _proposedNewLocation*/, ~CameraModeController.singleton.thirdPersonCamSettings.transparent);
                //If there's not enough space for the desired camera distance, use what is available.
                if (_hitWall)
                {
                    ChangeCameraDistance(-objectHitInfo.distance * (1 - CameraModeController.singleton.thirdPersonCamSettings.cameraPaddingPercent));
                }
                //else, use the chosen position.
                else
                {
                    chosenCamDistance = _proposedNewLocation;
                    ChangeCameraDistance(chosenCamDistance);
                }
            }
        }
    }
}