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

/* Scripts/Engines/Spawner/TotalRespawnCommand.cs
 * ChangeLog
 *	12/30/04 - Pixie
 *		Changed to Admin only; Added broadcast message; Only spawn Running spawners
 *	12/29/04 - Pixie
 *		Initial Version!
 */


using System;
using System.Collections;
using Server;

namespace Server.Commands
{
	public class TotalRespondCommand
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("TotalRespawn", AccessLevel.Administrator, new CommandEventHandler(TotalRespawn_OnCommand));
		}

		public static void TotalRespawn_OnCommand(CommandEventArgs e)
		{
			DateTime begin = DateTime.Now;

			World.Broadcast(0x35, true, "The world is respawning, please wait.");

			ArrayList spawners = new ArrayList();
			foreach (Item item in World.Items.Values)
			{
				if (item is Server.Mobiles.Spawner)
				{
					spawners.Add(item);
				}
			}

			foreach (Server.Mobiles.Spawner sp in spawners)
			{
				if (sp.Running)
				{
					sp.Respawn();
				}
			}

			DateTime end = DateTime.Now;

			TimeSpan timeTaken = end - begin;
			World.Broadcast(0x35, true, "World spawn complete. The entire process took {0:00.00} seconds.", timeTaken.TotalSeconds);
			e.Mobile.SendMessage("Total Respawn of {0} spawners took {1:00.00} seconds", spawners.Count, timeTaken.TotalSeconds);
		}
	}
}
