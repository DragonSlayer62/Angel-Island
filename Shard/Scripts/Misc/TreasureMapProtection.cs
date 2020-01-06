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

using System;
using System.IO;
using System.Collections;
using Server;
using Server.Regions;
using Server.Commands;

namespace Server
{
	public class TreasureRegion : Region
	{
		private const int Range = 5; // No house may be placed within 5 tiles of the treasure

		public TreasureRegion(int x, int y, Map map)
			: base("", "DynRegion", map)
		{
			Priority = Region.TownPriority;
			LoadFromXml = false;

			Coords = new ArrayList();
			Coords.Add(new Rectangle2D(x - Range, y - Range, 1 + (Range * 2), 1 + (Range * 2)));

			GoLocation = new Point3D(x, y, map.GetAverageZ(x, y));
		}

		public static void Initialize()
		{
			string filePath = Path.Combine(Core.BaseDirectory, "Data/treasure.cfg");
			int i = 0, x = 0, y = 0;

			if (File.Exists(filePath))
			{
				using (StreamReader ip = new StreamReader(filePath))
				{
					string line;

					while ((line = ip.ReadLine()) != null)
					{
						i++;

						try
						{
							string[] split = line.Split(' ');

							x = Convert.ToInt32(split[0]);
							y = Convert.ToInt32(split[1]);

							try
							{
								Region.AddRegion(new TreasureRegion(x, y, Map.Felucca));
								// Region.AddRegion( new TreasureRegion( x, y, Map.Trammel ) );
							}
							catch (Exception e)
							{
								LogHelper.LogException(e);
								Console.WriteLine("{0} {1} {2} {3}", i, x, y, e);
							}
						}
						catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
					}
				}
			}
		}

		public override bool AllowHousing(Point3D p)
		{
			return false;
		}

		public override void OnEnter(Mobile m)
		{
			if (m.AccessLevel > AccessLevel.Player)
				m.SendMessage("You have entered a protected treasure map area.");
		}

		public override void OnExit(Mobile m)
		{
			if (m.AccessLevel > AccessLevel.Player)
				m.SendMessage("You have left a protected treasure map area.");
		}
	}
}