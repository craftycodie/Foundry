//using Foundry.IO;
//using System;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using VisualForge.Usermaps.Games;

//namespace Foundry.HaloOnline
//{
//	public class Sandbox
//	{
//		protected StreamHelper streamHelper;
//		string filename;
//		public Sandbox(string fileName)
//		{
//			streamHelper = new StreamHelper(new FileStream(fileName, FileMode.Open));
//			header = DeserializeContentHeader(streamHelper);
//			map = DeserializeSandboxMap(streamHelper);
//		}

//		public Sandbox(Stream stream)
//		{
//			streamHelper = new StreamHelper(stream);
//			header = DeserializeContentHeader(streamHelper);
//			map = DeserializeSandboxMap(streamHelper);
//		}

//		public void SaveFile()
//		{
//			//if(filename != null)
//			//	SaveFile(filename);
//			//else
//			//{
//			//Stream
//			SerializeContentHeader(streamHelper);
//			SerializeSandboxMap(streamHelper);
//			//}
//		}

//		public void SaveFile(string fileName)
//		{

//		}

//		~Sandbox()
//		{
//			streamHelper.Dispose();
//		}

//		public SandboxContentHeader header;
//		public class SandboxContentHeader
//		{
//			public long CreationDate1 { get; set; }
//			public string VarientName1 { get; set; }
//			public string VarientDescription1 { get; set; }
//			public string VarientAuthor1 { get; set; }

//			public long CreationDate2 { get; set; }
//			public string VarientName2 { get; set; }
//			public string VarientDescription2 { get; set; }
//			public string VarientAuthor2 { get; set; }

//			public Int32 MapID { get; set; }
//			public Int16 SpawnedObjectCount { get; set; }

//			public WorldBound WorldBoundsX { get; set; }
//			public WorldBound WorldBoundsY { get; set; }
//			public WorldBound WorldBoundsZ { get; set; }

//			public class WorldBound
//			{
//				public float Min { get; set; }
//				public float Max { get; set; }
//			}

//			public float MaximiumBudget { get; set; }
//			public float CurrentBudget { get; set; }
//		}

//		public SandboxMap map;
//		public class SandboxMap
//		{
//			public Int16 ScnrObjectCount { get; set; }
//			public Int16 TotalObjectCount { get; set; }
//			public Int16 BudgetEntryCount { get; set; }
//			public int MapId { get; set; }

//			public List<SandboxPlacement> Placements { get; set; }
//			public List<BudgetEntry> Budget { get; set; }
//		}

//		[Serializable]
//		public class TagEntry
//		{
//			public List<Halo3.ObjectChunk> placedObjects;
//			public long offset;
//			public int ident;
//			public
//		}

//		[Serializable]
//		public class SandboxPlacement
//		{
//			public static SandboxPlacement Null = new SandboxPlacement()
//			{
//				PlacementFlags = 0,
//				Unknown_1 = 0,
//				ObjectDatumHandle = 0xFFFFFFFF,
//				GizmoDatumHandle = 0xFFFFFFFF,
//				BudgetIndex = -1,
//				Position = new Vector3(),
//				rughtVector = new Vector3(),
//				upVector = new Vector3(),
//				Unknown_2 = 0,
//				Unknown_3 = 0,
//				EngineFlags = 0,
//				Flags = 0,
//				Team = 0,
//				Extra = 0,
//				RespawnTime = 0,
//				ObjectType = 0,
//				ZoneShape = 0,
//				ZoneRadiusWidth = 0,
//				ZoneDepth = 0,
//				ZoneTop = 0,
//				ZoneBottom = 0,
//			};

//			public enum Team : byte
//			{
//				None = 0,
//				Red = 1,
//				Blue = 2,
//				Green = 3,
//				Unknown1 = 4,
//				Unknown2 = 5,
//				Unknown3 = 6,
//				Unknown4 = 7,
//				Unknown5 = 8,
//				Unknown6 = 9,
//				Unknown7 = 10,
//				Unknown8 = 11,
//				Unknown9 = 12,
//				Unknown10 = 13,
//				Unknown11 = 14,
//				Unknown12 = 15,
//				Unknown13 = 16
//			}

