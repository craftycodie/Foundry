using UnityEngine;
using System.Collections;
using System;

namespace Foundry.Common.Helpers
{
	public static class TimestampHelper
	{
		public static DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		public static long ToTimestamp(DateTime dateTime)
		{
			return (long)(dateTime - epoch).TotalSeconds;
		}

		public static DateTime ToDateTime(long timestamp)
		{
			return epoch.AddSeconds(timestamp);
		}
	}
}