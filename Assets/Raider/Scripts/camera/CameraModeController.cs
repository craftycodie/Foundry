using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Raider.Game.Cameras
{

    public class CameraModeController : MonoBehaviour
    {
		//Since there's only one camera, this class can use a singleton.
        #region Singleton Setup

        public static CameraModeController singleton;
        public static CameraController ControllerInstance
        {
            get { return singleton.GetComponent<CameraController>(); }
        }

        public void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (singleton != null)
                Debug.LogAssertion("It seems that multiple Camera Mode Controllers are active, breaking the singleton instance.");
            singleton = this;
        }

        public void OnDestroy()
        {
            singleton = null;
        }

        #endregion

        public GameObject camPoint;
        public GameObject cam;

        //how close the camera can be to directly overhead or underfoot.
        public float xAxisBuffer = 35f;

        public FirstPersonCameraSettings firstPersonCamSettings;
        public ThirdPersonCameraSettings thirdPersonCamSettings;

        [Serializable]
        public class FirstPersonCameraSettings
        {
            public float lookSensitivity = 3f;
            public bool inverted = false;
            public bool moveWithBody = true;
        }

        [Serializable]
        public class ThirdPersonCameraSettings
        {
            public LayerMask transparent;
            public float lookSensitivity = 3f;
            public float minDistance = 5f;
            public float maxDistance = 15f;
            public float distanceMoveSpeed = 3f;
            public float cameraPaddingPercent = 0.3f;
            public bool inverted = true;
        }

		//This enum is used to simply switch perspective.
        public enum CameraModes
        {
            Unknown = -1,
            None = 0,
            FirstPerson = 1,
            FlyCam = 4,
            Static = 5,
            FreeCam = 8,
        }

		/// <summary>
		/// This property allows the Camera Controller to be set based on a CameraMode enum value.
		/// Or, it can be used to return a CameraMode value based on the active CameraController.
		/// </summary>
        public CameraModes CameraMode
        {
            get
            {
                CameraController attachedController = GetComponent<CameraController>();

				if (attachedController == null)
					return CameraModes.None;
				else if (attachedController is FirstPersonCameraController)
					return CameraModes.FirstPerson;
				else if (attachedController is FlyCameraController)
					return CameraModes.FlyCam;
				else if (attachedController is StaticCameraController)
					return CameraModes.Static;
				else if (attachedController is FreeCameraController)
					return CameraModes.FreeCam;

                return CameraModes.Unknown;
            }

            //This can only be set once per frame, so a custom Change Camera method will ensure that happens.
            private set
            {
                RemoveCameraController();

                //Grab it now, or it'll only be retrievable next frame.
                CameraController newController = null;

                if (value == CameraModes.None)
                {
                    transform.Find("Camera").GetComponent<Camera>().enabled = false;
                }
                else
                {
                    transform.Find("Camera").GetComponent<Camera>().enabled = true;

                    switch (value)
                    {
                        case CameraModes.FirstPerson:
                            newController = gameObject.AddComponent<FirstPersonCameraController>();
                            break;
                        case CameraModes.FreeCam:
                            newController = gameObject.AddComponent<FreeCameraController>();
                            break;
                        case CameraModes.FlyCam:
                            newController = gameObject.AddComponent<FlyCameraController>();
                            break;
                        case CameraModes.Static:
                            newController = gameObject.AddComponent<StaticCameraController>();
                            break;
                    }
                }
				//Setup the new controller if it exists.
                if (newController != null)
                    newController.Setup();
            }
        }

		//The camera mode can't be switched multiple times in one framee, so this queue will process changes frame by frame.
        public Queue<CameraModes> cameraModeUpdates = new Queue<CameraModes>();
		//Add a camera mode to the queue.
        public void SetCameraMode(CameraModes newMode)
        {
            cameraModeUpdates.Enqueue(newMode);
        }

        public CameraController GetCameraController()
        {
            if (CameraMode > 0)
                return GetComponent<CameraController>();
            else
                return null;
        }

        // Use this for initialization
        void Start()
        {
            DontDestroyOnLoad(gameObject);
            camPoint = gameObject;
            cam = camPoint.transform.Find("Camera").gameObject;

            SetCameraMode(CameraModes.FlyCam);
        }

        //Camera mode changes need to be performed frame by frame, to ensure that controllers have been initialized.
        void Update()
        {
            if(cameraModeUpdates.Count > 0)
                CameraMode = cameraModeUpdates.Dequeue();

			ChangeCameraMode();
        }

        void OnSceneLoaded(Scene newScene, LoadSceneMode newSceneLoadMode)
        {
            //If a new single scene has been loaded, setup the camera.
            if (newSceneLoadMode == LoadSceneMode.Single)
            {
                //When the scene changes, we don't need the old updates anymore.
                cameraModeUpdates = new Queue<CameraModes>();

                CameraMode = CameraModes.None;
            }
        }

        void SetupCameraController()
        {
            if (GetCameraController() == null)
                return;
            else
                GetCameraController().Setup();
        }

        void RemoveCameraController()
        {
            //Remove script of type CameraController
            Destroy(GetComponent<CameraController>());
        }


        //Debug, throw me into update.
        void ChangeCameraMode()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
				//if (CameraMode == CameraModes.SpectatorThirdPerson)
				//	SetCameraMode(CameraModes.FreeCam);
				//else
				//	SetCameraMode(CameraModes.SpectatorThirdPerson);

				//UserFeedback.LogError("Changed Camera Mode.");
            }
        }

        //Usually a set parent method would be called,
        //But each camera controller inherently updates the position on start.
        public Transform CameraParent
        {
            set{ gameObject.transform.SetParent(value, false); }
            get { return camPoint.transform.parent; }
        }

    }
}