//			public long offset;
//			public int tagIndex;
//			public Vector3 position;
//			public Vector3 rughtVector;
//			public Vector3 upVector;
//			public Vector3 ForwardVector { get { return Vector3.Cross(rughtVector, upVector); } }
//			public Team team;
//			public byte respawnTime;
//			public TagEntry tagEntry;

//			//public ushort PlacementFlags;
//			//public ushort Unknown_1;
//			//public UInt32 ObjectDatumHandle;
//			//public UInt32 GizmoDatumHandle;
//			//public int BudgetIndex;
//			//public Vector3 Position;
//			//public Vector3 RightVector;
//			//public Vector3 UpVector;
//			//public UInt32 Unknown_2;
//			//public UInt32 Unknown_3;
//			//public ushort EngineFlags;
//			//public byte Flags;
//			//public byte Team;
//			//public byte Extra;
//			//public byte RespawnTime;
//			//public byte ObjectType;
//			//public byte ZoneShape;
//			//public float ZoneRadiusWidth;
//			//public float ZoneDepth;
//			//public float ZoneTop;
//			//public float ZoneBottom;

//			public SandboxPlacement Clone()
//			{
//				return (SandboxPlacement)this.MemberwiseClone();
//			}
//		}

//		public class BudgetEntry
//		{
//			public UInt32 TagIndex;
//			public byte RuntimeMin;
//			public byte RuntimeMax;
//			public byte CountOnMap;
//			public byte DesignTimeMax;
//			public float Cost;
//		}

//		private static SandboxContentHeader DeserializeContentHeader(StreamHelper streamHelper)
//		{
//			var content = new SandboxContentHeader();
//			streamHelper.SeekTo(0x138);
//			if (!streamHelper.ReadAscii(4).Equals("mapv"))
//			{
//				throw new Exception("expected mapv magic.");
//			}

//			streamHelper.SeekTo(0x48);
//			content.VarientName1 = streamHelper.ReadUTF16(0x1F);
//			streamHelper.SeekTo(0x68);
//			content.VarientDescription1 = streamHelper.ReadAscii(0x80);
//			streamHelper.SeekTo(0xE8);
//			content.VarientAuthor1 = streamHelper.ReadAscii(0x13);
//			streamHelper.SeekTo(0x110);
//			content.CreationDate1 = streamHelper.ReadInt64();

//			streamHelper.SeekTo(0x150);
//			content.VarientName2 = streamHelper.ReadUTF16(0x1F);
//			streamHelper.SeekTo(0x170);
//			content.VarientDescription2 = streamHelper.ReadAscii(0x80);
//			streamHelper.SeekTo(0x1F0);
//			content.VarientAuthor2 = streamHelper.ReadAscii(0x13);
//			streamHelper.SeekTo(0x218);
//			content.CreationDate2 = streamHelper.ReadInt64();

//			streamHelper.SeekTo(0x228);
//			content.MapID = streamHelper.ReadInt32();

//			streamHelper.SeekTo(0x246);
//			content.SpawnedObjectCount = streamHelper.ReadInt16();

//			streamHelper.SeekTo(0x24C);
//			content.WorldBoundsX = new SandboxContentHeader.WorldBound
//			{
//				Min = streamHelper.ReadFloat(),
//				Max = streamHelper.ReadFloat()
//			};
//			content.WorldBoundsY = new SandboxContentHeader.WorldBound
//			{
//				Min = streamHelper.ReadFloat(),
//				Max = streamHelper.ReadFloat()
//			};
//			content.WorldBoundsZ = new SandboxContentHeader.WorldBound
//			{
//				Min = streamHelper.ReadFloat(),
//				Max = streamHelper.ReadFloat()
//			};

//			streamHelper.SeekTo(0x268);
//			content.MaximiumBudget = streamHelper.ReadFloat();
//			content.CurrentBudget = streamHelper.ReadFloat();
//			return content;
//		}

//		private void SerializeContentHeader(StreamHelper streamHelper)
//		{
//			streamHelper.SeekTo(0x138);
//			if (!streamHelper.ReadAscii(4).Equals("mapv"))
//			{
//				throw new Exception("expected mapv magic.");
//			}

