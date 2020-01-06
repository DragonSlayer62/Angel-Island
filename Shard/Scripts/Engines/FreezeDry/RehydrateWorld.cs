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

/* Scripts/Commands/RehydrateWorld.cs
 * 	CHANGELOG:
 * 	2/23/06, Adam
 *	Initial Version
 */

using System;
using System.Collections;
using System.IO;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Commands
{
	public class RehydrateWorld
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("RehydrateWorld", AccessLevel.Administrator, new CommandEventHandler(RehydrateWorld_OnCommand));
		}

		[Usage("RehydrateWorld")]
		[Description("Rehydrates the entire world.")]
		public static void RehydrateWorld_OnCommand(CommandEventArgs e)
		{
			// make it known			
			Server.World.Broadcast(0x35, true, "The world is rehydrating, please wait.");
			Console.WriteLine("World: rehydrating...");
			DateTime startTime = DateTime.Now;

			LogHelper Logger = new LogHelper("RehydrateWorld.log", e.Mobile, true);

			// Extract property & value from command parameters
			ArrayList containers = new ArrayList();

			// Loop items and check vs. types
			foreach (Item item in World.Items.Values)
			{
				if (item is Container)
				{
					if ((item as Container).CanFreezeDry == true)
						containers.Add(item);
				}
			}

			Logger.Log(LogType.Text,
				string.Format("{0} containers scheduled for Rehydration...",
					containers.Count));

			int count = 0;
			for (int ix = 0; ix < containers.Count; ix++)
			{
				Container cont = containers[ix] as Container;

				if (cont != null)
				{
					// Rehydrate it if necessary
					if (cont.CanFreezeDry && cont.IsFreezeDried == true)
						cont.Rehydrate();
					count++;
				}
			}

			Logger.Log(LogType.Text,
				string.Format("{0} containers actually Rehydrated", count));

			Logger.Finish();

			e.Mobile.SendMessage("{0} containers actually Rehydrated", count);

			DateTime endTime = DateTime.Now;
			Console.WriteLine("done in {0:F1} seconds.", (endTime - startTime).TotalSeconds);
			Server.World.Broadcast(0x35, true, "World rehydration complete. The entire process took {0:F1} seconds.", (endTime - startTime).TotalSeconds);
		}
	}
}