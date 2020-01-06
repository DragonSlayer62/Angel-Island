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
using System.Collections;
using System.IO;
using Server;
using Server.Items;

namespace Server.Commands
{
	public class ExportCommand
	{
		private const string ExportFile = @"C:\Uo\WorldForge\items.wsc";

		public static void Initialize()
		{
			Server.CommandSystem.Register("ExportWSC", AccessLevel.Administrator, new CommandEventHandler(Export_OnCommand));
		}

		public static void Export_OnCommand(CommandEventArgs e)
		{
			StreamWriter w = new StreamWriter(ExportFile);
			ArrayList remove = new ArrayList();
			int count = 0;

			e.Mobile.SendMessage("Exporting all static items to \"{0}\"...", ExportFile);
			e.Mobile.SendMessage("This will delete all static items in the world.  Please make a backup.");

			foreach (Item item in World.Items.Values)
			{
				if ((item is Static || item is BaseFloor || item is BaseWall)
					&& item.RootParent == null)
				{
					w.WriteLine("SECTION WORLDITEM {0}", count);
					w.WriteLine("{");
					w.WriteLine("SERIAL {0}", item.Serial);
					w.WriteLine("NAME #");
					w.WriteLine("NAME2 #");
					w.WriteLine("ID {0}", item.ItemID);
					w.WriteLine("X {0}", item.X);
					w.WriteLine("Y {0}", item.Y);
					w.WriteLine("Z {0}", item.Z);
					w.WriteLine("COLOR {0}", item.Hue);
					w.WriteLine("CONT -1");
					w.WriteLine("TYPE 0");
					w.WriteLine("AMOUNT 1");
					w.WriteLine("WEIGHT 255");
					w.WriteLine("OWNER -1");
					w.WriteLine("SPAWN -1");
					w.WriteLine("VALUE 1");
					w.WriteLine("}");
					w.WriteLine("");

					count++;
					remove.Add(item);
					w.Flush();
				}
			}

			w.Close();

			foreach (Item item in remove)
				item.Delete();

			e.Mobile.SendMessage("Export complete.  Exported {0} statics.", count);
		}
	}
}
/*SECTION WORLDITEM 1
{
SERIAL 1073741830
NAME #
NAME2 #
ID 1709
X 1439
Y 1613
Z 20
CONT -1
TYPE 12
AMOUNT 1
WEIGHT 25500
OWNER -1
SPAWN -1
VALUE 1
}*/