//			streamHelper.SeekTo(0x48);
//			streamHelper.WriteUTF16(header.VarientName1, 0x1F);
//			streamHelper.SeekTo(0x68);
//			streamHelper.WriteAscii(header.VarientDescription1, 0x80);
//			streamHelper.SeekTo(0xE8);
//			streamHelper.WriteAscii(header.VarientAuthor1, 0x13);
//			streamHelper.SeekTo(0x110);
//			streamHelper.WriteInt64(header.CreationDate1);

//			streamHelper.SeekTo(0x150);
//			streamHelper.WriteUTF16(header.VarientName2, 0x1F);
//			streamHelper.SeekTo(0x170);
//			streamHelper.WriteAscii(header.VarientDescription2, 0x80);
//			streamHelper.SeekTo(0x1F0);
//			streamHelper.WriteAscii(header.VarientAuthor2, 0x13);
//			streamHelper.SeekTo(0x218);
//			streamHelper.WriteInt64(header.CreationDate2);

//			streamHelper.SeekTo(0x228);
//			streamHelper.WriteInt32(header.MapID);

//			streamHelper.SeekTo(0x246);
//			streamHelper.WriteInt16(header.SpawnedObjectCount);

//			streamHelper.SeekTo(0x24C);
//			streamHelper.WriteFloat(header.WorldBoundsX.Min);
//			streamHelper.WriteFloat(header.WorldBoundsX.Max);
//			streamHelper.WriteFloat(header.WorldBoundsY.Min);
//			streamHelper.WriteFloat(header.WorldBoundsY.Max);
//			streamHelper.WriteFloat(header.WorldBoundsZ.Min);
//			streamHelper.WriteFloat(header.WorldBoundsZ.Max);

//			streamHelper.SeekTo(0x268);
//			streamHelper.WriteFloat(header.MaximiumBudget);
//			streamHelper.WriteFloat(header.CurrentBudget);
//		}

//		private static SandboxMap DeserializeSandboxMap(StreamHelper streamHelper)
//		{
//			var map = new SandboxMap();

//			streamHelper.SeekTo(0x228);
//			map.MapId = streamHelper.ReadInt32();

//			streamHelper.SeekTo(0x242);
//			map.ScnrObjectCount = streamHelper.ReadInt16();
//			map.TotalObjectCount = streamHelper.ReadInt16();
//			map.BudgetEntryCount = streamHelper.ReadInt16();

//			map.Placements = new List<SandboxPlacement>();

//			streamHelper.SeekTo(0x278);
//			for (var i = 0; i < 640; i++)
//				map.Placements.Add(DeserializePlacement(streamHelper));

//			map.Budget = new List<BudgetEntry>();

//			streamHelper.SeekTo(0xD498);
//			for (var i = 0; i < 256; i++)
//				map.Budget.Add(DeserializeBudgetEntry(streamHelper));

//			return map;
//		}

//		private void SerializeSandboxMap(StreamHelper streamHelper)
//		{
//			streamHelper.SeekTo(0x228);
//			streamHelper.WriteInt32(map.MapId);

//			streamHelper.SeekTo(0x242);
//			streamHelper.WriteInt16(map.ScnrObjectCount);
//			streamHelper.WriteInt16(map.TotalObjectCount);
//			streamHelper.WriteInt16(map.BudgetEntryCount);

//			streamHelper.SeekTo(0x278);
//			Debug.Log("Saving " + map.Placements.Count + " placements.");
//			foreach (SandboxPlacement placement in map.Placements)
//			{
//				SerializePlacement(streamHelper, placement);
//			}
//			for (int i = 0; i < 640 - map.Placements.Count; i++)
//			{
//				SerializePlacement(streamHelper, SandboxPlacement.Null);
//			}

//			streamHelper.SeekTo(0xD498);
//			foreach (BudgetEntry budgetEntry in map.Budget)
//			{
//				SerializeBudgetEntry(streamHelper, budgetEntry);
//			}

//		}

//		public static Vector3 DeserializeVector3(StreamHelper streamHelper)
//		{
//			float x = streamHelper.ReadFloat();
//			float y = streamHelper.ReadFloat();
//			float z = streamHelper.ReadFloat();

//			return new Vector3 { x = x, y = z, z = y };
//		}

//		public static void SerializeBudgetEntry(StreamHelper streamHelper, BudgetEntry entry)
//		{
//			streamHelper.WriteUInt32(entry.TagIndex);
//			streamHelper.WriteUInt8(entry.RuntimeMin);
//			streamHelper.WriteUInt8(entry.RuntimeMax);
//			streamHelper.WriteUInt8(entry.CountOnMap);
//			streamHelper.WriteUInt8(entry.DesignTimeMax);
//			streamHelper.WriteFloat(entry.Cost);
//		}

