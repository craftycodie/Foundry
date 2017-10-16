using Foundry.HaloOnline;
using Foundry.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Assets.Foundry.Scripts.HaloOnline
{
    public class MapVariantFile
    {
        public MapVariant MapVariant { get; private set; }

        public readonly StreamHelper streamHelper;

        public MapVariantFile(string fileName)
        {
            streamHelper = new StreamHelper(new FileStream(fileName, FileMode.Open));
            LoadFile();
        }

        ~MapVariantFile()
        {
            streamHelper.Dispose();
        }

        public void SaveFile()
        {
            //if(filename != null)
            //	SaveFile(filename);
            //else
            //{
            //Stream
            //header.Serialize(streamHelper);
            MapVariant.Serialize(streamHelper);
            //}
        }

        private void LoadFile()
        {
            MapVariant = MapVariant.Deserialize(streamHelper);
        }
    }
}
