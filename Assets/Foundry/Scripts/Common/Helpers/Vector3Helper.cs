using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Foundry.Common.Helpers
{
	public static class Vector3Helper
	{
		public static Vector3 SwapYZ(Vector3 vector)
		{
			return new Vector3(vector.x, vector.z, vector.y);
		}
	}
}
