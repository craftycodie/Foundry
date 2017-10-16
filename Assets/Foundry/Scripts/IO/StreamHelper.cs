using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Foundry.IO
{
    public class StreamHelper : IDisposable {

        public readonly Stream Stream;

        public StreamHelper(Stream stream)
        {
            Stream = stream;
        }

        public bool CanSeek { get { return Stream.CanSeek; } }
        public long Position { get { return Stream.Position; } set { Stream.Position = value; } }

        public long SeekTo(long offset)
        {
            return Stream.Seek(offset, SeekOrigin.Begin);
        }

        public void Skip(int bytes)
        {
            Stream.Seek(bytes, SeekOrigin.Current);
        }

        public Int32 ReadInt64()
        {
            return BitConverter.ToInt32(ReadValue(8), 0);
        }

        public UInt32 ReadUInt64()
        {
            return BitConverter.ToUInt32(ReadValue(8), 0);
        }

        public Int32 ReadInt32()
        {
            return BitConverter.ToInt32(ReadValue(4), 0);
        }

        public UInt32 ReadUInt32()
        {
            return BitConverter.ToUInt32(ReadValue(4), 0);
        }

        public Int16 ReadInt16()
        {
            return BitConverter.ToInt16(ReadValue(2), 0);
        }

        public UInt16 ReadUInt16()
        {
            return BitConverter.ToUInt16(ReadValue(2), 0);
        }

        public sbyte ReadInt8()
        {
            return (sbyte)Stream.ReadByte();
        }

        public byte ReadUInt8()
        {
            return (byte)Stream.ReadByte();
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ReadValue(4), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ReadValue(8), 0);
        }

        public void WriteInt64(Int64 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteUInt64(UInt64 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteInt32(Int32 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteUInt32(UInt32 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteInt16(Int16 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteUInt16(UInt16 value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteInt8(sbyte value)
        {
            Stream.WriteByte((byte)value);
        }

        public void WriteUInt8(byte value)
        {
            Stream.WriteByte(value);
        }

        public void WriteFloat(float value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public void WriteDouble(double value)
        {
            WriteValue(BitConverter.GetBytes(value));
        }

        public string ReadAscii(int length)
        {
            var buff = new byte[length];
            Stream.Read(buff, 0, length);
            return Encoding.ASCII.GetString(buff).TrimEnd('\0');
        }

        public string ReadUTF16(int length)
        {
            var buff = new byte[length];
            Stream.Read(buff, 0, length);
            return Encoding.Unicode.GetString(buff).TrimEnd('\0');
        }

        private byte[] ReadValue(int length)
        {
            var buff = new byte[length];
            Stream.Read(buff, 0, length);
            return buff;
        }

        private void WriteValue(byte[] bytes)
        {
            Stream.Write(bytes, 0, bytes.Length);
        }

        public byte[] ReadBytes(int count)
        {
            var buff = new byte[count];
            Stream.Read(buff, 0, count);
            return buff;
        }

		public Vector2 ReadVector2()
		{
			return new Vector2(ReadFloat(), ReadFloat());
		}

		public void WriteVector2(Vector2 value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
		}

		public Vector3 ReadVector3()
		{
			return new Vector3(ReadFloat(), ReadFloat(), ReadFloat());
		}

		public void WriteVector3(Vector3 value)
		{
			WriteFloat(value.x);
			WriteFloat(value.y);
			WriteFloat(value.z);
		}

        public void WriteBytes(byte[] bytes)
        {
            Stream.Write(bytes, 0, bytes.Length);
        }

        public void Dispose()
        {
            Stream.Dispose();
        }

        internal void WriteUTF16(string value, int length)
        {
            WriteBytes(Encoding.Unicode.GetBytes(value.PadRight(length, '\0')));
        }

        internal void WriteAscii(string value, int length)
        {
            WriteBytes(Encoding.ASCII.GetBytes(value.PadRight(length, '\0')));
        }
    }
}