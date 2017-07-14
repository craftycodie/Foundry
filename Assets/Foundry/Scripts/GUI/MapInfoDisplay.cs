using UnityEngine;
using System.Collections;
using Foundry;
using UnityEngine.UI;
using Foundry.HaloOnline;
using System;

namespace Foundry.GUI
{
    public class MapInfoDisplay : MonoBehaviour
    {
        public Text baseMapName;
        public Text mapName;
        public Text editedMapName;
        public Text mapDescription;
        public Text editedMapDescription;
        public Text mapAuthor;
        public Text editedMapAuthor;
        public Text creationDate;
        public Text lastEditDate;


        private void Start()
        {
            //Session.singleton.sandboxUpdated += UpdateInfo;
            UpdateInfo(Session.singleton.sandbox);
        }

        void UpdateInfo(Sandbox sandbox)
        {
            baseMapName.text = Maps.mapIDs[sandbox.header.MapID] + ", " + sandbox.header.MapID;
            mapName.text = sandbox.header.CreationVarientName;
            editedMapName.text = sandbox.header.VarientName;
            mapDescription.text = sandbox.header.CreationVarientDescription;
            editedMapDescription.text = sandbox.header.VarientDescription;
            mapAuthor.text = sandbox.header.CreationVarientAuthor;
            editedMapAuthor.text = sandbox.header.VarientAuthor;

            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(sandbox.header.CreationDate).ToLocalTime();
            creationDate.text = dt.ToString();

            dt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(sandbox.header.ModificationDate).ToLocalTime();
            lastEditDate.text = dt.ToString();
        }
    }
}