//		public static BudgetEntry DeserializeBudgetEntry(StreamHelper streamHelper)
//		{
//			var entry = new BudgetEntry();
//			entry.TagIndex = streamHelper.ReadUInt32();
//			entry.RuntimeMin = streamHelper.ReadUInt8();
//			entry.RuntimeMax = streamHelper.ReadUInt8();
//			entry.CountOnMap = streamHelper.ReadUInt8();
//			entry.DesignTimeMax = streamHelper.ReadUInt8();
//			entry.Cost = streamHelper.ReadFloat();
//			return entry;
//		}

//		public static void SerializeVector3(StreamHelper streamHelper, Vector3 vector)
//		{
//			streamHelper.WriteFloat(vector.x);
//			streamHelper.WriteFloat(vector.y);
//			streamHelper.WriteFloat(vector.z);
//		}

//		public static void SerializePlacement(StreamHelper streamHelper, SandboxPlacement placement)
//		{
//			streamHelper.WriteUInt16(placement.PlacementFlags);
//			streamHelper.WriteUInt16(placement.Unknown_1);
//			streamHelper.WriteUInt32(placement.ObjectDatumHandle);
//			streamHelper.WriteUInt32(placement.GizmoDatumHandle);
//			streamHelper.WriteInt32(placement.BudgetIndex);
//			SerializeVector3(streamHelper, placement.Position);
//			SerializeVector3(streamHelper, placement.rughtVector);
//			SerializeVector3(streamHelper, placement.upVector);
//			streamHelper.WriteUInt32(placement.Unknown_2);
//			streamHelper.WriteUInt32(placement.Unknown_3);
//			streamHelper.WriteUInt16(placement.EngineFlags);
//			streamHelper.WriteUInt8(placement.Flags);
//			streamHelper.WriteUInt8(placement.Team);
//			streamHelper.WriteUInt8(placement.Extra);
//			streamHelper.WriteUInt8(placement.RespawnTime);
//			streamHelper.WriteUInt8(placement.ObjectType);
//			streamHelper.WriteUInt8(placement.ZoneShape);
//			streamHelper.WriteFloat(placement.ZoneRadiusWidth);
//			streamHelper.WriteFloat(placement.ZoneDepth);
//			streamHelper.WriteFloat(placement.ZoneTop);
//			streamHelper.WriteFloat(placement.ZoneBottom);
//		}

//		public static SandboxPlacement DeserializePlacement(StreamHelper streamHelper)
//		{
//			var placement = new SandboxPlacement();
//			placement.PlacementFlags = streamHelper.ReadUInt16();
//			//placement.Unknown_1 = streamHelper.ReadUInt16();
//			//placement.ObjectDatumHandle = streamHelper.ReadUInt32();
//			//placement.GizmoDatumHandle = streamHelper.ReadUInt32();
//			//placement.BudgetIndex = streamHelper.ReadInt32();
//			//placement.Position = DeserializeVector3(streamHelper);
//			//placement.RightVector = DeserializeVector3(streamHelper);
//			//placement.UpVector = DeserializeVector3(streamHelper);
//			//placement.Unknown_2 = streamHelper.ReadUInt32();
//			//placement.Unknown_3 = streamHelper.ReadUInt32();
//			//placement.EngineFlags = streamHelper.ReadUInt16();
//			//placement.Flags = streamHelper.ReadUInt8();
//			//placement.Team = streamHelper.ReadUInt8();
//			//placement.Extra = streamHelper.ReadUInt8();
//			//placement.RespawnTime = streamHelper.ReadUInt8();
//			//placement.ObjectType = streamHelper.ReadUInt8();
//			//placement.ZoneShape = streamHelper.ReadUInt8();
//			//placement.ZoneRadiusWidth = streamHelper.ReadFloat();
//			//placement.ZoneDepth = streamHelper.ReadFloat();
//			//placement.ZoneTop = streamHelper.ReadFloat();
//			//placement.ZoneBottom = streamHelper.ReadFloat();
//			return placement;
//		}

//	}
//}
