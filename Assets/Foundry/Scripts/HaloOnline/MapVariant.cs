using Foundry.IO;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Foundry.HaloOnline
{
	public class MapVariant
	{
        public static MapVariant Deserialize(StreamHelper streamHelper)
        {
            MapVariant mapVariant = new MapVariant();

            streamHelper.Skip(4);//mapVariant._blf = streamHelper.ReadInt32();
            mapVariant.blfUnknown = streamHelper.ReadBytes(0x2C);
            streamHelper.Skip(4);//mapVariant.chdr = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown1 = streamHelper.ReadBytes(0x14);
            mapVariant.chdrHeaderName = streamHelper.ReadUTF16(0x20);
            mapVariant.chdrHeaderDescription = streamHelper.ReadAscii(0x80);
            mapVariant.chdrHeaderAuthor = streamHelper.ReadAscii(0x10);
            mapVariant.chdrHeaderSize = streamHelper.ReadInt64();
            mapVariant.chdrHeaderCreationDate = streamHelper.ReadInt64();
            mapVariant.chdrHeaderUnknown2 = streamHelper.ReadInt64();
            mapVariant.chdrHeaderUnknown3 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown4 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown5 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown6 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderMapID = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown7 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown8 = streamHelper.ReadInt32();
            mapVariant.chdrHeaderUnknown9 = streamHelper.ReadBytes(0xC);
            streamHelper.Skip(4);//mapVariant.mapv = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown1 = streamHelper.ReadBytes(0x14);
            mapVariant.mapvHeaderName = streamHelper.ReadUTF16(0x20);
            mapVariant.mapvHeaderDescription = streamHelper.ReadAscii(0x80);
            mapVariant.mapvHeaderAuthor = streamHelper.ReadAscii(0x10);
            mapVariant.mapvHeaderSize = streamHelper.ReadInt64();
            mapVariant.mapvHeaderCreationDate = streamHelper.ReadInt64();
            mapVariant.mapvHeaderUnknown2 = streamHelper.ReadInt64();
            mapVariant.mapvHeaderUnknown3 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown4 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown5 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown6 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderMapID = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown7 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown8 = streamHelper.ReadInt32();
            mapVariant.mapvHeaderUnknown9 = streamHelper.ReadBytes(0xC);

            mapVariant.scnrObjectsCount = streamHelper.ReadInt16();
            mapVariant.totalObjectsCount = streamHelper.ReadInt16();
            mapVariant.budgetEntriesCount = streamHelper.ReadInt16();
            mapVariant.sandboxPlacementsCount = streamHelper.ReadInt16();
            mapVariant.mapID = streamHelper.ReadInt32();
            mapVariant.worldBoundsX = streamHelper.ReadVector2();
            mapVariant.worldBoundsY = streamHelper.ReadVector2();
            mapVariant.worldBoundsZ = streamHelper.ReadVector2();
            mapVariant.unknown1 = streamHelper.ReadInt32();
            mapVariant.maxBudget = streamHelper.ReadFloat();
            mapVariant.currentBudget = streamHelper.ReadFloat();
            mapVariant.unknown2 = streamHelper.ReadInt32();
            mapVariant.unknown3 = streamHelper.ReadInt32();

            mapVariant.sandboxPlacements = new List<SandboxPlacement>();
            for (int i = 0; i < 640; i++)
            {
                mapVariant.sandboxPlacements.Add(SandboxPlacement.Deserialize(streamHelper));
            }

            mapVariant.unknown4 = streamHelper.ReadBytes(0x38);

            mapVariant.budgetEntries = new List<BudgetEntry>();
            for (int i = 0; i < 256; i++)
            {
                mapVariant.budgetEntries.Add(BudgetEntry.Deserialize(streamHelper));
            }

            mapVariant.unknownPad = streamHelper.ReadBytes(0x140);

            streamHelper.Skip(4); //mapVariant._eof = streamHelper.ReadInt32();

            mapVariant.unknown5 = streamHelper.ReadBytes(0xC);
            mapVariant.emptyPad = streamHelper.ReadBytes(0xE1C);

            return mapVariant;
        }

        public void Serialize(StreamHelper streamHelper)
        {
            //streamHelper.SeekTo(0x138);
            //if (!streamHelper.ReadAscii(4).Equals("mapv"))
            //{
            //    throw new Exception("expected mapv magic.");
            //}

            streamHelper.WriteInt32(_blf);
            streamHelper.WriteBytes(blfUnknown);

            streamHelper.WriteInt32(chdr);
            streamHelper.WriteBytes(chdrHeaderUnknown1);
            streamHelper.WriteUTF16(chdrHeaderName, 0x20);
            streamHelper.WriteAscii(chdrHeaderDescription, 0x80);
            streamHelper.WriteAscii(chdrHeaderAuthor, 0x10);
            streamHelper.WriteInt64(chdrHeaderSize);
            streamHelper.WriteInt64(chdrHeaderCreationDate);
            streamHelper.WriteInt64(chdrHeaderUnknown2);
            streamHelper.WriteInt32(chdrHeaderUnknown3);
            streamHelper.WriteInt32(chdrHeaderUnknown4);
            streamHelper.WriteInt32(chdrHeaderUnknown5);
            streamHelper.WriteInt32(chdrHeaderUnknown6);
            streamHelper.WriteInt32(chdrHeaderMapID);
            streamHelper.WriteInt32(chdrHeaderUnknown7);
            streamHelper.WriteInt32(chdrHeaderUnknown8);
            streamHelper.WriteBytes(chdrHeaderUnknown9);

            streamHelper.WriteInt32(mapv);
            streamHelper.WriteBytes(mapvHeaderUnknown1);
            streamHelper.WriteUTF16(mapvHeaderName, 0x20);
            streamHelper.WriteAscii(mapvHeaderDescription, 0x80);
            streamHelper.WriteAscii(mapvHeaderAuthor, 0x10);
            streamHelper.WriteInt64(mapvHeaderSize);
            streamHelper.WriteInt64(mapvHeaderCreationDate);
            streamHelper.WriteInt64(mapvHeaderUnknown2);
            streamHelper.WriteInt32(mapvHeaderUnknown3);
            streamHelper.WriteInt32(mapvHeaderUnknown4);
            streamHelper.WriteInt32(mapvHeaderUnknown5);
            streamHelper.WriteInt32(mapvHeaderUnknown6);
            streamHelper.WriteInt32(mapvHeaderMapID);
            streamHelper.WriteInt32(mapvHeaderUnknown7);
            streamHelper.WriteInt32(mapvHeaderUnknown8);
            streamHelper.WriteBytes(mapvHeaderUnknown9);

            streamHelper.WriteInt16(scnrObjectsCount);
            streamHelper.WriteInt16(totalObjectsCount);
            streamHelper.WriteInt16(budgetEntriesCount);
            streamHelper.WriteInt16(sandboxPlacementsCount);
            streamHelper.WriteInt32(mapID);
            streamHelper.WriteVector2(worldBoundsX);
            streamHelper.WriteVector2(worldBoundsY);
            streamHelper.WriteVector2(worldBoundsZ);
            streamHelper.WriteInt32(unknown1);
            streamHelper.WriteFloat(maxBudget);
            streamHelper.WriteFloat(currentBudget);
            streamHelper.WriteInt32(unknown2);
            streamHelper.WriteInt32(unknown3);

            foreach (SandboxPlacement placement in sandboxPlacements)
            {
                placement.Serialize(streamHelper);
            }

            streamHelper.WriteBytes(unknown4);

            foreach (BudgetEntry entry in budgetEntries)
            {
                entry.Serialize(streamHelper);
            }

            streamHelper.WriteBytes(unknownPad);

            streamHelper.WriteInt32(_eof);

            streamHelper.WriteBytes(unknown5);
            streamHelper.WriteBytes(emptyPad);
        }

        public string VariantName { get { return chdrHeaderName; } set { chdrHeaderName = mapvHeaderName = value; } }
        public string VariantDescription { get { return chdrHeaderDescription; } set { chdrHeaderDescription = mapvHeaderDescription = value; } }
        public string VariantAuthor { get { return chdrHeaderAuthor; } set { chdrHeaderAuthor = mapvHeaderAuthor = value; } }
        public long VariantSize { get { return chdrHeaderSize; } set { chdrHeaderSize = mapvHeaderSize = value; } }
        public long VariantCreationDate { get { return chdrHeaderCreationDate; } set { chdrHeaderCreationDate = mapvHeaderCreationDate = value; } }
        public int VariantMapID { get { return chdrHeaderMapID; } set { chdrHeaderMapID = mapvHeaderMapID = mapID = value; } }

        private readonly int _blf = 0x5F626C66; //"_blf"
        private byte[] blfUnknown = new byte[0x2C];

        private readonly int chdr = 0x63686472; //"chdr"
        private byte[] chdrHeaderUnknown1 = new byte[0x14];
        private string chdrHeaderName;
        private string chdrHeaderDescription;
        private string chdrHeaderAuthor;
        private long chdrHeaderSize;
        private long chdrHeaderCreationDate;
        private long chdrHeaderUnknown2;
        private int chdrHeaderUnknown3;
        private int chdrHeaderUnknown4;
        private int chdrHeaderUnknown5;
        private int chdrHeaderUnknown6 = -1;
        private int chdrHeaderMapID;
        private int chdrHeaderUnknown7;
        private int chdrHeaderUnknown8 = -1;
        private byte[] chdrHeaderUnknown9 = new byte[0xC];

        private readonly int mapv = 0x6D617076; //"mapv"
        private byte[] mapvHeaderUnknown1 = new byte[0x14];
        private string mapvHeaderName;
        private string mapvHeaderDescription;
        private string mapvHeaderAuthor;
        private long mapvHeaderSize;
        private long mapvHeaderCreationDate;
        private long mapvHeaderUnknown2;
        private int mapvHeaderUnknown3;
        private int mapvHeaderUnknown4;
        private int mapvHeaderUnknown5;
        private int mapvHeaderUnknown6 = -1;
        private int mapvHeaderMapID;
        private int mapvHeaderUnknown7;
        private int mapvHeaderUnknown8 = -1;
        private byte[] mapvHeaderUnknown9 = new byte[0xC];

        private short scnrObjectsCount;
        private short totalObjectsCount; //not sure
        public short budgetEntriesCount; //could probably property these
        public short sandboxPlacementsCount; //not sure
        private int mapID;
        private Vector2 worldBoundsX;
        private Vector2 worldBoundsY;
        private Vector2 worldBoundsZ;
        private int unknown1;
        private float maxBudget;
        private float currentBudget;
        private int unknown2;
        private int unknown3;

        public List<SandboxPlacement> sandboxPlacements; //640 of these
        private byte[] unknown4 = new byte[0x38]; //there seem to be a bunch of shorts in here.
        public List<BudgetEntry> budgetEntries;

        public byte[] unknownPad = new byte[0x140]; //All bytes are 0xFF
        public int _eof = 0x5F656F66;

        private byte[] unknown5 = new byte[0xC]; //There seem to be some values after the end of file...
        public byte[] emptyPad = new byte[0xE1C];

        public class SandboxPlacement
        {
            public void Update(StreamHelper streamHelper)
            {
                streamHelper.SeekTo(offset);
                Serialize(streamHelper);
            }

            public void Serialize(StreamHelper streamHelper)
            {
                streamHelper.WriteUInt16((ushort)placementFlags);
                streamHelper.WriteInt16(unknown1);
                streamHelper.WriteInt32(objectDatumHandle);
                streamHelper.WriteInt32(gizmoDatumHandle);
                streamHelper.WriteInt32(budgetIndex);
                streamHelper.WriteVector3(position);
                streamHelper.WriteVector3(rightVector);
                streamHelper.WriteVector3(upVector);
                streamHelper.WriteInt32(unknown2);
                streamHelper.WriteInt32(unknown3);
                streamHelper.WriteInt16(engineFlags);
                streamHelper.WriteUInt8((byte)flags);
                streamHelper.WriteUInt8((byte)team);
                streamHelper.WriteUInt8(extra);
                streamHelper.WriteUInt8(respawnTime);
                streamHelper.WriteUInt8(objectType);
                streamHelper.WriteUInt8((byte)zoneShape);
                streamHelper.WriteFloat(zoneRadiusWidth);
                streamHelper.WriteFloat(zoneDepth);
                streamHelper.WriteFloat(zoneTop);
                streamHelper.WriteFloat(zoneBottom);
            }

            public static SandboxPlacement Deserialize(StreamHelper streamHelper)
            {
                SandboxPlacement placmeent = new SandboxPlacement();

                placmeent.offset = streamHelper.Position;

                placmeent.placementFlags = (PlacementFlags)streamHelper.ReadUInt16();
                placmeent.unknown1 = streamHelper.ReadInt16();
                placmeent.objectDatumHandle = streamHelper.ReadInt32();
                placmeent.gizmoDatumHandle = streamHelper.ReadInt32();
                placmeent.budgetIndex = streamHelper.ReadInt32();
                placmeent.position = streamHelper.ReadVector3();
                placmeent.rightVector = streamHelper.ReadVector3();
                placmeent.upVector = streamHelper.ReadVector3();
                placmeent.unknown2 = streamHelper.ReadInt32();
                placmeent.unknown3 = streamHelper.ReadInt32();
                placmeent.engineFlags = streamHelper.ReadInt16();
                placmeent.flags = (Flags)streamHelper.ReadUInt8();
                placmeent.team = (Teams)streamHelper.ReadUInt8();
                placmeent.extra = streamHelper.ReadUInt8();
                placmeent.respawnTime = streamHelper.ReadUInt8();
                placmeent.objectType = streamHelper.ReadUInt8();
                placmeent.zoneShape = (ZoneShapes)streamHelper.ReadUInt8();
                placmeent.zoneRadiusWidth = streamHelper.ReadFloat();
                placmeent.zoneDepth = streamHelper.ReadFloat();
                placmeent.zoneTop = streamHelper.ReadFloat();
                placmeent.zoneBottom = streamHelper.ReadFloat();

                return placmeent;
            }

            public static SandboxPlacement Null = new SandboxPlacement()
            {
                objectDatumHandle = -1,
                gizmoDatumHandle = -1,
                budgetIndex = -1,
                position = Vector3.zero,
                rightVector = Vector3.zero,
                upVector = Vector3.zero,
                unknown2 = -1,
                unknown3 = -1,
            };

            public enum Teams : byte
            {
                None,
                Red,
                Blue,
                Green
            }

            public enum ZoneShapes : byte
            {
                Cylinder,
                Rectangle
            }

            [Flags]
            public enum Flags : byte
            {
                Unknown
            }

            [Flags]
            public enum PlacementFlags : short
            {
                Unknown
            }

            private long offset;

            public PlacementFlags placementFlags;
            public short unknown1;
            public int objectDatumHandle;
            public int gizmoDatumHandle;
            public int budgetIndex;
            public Vector3 position;
            public Vector3 rightVector;
            public Vector3 upVector;
            public Vector3 ForwardVector { get { return Vector3.Cross(upVector, rightVector); } }
            public int unknown2;
            public int unknown3;
            public short engineFlags;
            public Flags flags;
            public Teams team;
            public byte extra;
            public byte respawnTime;
            public byte objectType;
            public ZoneShapes zoneShape;
            public float zoneRadiusWidth;
            public float zoneDepth;
            public float zoneTop;
            public float zoneBottom;

            public SandboxPlacement Clone()
            {
                return (SandboxPlacement)MemberwiseClone();
            }
        }

        public class BudgetEntry
        {
            public static BudgetEntry Deserialize(StreamHelper streamHelper)
            {
                var entry = new BudgetEntry();
                entry.tagIndex = streamHelper.ReadInt32();
                entry.runtimeMin = streamHelper.ReadUInt8();
                entry.runtimeMax = streamHelper.ReadUInt8();
                entry.countOnMap = streamHelper.ReadUInt8();
                entry.designTimeMax = streamHelper.ReadUInt8();
                entry.cost = streamHelper.ReadFloat();
                return entry;
            }

            public void Serialize(StreamHelper streamHelper)
            {
                streamHelper.WriteInt32(tagIndex);
                streamHelper.WriteUInt8(runtimeMin);
                streamHelper.WriteUInt8(runtimeMax);
                streamHelper.WriteUInt8(countOnMap);
                streamHelper.WriteUInt8(designTimeMax);
                streamHelper.WriteFloat(cost);
            }

            public static BudgetEntry Null = new BudgetEntry()
            {
                tagIndex = -1,
                designTimeMax = 0xFF,
                cost = 0x000080BF
            };

            public long offset;

            public int tagIndex;
            public byte runtimeMin;
            public byte runtimeMax;
            public byte countOnMap;
            public byte designTimeMax;
            public float cost;
        }
    }
}