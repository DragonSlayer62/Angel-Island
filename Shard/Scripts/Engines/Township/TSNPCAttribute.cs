using System;
using System.Collections.Generic;
using System.Text;

using Server.Multis;

namespace Server
{
	[AttributeUsage(AttributeTargets.Class)]
	public class TownshipNPCAttribute : Attribute
	{
		public TownshipNPCAttribute()
		{
		}
	}
}
