using Foundry.HaloOnline;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Foundry
{
    public class Session : MonoBehaviour
    {
        public static Session singleton;


        public Sandbox sandbox;
        public delegate void SandboxUpdatedEventHandler(Sandbox sandbox);
        public event SandboxUpdatedEventHandler sandboxUpdated;

        // Use this for initialization
        void Start()
        {
            singleton = this;
            LoadSandbox();
        }

        private void OnDestroy()
        {
            Debug.Log("Something destroyed the session!");
            singleton = null;
            //sandboxUpdated(null);
        }

        void LoadSandbox()
        {
            sandbox = new Sandbox(@"C:\Users\AlexN\Desktop\sandbox.map");

            //sandboxUpdated(sandbox);

            foreach (Sandbox.SandboxPlacement placement in sandbox.map.Placements)
            {
                if (placement.BudgetIndex == -1)
                    continue;

                GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
                go.transform.position = placement.Position;

                go.name = "0x" + sandbox.map.Budget[placement.BudgetIndex].TagIndex.ToString("X4");
            }
        }
    }
}