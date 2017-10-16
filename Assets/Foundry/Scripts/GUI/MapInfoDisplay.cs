using Foundry.Common.Helpers;
using Foundry.HaloOnline;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Foundry.GUI
{
	[RequireComponent(typeof(CanvasGroup))]
    public class MapInfoDisplay : MonoBehaviour
    {
		//Assigned in inspector.
        //public Text baseMapName;
        public InputField editedMapName;
        public InputField editedMapDescription;
        public InputField editedMapAuthor;
        public Text lastEditDate;

		//Required
		private CanvasGroup canvasGroup;

		bool open = false;
		bool Open
		{
			get { return open; }
			set
			{
				canvasGroup.alpha = value ? 1 : 0;
				canvasGroup.interactable = value;
				this.open = value;
				Session.Paused = value;

				if (value)
					UpdateDisplay(Session.mapVariantFile.MapVariant);
			}
		}


		//Unity's events aren't great.
		//With no sender, I need to track what's updating the map information.
		//So this bool will be set to true if scripts are updating the information.

		bool ignoreChangeEvents = false;

		public void SaveSandboxHeader()
		{
			if (ignoreChangeEvents)
				return;

			Session.mapVariantFile.MapVariant.VariantAuthor = editedMapAuthor.text.Replace("	", "");
			Session.mapVariantFile.MapVariant.VariantDescription = editedMapDescription.text.Replace("	", ""); ;
			Session.mapVariantFile.MapVariant.VariantName = editedMapName.text.Replace("	", ""); ;
			Session.mapVariantFile.MapVariant.VariantCreationDate = TimestampHelper.ToTimestamp(DateTime.Now);

			Session.mapVariantFile.SaveFile();

			UpdateDisplay(Session.mapVariantFile.MapVariant);
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Tab))
				Open = !Open;
		}

		private void Start()
        {
			canvasGroup = GetComponent<CanvasGroup>();

            //Session.SandboxUpdated += UpdateInfo;
            UpdateDisplay(Session.mapVariantFile.MapVariant);
        }

        void UpdateDisplay(MapVariant mapVariant)
        {
			ignoreChangeEvents = true;

			//baseMapName.text = Maps.GetMapByID(sandbox.header.MapID).mapName + ", " + sandbox.header.MapID;
            editedMapName.text = mapVariant.VariantName;
            editedMapDescription.text = mapVariant.VariantDescription;
            editedMapAuthor.text = mapVariant.VariantAuthor;
            lastEditDate.text = TimestampHelper.ToDateTime(mapVariant.VariantCreationDate).ToString();

			ignoreChangeEvents = false;
		}
    }
}