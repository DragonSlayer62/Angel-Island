/*
 *	This program is the CONFIDENTIAL and PROPRIETARY property 
 *	of Tomasello Software LLC. Any unauthorized use, reproduction or
 *	transfer of this computer program is strictly prohibited.
 *
 *      Copyright (c) 2004 Tomasello Software LLC.
 *	This is an unpublished work, and is subject to limited distribution and
 *	restricted disclosure only. ALL RIGHTS RESERVED.
 *
 *			RESTRICTED RIGHTS LEGEND
 *	Use, duplication, or disclosure by the Government is subject to
 *	restrictions set forth in subparagraph (c)(1)(ii) of the Rights in
 * 	Technical Data and Computer Software clause at DFARS 252.227-7013.
 *
 *	Angel Island UO Shard	Version 1.0
 *			Release A
 *			March 25, 2004
 */

/* Scripts/Items/Skill Items/Fishing/Misc/Sextant.cs
 * CHANGELOG:
 *	02/11/06, Adam
 *		Make common the formatting of sextant coords.
 */

using System;
using Server.Network;

namespace Server.Items
{
	public class Sextant : Item
	{
		[Constructable]
		public Sextant()
			: base(0x1058)
		{
			Weight = 2.0;
		}

		public Sextant(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			int xLong = 0, yLat = 0;
			int xMins = 0, yMins = 0;
			bool xEast = false, ySouth = false;

			if (Sextant.Format(from.Location, from.Map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth))
			{
				string location = Format(xLong, yLat, xMins, yMins, xEast, ySouth);
				from.LocalOverheadMessage(MessageType.Regular, from.SpeechHue, false, location);
			}
		}

		public static bool ComputeMapDetails(Map map, int x, int y, out int xCenter, out int yCenter, out int xWidth, out int yHeight)
		{
			xWidth = 5120; yHeight = 4096;

			if (map == Map.Trammel || map == Map.Felucca)
			{
				if (x >= 0 && y >= 0 && x < 5120 && y < 4096)
				{
					xCenter = 1323; yCenter = 1624;
				}
				else if (x >= 5120 && y >= 2304 && x < 6144 && y < 4096)
				{
					xCenter = 5936; yCenter = 3112;
				}
				else
				{
					xCenter = 0; yCenter = 0;
					return false;
				}
			}
			else if (x >= 0 && y >= 0 && x < map.Width && y < map.Height)
			{
				xCenter = 1323; yCenter = 1624;
			}
			else
			{
				xCenter = 0; yCenter = 0;
				return false;
			}

			return true;
		}

		public static Point3D ReverseLookup(Map map, int xLong, int yLat, int xMins, int yMins, bool xEast, bool ySouth)
		{
			if (map == null || map == Map.Internal)
				return Point3D.Zero;

			int xCenter, yCenter;
			int xWidth, yHeight;

			if (!ComputeMapDetails(map, 0, 0, out xCenter, out yCenter, out xWidth, out yHeight))
				return Point3D.Zero;

			double absLong = xLong + ((double)xMins / 60);
			double absLat = yLat + ((double)yMins / 60);

			if (!xEast)
				absLong = 360.0 - absLong;

			if (!ySouth)
				absLat = 360.0 - absLat;

			int x, y, z;

			x = xCenter + (int)((absLong * xWidth) / 360);
			y = yCenter + (int)((absLat * yHeight) / 360);

			if (x < 0)
				x += xWidth;
			else if (x >= xWidth)
				x -= xWidth;

			if (y < 0)
				y += yHeight;
			else if (y >= yHeight)
				y -= yHeight;

			z = map.GetAverageZ(x, y);

			return new Point3D(x, y, z);
		}

		public static string Format(int xLong, int yLat, int xMins, int yMins, bool xEast, bool ySouth)
		{
			return String.Format("{0}� {1}'{2}, {3}� {4}'{5}", yLat, yMins, ySouth ? "S" : "N", xLong, xMins, xEast ? "E" : "W");
		}

		public static bool Format(Point3D p, Map map, ref int xLong, ref int yLat, ref int xMins, ref int yMins, ref bool xEast, ref bool ySouth)
		{
			if (map == null || map == Map.Internal)
				return false;

			int x = p.X, y = p.Y;
			int xCenter, yCenter;
			int xWidth, yHeight;

			if (!ComputeMapDetails(map, x, y, out xCenter, out yCenter, out xWidth, out yHeight))
				return false;

			double absLong = (double)((x - xCenter) * 360) / xWidth;
			double absLat = (double)((y - yCenter) * 360) / yHeight;

			if (absLong > 180.0)
				absLong = -180.0 + (absLong % 180.0);

			if (absLat > 180.0)
				absLat = -180.0 + (absLat % 180.0);

			bool east = (absLong >= 0), south = (absLat >= 0);

			if (absLong < 0.0)
				absLong = -absLong;

			if (absLat < 0.0)
				absLat = -absLat;

			xLong = (int)absLong;
			yLat = (int)absLat;

			xMins = (int)((absLong % 1.0) * 60);
			yMins = (int)((absLat % 1.0) * 60);

			xEast = east;
			ySouth = south;

			return true;
		}
	}
}