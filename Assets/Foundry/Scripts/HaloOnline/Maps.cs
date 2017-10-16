using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Foundry.HaloOnline
{
    public static class Maps
    {
        //public delegate void MapsReadyEventHandler(List<Map> maps);
        //public static event MapsReadyEventHandler MapsReady;

        static Maps()
        {
            AssignSceneIDs();

            //if(MapsReady != null)
            //    MapsReady(maps);
        }

        private const string SCENES_PATH = "Assets/Foundry/levels/";

        private static void AssignSceneIDs()
        {
            List<string> scenePaths = new List<string>();

#if UNITY_EDITOR
            foreach (UnityEditor.EditorBuildSettingsScene scene in UnityEditor.EditorBuildSettings.scenes)
            {
                scenePaths.Add(scene.path);
            }
#else
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                scenePaths.Add(SceneManager.GetSceneAt(i).path); 
            }
#endif

            foreach(string scenePath in scenePaths)
            {
                if (scenePath == "")
                    continue;

                string foldersString = scenePath.Replace(SCENES_PATH, "");
                //Grab the folders from scenepath
                //Eg, multi/zanzibar/zanzibar becomes multi, zanzibar
                string[] folders = foldersString.Remove(foldersString.LastIndexOf('/')).Split('/');
                //Parse the map type from the folder.
                Map.MapType sceneMapType;

                try
                {
                    sceneMapType = (Map.MapType)Enum.Parse(typeof(Map.MapType), folders[0], true);
                }
                catch (ArgumentException ae)
                {
                    continue;
                }

                for (int i = 0; i < maps.Count; i++)
                {
                    if (maps[i].fileName == folders[1] && maps[i].mapType == sceneMapType)
                    {
                        maps[i].sceneID = SceneUtility.GetBuildIndexByScenePath(scenePath);
                        break;
                    }
                }
            }

        }

        public static List<Map> maps = new List<Map>()
        {
            //Halo 3
            new Map(Map.MapType.Multi, 30, "zanzibar", "Last Resort"),
            new Map(Map.MapType.Multi, 300, "construct", "Construct"),
            new Map(Map.MapType.Multi, 310, "deadlock", "High Ground"),
            new Map(Map.MapType.Multi, 320, "guardian", "Guardian"),
            new Map(Map.MapType.Multi, 330, "isolation", "Isolation"),
            new Map(Map.MapType.Multi, 340, "valhalla", "Valhalla"),
            new Map(Map.MapType.Multi, 350, "salvation", "Salvation"),
            new Map(Map.MapType.Multi, 360, "snowbound", "Snowbound"),
            new Map(Map.MapType.Multi, 380, "chill", "Narrows"),
            new Map(Map.MapType.Multi, 390, "cyberdyne", "The Pit"),
            new Map(Map.MapType.Multi, 400, "shrine", "Sandtrap"),
            new Map(Map.MapType.DLC, 410, "bunkerworld", "Standoff"),
            new Map(Map.MapType.DLC, 440, "docks", "Longshore"),
            new Map(Map.MapType.DLC, 470, "sidewinder", "Avalanche"),
            new Map(Map.MapType.DLC, 480, "warehouse", "Foundry"),
            new Map(Map.MapType.DLC, 490, "descent", "Assembly"),
            new Map(Map.MapType.DLC, 500, "spacecamp", "Orbital"),
            new Map(Map.MapType.DLC, 520, "lockout", "Blackout"),
            new Map(Map.MapType.DLC, 580, "armory", "Rat's Nest"),
            new Map(Map.MapType.DLC, 590, "ghosttown", "Ghost Town"),
            new Map(Map.MapType.DLC, 600, "chillout", "Cold Storage"),
            new Map(Map.MapType.DLC, 720, "midship", "Heretic"),
            new Map(Map.MapType.DLC, 730, "sandbox", "Sandbox"),
            new Map(Map.MapType.DLC, 740, "foretress", "Citadel"),
            //Halo Online
            new Map(Map.MapType.DLC, 31, "icebox", "Turf"),
            new Map(Map.MapType.DLC, 700, "reactor", "Reactor"),
            new Map(Map.MapType.DLC, 703, "edge", "Edge"),
            new Map(Map.MapType.DLC, 705, "diamondback", "Diamondback"),
            //Halo 3 Campaign
            new Map(Map.MapType.Solo, 3005, "005_intro", "Arrival"),
            new Map(Map.MapType.Solo, 3010, "010_jungle", "Sierra 117"),
            new Map(Map.MapType.Solo, 3020, "020_base", "Crow's Nest"),
            new Map(Map.MapType.Solo, 3030, "030_outskirts", "Tsavo Highway"),
            new Map(Map.MapType.Solo, 3040, "040_voi", "The Storm"),
            new Map(Map.MapType.Solo, 3050, "050_floodvoi", "Floodgate"),
            new Map(Map.MapType.Solo, 3070, "070_waste", "The Ark"),
            new Map(Map.MapType.Solo, 3100, "100_citadel", "The Covenant"),
            new Map(Map.MapType.Solo, 3110, "110_hc", "Cortana"),
            new Map(Map.MapType.Solo, 3120, "120_halo", "Halo"),
            new Map(Map.MapType.Solo, 3130, "130_epilogue", "Epilogue"),
            //Custom
            new Map(Map.MapType.Custom, 413, "xe_flatgrass", "Flatgrass" ),
            new Map(Map.MapType.Custom, 414, "lockout", "Lockout"),
            new Map(Map.MapType.Custom, 415, "station", "Station"),
            new Map(Map.MapType.Custom, 416, "hang-em-high", "Hang 'Em High")

        };

        public class Map
        {
            private Map() { }
            public Map(MapType mapType, int id, string fileName, string mapName)
            {
                this.mapType = mapType;
                this.id = id;
                this.fileName = fileName;
                this.mapName = mapName;
            }

            //These are used to differenciate custom maps.
            //Another structure, like H1,H2,H3,Custom could work too.
            //But for now I'd like to copy Halo 3's structure.
            public enum MapType
            {
                Mainmenu,
                Solo,
                Multi,
                DLC,
                Custom
            }

            public MapType mapType;
            public int id;
            //This is the .map file name, and also the scene name.
            public string fileName;
            public string mapName;
            public int sceneID = -1;
        }

        public static Map GetMapByID(int id)
        {
            foreach(Map map in maps)
            {
                if (map.id == id)
                    return map;
            }
            Debug.LogError("Couldn't find map");
            return null;
        }

        public static void AdditiveLoadMap(int id)
        {
            int sceneID = GetMapByID(id).sceneID;
            if (sceneID != -1)
                SceneManager.LoadScene(sceneID, LoadSceneMode.Additive);
            else
                Debug.LogError("No scene found for map " + GetMapByID(id).mapName);
        }
    }
}
