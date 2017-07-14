using Foundry.IO;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Foundry.HaloOnline
{
    public class Sandbox
    {
        protected StreamHelper streamHelper;
        public Sandbox(string fileName)
        {
            streamHelper = new StreamHelper(new FileStream(fileName, FileMode.Open));
            header = DeserializeContentHeader(streamHelper);
            map = DeserializeSandboxMap(streamHelper);
        }

        ~Sandbox()
        {
            streamHelper.Dispose();
        }

        public SandboxContentHeader header;
        public class SandboxContentHeader
        {
            public Int32 CreationDate { get; set; }
            public string CreationVarientName { get; set; }
            public string CreationVarientDescription { get; set; }
            public string CreationVarientAuthor { get; set; }

            public Int32 ModificationDate { get; set; }
            public string VarientName { get; set; }
            public string VarientDescription { get; set; }
            public string VarientAuthor { get; set; }

            public Int32 MapID { get; set; }
            public Int16 SpawnedObjectCount { get; set; }

            public WorldBound WorldBoundsX { get; set; }
            public WorldBound WorldBoundsY { get; set; }
            public WorldBound WorldBoundsZ { get; set; }

            public class WorldBound
            {
                public float Min { get; set; }
                public float Max { get; set; }
            }

            public float MaximiumBudget { get; set; }
            public float CurrentBudget { get; set; }
        }

        public SandboxMap map;
        public class SandboxMap
        {
            public Int16 ScnrObjectCount { get; set; }
            public Int16 TotalObjectCount { get; set; }
            public Int16 BudgetEntryCount { get; set; }
            public int MapId { get; set; }

            public List<SandboxPlacement> Placements { get; set; }
            public List<BudgetEntry> Budget { get; set; }
        }

        public class SandboxPlacement
        {
            public static SandboxPlacement Null = new SandboxPlacement()
            {
                PlacementFlags = 0,
                Unknown_1 = 0,
                ObjectDatumHandle = 0xFFFFFFFF,
                GizmoDatumHandle = 0xFFFFFFFF,
                BudgetIndex = -1,
                Position = new Vector3(),
                RightVector = new Vector3(),
                UpVector = new Vector3(),
                Unknown_2 = 0,
                Unknown_3 = 0,
                EngineFlags = 0,
                Flags = 0,
                Team = 0,
                Extra = 0,
                RespawnTime = 0,
                ObjectType = 0,
                ZoneShape = 0,
                ZoneRadiusWidth = 0,
                ZoneDepth = 0,
                ZoneTop = 0,
                ZoneBottom = 0,
            };

            public ushort PlacementFlags;
            public ushort Unknown_1;
            public UInt32 ObjectDatumHandle;
            public UInt32 GizmoDatumHandle;
            public int BudgetIndex;
            public Vector3 Position;
            public Vector3 RightVector;
            public Vector3 UpVector;
            public UInt32 Unknown_2;
            public UInt32 Unknown_3;
            public ushort EngineFlags;
            public byte Flags;
            public byte Team;
            public byte Extra;
            public byte RespawnTime;
            public byte ObjectType;
            public byte ZoneShape;
            public float ZoneRadiusWidth;
            public float ZoneDepth;
            public float ZoneTop;
            public float ZoneBottom;

            public SandboxPlacement Clone()
            {
                return (SandboxPlacement)this.MemberwiseClone();
            }
        }

        public class BudgetEntry
        {
            public UInt32 TagIndex;
            public byte RuntimeMin;
            public byte RuntimeMax;
            public byte CountOnMap;
            public byte DesignTimeMax;
            public float Cost;
        }

        private static SandboxContentHeader DeserializeContentHeader(StreamHelper streamHelper)
        {
            var content = new SandboxContentHeader();
            streamHelper.SeekTo(0x138);
            if (!streamHelper.ReadAscii(4).Equals("mapv"))
            {
                throw new Exception("expected mapv magic.");
            }

            streamHelper.SeekTo(0x42);
            content.CreationDate = streamHelper.ReadInt32();
            streamHelper.SeekTo(0x48);
            content.CreationVarientName = streamHelper.ReadUTF16(0x1F);
            streamHelper.SeekTo(0x68);
            content.CreationVarientDescription = streamHelper.ReadAscii(0x80);
            streamHelper.SeekTo(0xE8);
            content.CreationVarientAuthor = streamHelper.ReadAscii(0x13);

            streamHelper.SeekTo(0x114);
            content.ModificationDate = streamHelper.ReadInt32();
            streamHelper.SeekTo(0x150);
            content.VarientName = streamHelper.ReadUTF16(0x1F);
            streamHelper.SeekTo(0x170);
            content.VarientDescription = streamHelper.ReadAscii(0x80);
            streamHelper.SeekTo(0x1F0);
            content.VarientAuthor = streamHelper.ReadAscii(0x13);

            streamHelper.SeekTo(0x228);
            content.MapID = streamHelper.ReadInt32();

            streamHelper.SeekTo(0x246);
            content.SpawnedObjectCount = streamHelper.ReadInt16();

            streamHelper.SeekTo(0x24C);
            content.WorldBoundsX = new SandboxContentHeader.WorldBound
            {
                Min = streamHelper.ReadFloat(),
                Max = streamHelper.ReadFloat()
            };
            content.WorldBoundsY = new SandboxContentHeader.WorldBound
            {
                Min = streamHelper.ReadFloat(),
                Max = streamHelper.ReadFloat()
            };
            content.WorldBoundsZ = new SandboxContentHeader.WorldBound
            {
                Min = streamHelper.ReadFloat(),
                Max = streamHelper.ReadFloat()
            };

            streamHelper.SeekTo(0x268);
            content.MaximiumBudget = streamHelper.ReadFloat();
            content.CurrentBudget = streamHelper.ReadFloat();
            return content;
        }

        private static SandboxMap DeserializeSandboxMap(StreamHelper streamHelper)
        {
            var map = new SandboxMap();

            streamHelper.SeekTo(0x228);
            map.MapId = streamHelper.ReadInt32();

            streamHelper.SeekTo(0x242);
            map.ScnrObjectCount = streamHelper.ReadInt16();
            map.TotalObjectCount = streamHelper.ReadInt16();
            map.BudgetEntryCount = streamHelper.ReadInt16();

            map.Placements = new List<SandboxPlacement>();

            streamHelper.SeekTo(0x278);
            for (var i = 0; i < 640; i++)
                map.Placements.Add(DeserializePlacement(streamHelper));

            map.Budget = new List<BudgetEntry>();

            streamHelper.SeekTo(0xD498);
            for (var i = 0; i < 256; i++)
                map.Budget.Add(DeserializeBudgetEntry(streamHelper));

            return map;
        }

        public static Vector3 DeserializeVector3(StreamHelper streamHelper)
        {
            float x = streamHelper.ReadFloat();
            float y = streamHelper.ReadFloat();
            float z = streamHelper.ReadFloat();

            return new Vector3 { x = x, y = z, z = y };
        }

        public static void SerializeBudgetEntry(StreamHelper streamHelper, BudgetEntry entry)
        {
            streamHelper.WriteUInt32(entry.TagIndex);
            streamHelper.WriteUInt8(entry.RuntimeMin);
            streamHelper.WriteUInt8(entry.RuntimeMax);
            streamHelper.WriteUInt8(entry.CountOnMap);
            streamHelper.WriteUInt8(entry.DesignTimeMax);
            streamHelper.WriteFloat(entry.Cost);
        }

        public static BudgetEntry DeserializeBudgetEntry(StreamHelper streamHelper)
        {
            var entry = new BudgetEntry();
            entry.TagIndex = streamHelper.ReadUInt32();
            entry.RuntimeMin = streamHelper.ReadUInt8();
            entry.RuntimeMax = streamHelper.ReadUInt8();
            entry.CountOnMap = streamHelper.ReadUInt8();
            entry.DesignTimeMax = streamHelper.ReadUInt8();
            entry.Cost = streamHelper.ReadFloat();
            return entry;
        }

        public static void SerializeVector3(StreamHelper streamHelper, Vector3 vector)
        {
            streamHelper.WriteFloat(vector.x);
            streamHelper.WriteFloat(vector.y);
            streamHelper.WriteFloat(vector.z);
        }

        public static void SerializePlacement(StreamHelper streamHelper, SandboxPlacement placement)
        {
            streamHelper.WriteUInt16(placement.PlacementFlags);
            streamHelper.WriteUInt16(placement.Unknown_1);
            streamHelper.WriteUInt32(placement.ObjectDatumHandle);
            streamHelper.WriteUInt32(placement.GizmoDatumHandle);
            streamHelper.WriteInt32(placement.BudgetIndex);
            SerializeVector3(streamHelper, placement.Position);
            SerializeVector3(streamHelper, placement.RightVector);
            SerializeVector3(streamHelper, placement.UpVector);
            streamHelper.WriteUInt32(placement.Unknown_2);
            streamHelper.WriteUInt32(placement.Unknown_3);
            streamHelper.WriteUInt16(placement.EngineFlags);
            streamHelper.WriteUInt8(placement.Flags);
            streamHelper.WriteUInt8(placement.Team);
            streamHelper.WriteUInt8(placement.Extra);
            streamHelper.WriteUInt8(placement.RespawnTime);
            streamHelper.WriteUInt8(placement.ObjectType);
            streamHelper.WriteUInt8(placement.ZoneShape);
            streamHelper.WriteFloat(placement.ZoneRadiusWidth);
            streamHelper.WriteFloat(placement.ZoneDepth);
            streamHelper.WriteFloat(placement.ZoneTop);
            streamHelper.WriteFloat(placement.ZoneBottom);
        }

        public static SandboxPlacement DeserializePlacement(StreamHelper streamHelper)
        {
            var placement = new SandboxPlacement();
            placement.PlacementFlags = streamHelper.ReadUInt16();
            placement.Unknown_1 = streamHelper.ReadUInt16();
            placement.ObjectDatumHandle = streamHelper.ReadUInt32();
            placement.GizmoDatumHandle = streamHelper.ReadUInt32();
            placement.BudgetIndex = streamHelper.ReadInt32();
            placement.Position = DeserializeVector3(streamHelper);
            placement.RightVector = DeserializeVector3(streamHelper);
            placement.UpVector = DeserializeVector3(streamHelper);
            placement.Unknown_2 = streamHelper.ReadUInt32();
            placement.Unknown_3 = streamHelper.ReadUInt32();
            placement.EngineFlags = streamHelper.ReadUInt16();
            placement.Flags = streamHelper.ReadUInt8();
            placement.Team = streamHelper.ReadUInt8();
            placement.Extra = streamHelper.ReadUInt8();
            placement.RespawnTime = streamHelper.ReadUInt8();
            placement.ObjectType = streamHelper.ReadUInt8();
            placement.ZoneShape = streamHelper.ReadUInt8();
            placement.ZoneRadiusWidth = streamHelper.ReadFloat();
            placement.ZoneDepth = streamHelper.ReadFloat();
            placement.ZoneTop = streamHelper.ReadFloat();
            placement.ZoneBottom = streamHelper.ReadFloat();
            return placement;
        }

    }
}
