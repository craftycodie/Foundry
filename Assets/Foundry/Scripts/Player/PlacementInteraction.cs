using UnityEngine;
using System.Collections;

namespace Foundry.Player
{
	public class PlacementInteraction : MonoBehaviour
	{
		public static PlacementInteraction singleton;

		//[HideInInspector]
		public GameObject heldPlacement;

		public LayerMask placementLayerMask;

		// Use this for initialization
		private void Start()
		{
			singleton = this;
		}

		private void OnDestroy()
		{
			singleton = null;
		}

		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (heldPlacement != null)
				{
					heldPlacement.transform.SetParent(null, true);
					heldPlacement = null;

					Session.SaveMapVariantPlacements();
				}
				else
				{
					Debug.DrawRay(transform.position, transform.forward, Color.red, 10f);

					Ray ray = new Ray(transform.position, transform.forward);
					RaycastHit rayHit;
					if (Physics.Raycast(ray, out rayHit, 10, placementLayerMask.value))
					{
						heldPlacement = rayHit.collider.gameObject.transform.root.gameObject;
						rayHit.collider.gameObject.transform.root.SetParent(transform, true);
					}
				}
			}
		}
	}
}