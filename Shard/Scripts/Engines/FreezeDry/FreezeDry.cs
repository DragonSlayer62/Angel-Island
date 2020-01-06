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

/* Scripts\Commands\FreezeDry.cs
 * Changelog
 *  6/7/07, Adam
 *      Add FDStats command to dump the eligible vs FD'ed containers
 *	12/14/05 Taran Kain
 *		Initial version.
 */

using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.Commands
{
	/// <summary>
	/// Summary description for FreezeDry.
	/// </summary>
	public class FreezeDry
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FDStats", AccessLevel.Administrator, new CommandEventHandler(On_FDStats));
			Server.CommandSystem.Register("StartFDTimers", AccessLevel.Administrator, new CommandEventHandler(On_StartFDTimers));
			Server.CommandSystem.Register("RehydrateAll", AccessLevel.Administrator, new CommandEventHandler(On_RehydrateAll));
		}

		public static void On_FDStats(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Display FreezeDry status for all eligible containers...");

			DateTime start = DateTime.Now;
			int containers = 0;
			int eligible = 0;
			int freezeDried = 0;
			int scheduled = 0;
			int orphans = 0;
			foreach (Item i in World.Items.Values)
			{
				if (i as Container != null)
				{
					Container cx = i as Container;
					containers++;
					if (cx.CanFreezeDry == true)
					{
						eligible++;
						if (cx.IsFreezeDried == true)
							freezeDried++;
					}

					if (cx.IsFreezeScheduled == true)
						scheduled++;

					if (cx.CanFreezeDry == true && cx.IsFreezeDried == false && cx.IsFreezeScheduled == false)
						orphans++;
				}
			}

			e.Mobile.SendMessage("Out of {0} eligible containers, {1} are freeze dried, {2} scheduled, and {3} orphans.", eligible, freezeDried, scheduled, orphans);
			DateTime end = DateTime.Now;
			e.Mobile.SendMessage("Finished in {0}ms.", (end - start).TotalMilliseconds);
		}

		public static void On_StartFDTimers(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Starting FreezeTimers for all eligible containers...");

			DateTime start = DateTime.Now;
			foreach (Item i in World.Items.Values)
				i.OnRehydrate();

			DateTime end = DateTime.Now;
			e.Mobile.SendMessage("Finished in {0}ms.", (end - start).TotalMilliseconds);
		}

		public static void On_RehydrateAll(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Rehydrating all FreezeDried containers...");

			int count = 0;

			DateTime start = DateTime.Now;
			ArrayList al = new ArrayList(World.Items.Values);
			foreach (Item i in al)
			{
				if (i.IsFreezeDried)
				{
					i.Rehydrate();
					count++;
				}
			}

			DateTime end = DateTime.Now;
			e.Mobile.SendMessage("{0} containers rehydrated in {1} seconds.", count, (end - start).TotalSeconds);
			e.Mobile.SendMessage("Rehydrate() averaged {0}ms per call.", (end - start).TotalMilliseconds / count);
		}
	}
}
