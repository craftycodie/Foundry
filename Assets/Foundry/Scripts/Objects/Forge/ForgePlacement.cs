using UnityEngine;
using System.Collections;
using Foundry.HaloOnline;
using System;
using Foundry.Common.Helpers;

namespace Foundry.Objects.Forge
{
	public class ForgePlacement : MonoBehaviour
	{
		private MapVariant.SandboxPlacement placementData;
		public MapVariant.SandboxPlacement PlacementData
		{
			get
			{
				placementData.position = Vector3Helper.SwapYZ(transform.position);

				//Create a guide for rotation.
				//This isn't necessary, there's probably a better way to get the vectors i need.
				//GameObject rotationGuide = GameObject.CreatePrimitive(PrimitiveType.Cube);
				//rotationGuide.transform.position = transform.position;
				//rotationGuide.transform.rotation = QuaternionHelper.SwapYZ(rotationGuide.transform.rotation);
				//rotationGuide.transform.Rotate(-270, 0, 0);

				//Quaternion rotation = transform.rotation;
				//rotation = QuaternionHelper.SwapYZ(rotation);

				Debug.DrawRay(transform.position, transform.up, Color.green, 10f);
				//Debug.DrawRay(transform.position, Vector3.up, Color.yellow, 10f);
				Debug.DrawRay(transform.position, transform.right, Color.red, 10f);
				//Debug.DrawRay(transform.position, Vector3.right, Color.magenta, 10f);
				Debug.DrawRay(transform.position, transform.forward, Color.blue, 10f);
                //Debug.DrawRay(transform.position, Vector3.forward, Color.cyan, 10f);

                //Debug.DrawRay(transform.position, transform.worldToLocalMatrix.MultiplyVector(transform.up), Color.red, 10f);
                //Debug.DrawRay(transform.position, transform.InverseTransformDirection(Vector3.up + Vector3.right + Vector3.forward), Color.blue, 10f);

                //placementData.upVector = transform.up;
                //placementData.rightVector = transform.right;

                //Destroy(rotationGuide);

                return placementData;
			}
			set
			{
				placementData = value;
				transform.position = Vector3Helper.SwapYZ(value.position);
				transform.rotation = QuaternionHelper.FromRightUpVectors(value.rightVector, value.upVector);
				//transform.Rotate(270, 0, 0);
			}
		}
	}
}
