using Assets.Foundry.Scripts.HaloOnline;
using Foundry.Common.Helpers;
using Foundry.HaloOnline;
using Foundry.Objects.Forge;
using Foundry.Player;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Foundry
{
    public class Session : MonoBehaviour
    {
		//private static Session singleton;

	    //Underlying
		private static bool paused = false;
		public static bool Paused
		{
			get { return paused; }
			set
			{
				paused = value;
				Cursor.lockState = value ? CursorLockMode.None : CursorLockMode.Locked;
				Cursor.visible = value;
			}
		}

		public static MapVariantFile mapVariantFile;
        public delegate void MapVariantFileUpdated(MapVariantFile mapVariantFile);
        public static event MapVariantFileUpdated mapVariantFileUpdated;

        // Use this for initialization
        void Start()
        {
			//singleton = this;
			LoadSandbox();

			//Although it defaults to false,
			//The set accessor needs to be used.
			Paused = false;


            //Maps.MapsReady += LoadSandbox;
        }

		public static void SaveMapVariantPlacements()
		{

            //Invisibles are objects without budget data.
            //These could be scenery objects.
            //Because they aren't placed in the scene, they need to be collected from the sandbox before clearing the placmeents.
            //Then they can be added back with objects in the scene :)

            //List<Sandbox.SandboxPlacement> invisibles = new List<Sandbox.SandboxPlacement>();
            //foreach (Sandbox.SandboxPlacement placement in sandbox.map.placements)
            //{
            //	if (placement.budgetIndex == -1)
            //		invisibles.Add(placement);
            //}

            //sandbox.map.placements = new List<Sandbox.SandboxPlacement>();
            //int i = 1;
            foreach (ForgePlacement placement in FindObjectsOfType<ForgePlacement>())
            {
                placement.PlacementData.Update(mapVariantFile.streamHelper);
                //i++;
                //sandbox.map.placements.Add(placement.PlacementDate);
            }

            //Debug.Log("Found " + i + " placements in scene.");

            //sandbox.map.placements.AddRange(invisibles);

            //sandbox.header.CreationDate = TimestampHelper.ToTimestamp(DateTime.Now);

            //sandbox.SaveFile();
        }

        private void OnDestroy()
        {
            Debug.Log("Something destroyed the session!");

            //singleton = null;
            if(mapVariantFileUpdated != null)
                mapVariantFileUpdated(null);

            //Make sure the sandbox is properly disposed of.
            mapVariantFile = null;
        }

        void LoadSandbox()
        {

            //byte[] mapData = null;

            //using (var wc = new System.Net.WebClient())
            //    mapData = wc.DownloadData(@"C:\Users\AlexN\Desktop\jenga.map");

            //sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\No Elephants (Alex231).bin"); //Sandtrap
            //sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\fatkid.map"); //Guardian
            //sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\jenga.map"); //Standoff
            mapVariantFile = new MapVariantFile(@"C:\Users\AlexN\Desktop\sandbox.map"); //Guardian
			//sandbox = new Sandbox(new MemoryStream(mapData)); //Sandtrap
			//sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\sandbox1.map"); //sandbox1
			//sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\sandbox2.map"); //sandbox2
			//sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\sandtrap.map"); //sandtrap

			if (mapVariantFileUpdated != null)
                mapVariantFileUpdated(mapVariantFile);

			//Debug.Log("Processing " + sandbox.map.Placements.Count + " placements.");

			int count = 1;

            foreach (MapVariant.SandboxPlacement placement in mapVariantFile.MapVariant.sandboxPlacements)
            {
                GameObject go = null;

				if (placement.budgetIndex == -1)
				{
					continue;
					//go = GameObject.CreatePrimitive(PrimitiveType.Cube);
					//go.GetComponent<Renderer>().material = null;
				}
				else
				{
					count++;

					try
					{
						go = (GameObject)Instantiate(Resources.Load("Foundry/Tags/Prefabs/" + "0x" + mapVariantFile.MapVariant.budgetEntries[placement.budgetIndex].tagIndex.ToString("X4")/*sandbox.map.Budget[placement.BudgetIndex].TagIndex.ToString("X4")*/, typeof(GameObject)));
					}
					catch (ArgumentException ae)
					{
						go = GameObject.CreatePrimitive(PrimitiveType.Cube);
						Debug.LogAssertion("Couldn't find a prefab for " + "0x" + mapVariantFile.MapVariant.budgetEntries[placement.budgetIndex].tagIndex.ToString("X4")/*sandbox.map.Budget[placement.BudgetIndex].TagIndex.ToString("X4"))*/);
						go.GetComponent<Renderer>().material = null;
					}
					finally
					{
						go.name = "0x" + mapVariantFile.MapVariant.budgetEntries[placement.budgetIndex].tagIndex.ToString("X4")/*sandbox.map.Budget[placement.BudgetIndex].TagIndex.ToString("X4"))*/;
						//go.tag = "forgePlacement";
						go.layer = LayerMask.NameToLayer("forgePlacement");

						go.AddComponent<ForgePlacement>().PlacementData = placement;
					}
				}
			}
			Debug.Log("Added " + count + " placements to the scene.");

			Maps.AdditiveLoadMap(mapVariantFile.MapVariant.VariantMapID);
        }
    }
}