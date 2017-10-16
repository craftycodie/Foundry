using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Foundry.Common.Helpers
{
	public static class QuaternionHelper
	{
		public static Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
		{
			float num9 = roll * 0.5f;
			float num6 = (float)Math.Sin(num9);
			float num5 = (float)Math.Cos(num9);
			float num8 = pitch * 0.5f;
			float num4 = (float)Math.Sin(num8);
			float num3 = (float)Math.Cos(num8);
			float num7 = yaw * 0.5f;
			float num2 = (float)Math.Sin(num7);
			float num = (float)Math.Cos(num7);
			return new Quaternion(
			((num * num4) * num5) + ((num2 * num3) * num6),
			((num2 * num3) * num5) - ((num * num4) * num6),
			((num * num3) * num6) - ((num2 * num4) * num5),
			((num * num3) * num5) + ((num2 * num4) * num6));
		}

		//public static float[] GetYawPitchRoll(Quaternion rotation)
		//{
		//	float num9, num8, num7, num6, num5, num4, num3, num2, num;
		//	rotation.

		//}

		public static Quaternion FromRightUpVectors(Vector3 rightVector, Vector3 upVector)
		{
			float yaw, roll, pitch;
			Vector3 forwardVector = new Vector3();
			forwardVector = Vector3.Cross(rightVector, upVector);
			pitch = (float)Math.Atan2(-upVector.x, Math.Sqrt(upVector.y * upVector.y + upVector.z * upVector.z));

			try
			{
				roll = (float)Math.Atan2(upVector.y, upVector.z);
				yaw = (float)Math.Atan2(forwardVector.x, rightVector.x);
			}
			catch (DivideByZeroException)
			{
				// gimbal lock :O
				roll = (float)-Math.Atan2(forwardVector.z, forwardVector.y);
				yaw = 0;
			}

			return CreateFromYawPitchRoll(yaw, roll, pitch) * Quaternion.Euler(Vector3.right * 270);
		}

		public static Quaternion SwapYZ(Quaternion rotation)
		{
			return Quaternion.Euler(new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.z, rotation.eulerAngles.y));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rotation"></param>
		/// <returns>Returns the two vectors. Right at [0], Up at [1]</returns>
		//public Vector3[] ToRightUpVectors(Quaternion rotation)
		//{

		//}
	}
}
