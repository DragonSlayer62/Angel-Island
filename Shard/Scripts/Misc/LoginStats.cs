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

/* Scripts/Misc/LoginStats.cs
 * CHANGELOG
 *	3/3/11, Adam
 *		if we're on a test center server, send a detailed message to the player telling what flavor of test center they are on
 *	11/17/10, Adam
 *		Add back stats printing on login for UOSP if you are staff
 *	11/11/10, pix
 *      Removed stats printing on login for UOSP.
 *	7/11/10, adam
 *		replace local copy of memory system with the common implementation in Utility.Memory
 *	6/21/10, adam
 *		remove item and mobile counts from stats delivered to accesslevel.player
 *	6/15/10, Adam
 *		Turn on online counts on login and display number of adventurers and pvpers currently active.
 *	6/10/10, Adam
 *		Turn off online counts on login.
 *  09/14/05 Taran Kain
 *		Notify of TC functionality if enabled.
 */

using System;
using System.Collections;
using Server.Network;
using Server.Regions;

namespace Server.Misc
{
	public class LoginStats
	{
		public static void Initialize()
		{
			// Register our event handler
			EventSink.Login += new LoginEventHandler(EventSink_Login);
		}

		private static void EventSink_Login(LoginEventArgs args)
		{
			Mobile m = args.Mobile;

			if (Core.UOTC && m.AccessLevel == AccessLevel.Player)
			{	// if we're on a test center server, send a detailed message to the player telling what flavor of test center they are on
				//m.SendMessage(String.Format("Welcome to {0} Test Center{1}", Core.Server, Core.UOEV ? " Event Shard." : "."));
				m.SendMessage(String.Format("Welcome to {0}{1}{2}.", Core.Server, Core.UOTC ? " Test Center" : "", Core.UOEV ? " Event Shard" : ""));
			}
			else if (Core.UOSP && m.AccessLevel == AccessLevel.Player)
			{	// regular player on siege
				m.SendMessage("Welcome, {0}!", args.Mobile.Name);
			}
			else
			{	// regular player on ai or mort
				m.SendMessage("Welcome, {0}! {1}", args.Mobile.Name, Server.Misc.Stats.Format(m.AccessLevel));
			}

			if (TestCenter.Enabled)
				m.SendMessage("Test Center functionality is enabled. You are able to customize your character's stats and skills at anytime to anything you wish.  To see the commands to do this just say 'help'.");
		}

	}

	public static class Stats
	{
		private static Memory m_AggressiveActionMemory = new Memory();

		public static void Initialize()
		{
			EventSink.AggressiveAction += new AggressiveActionEventHandler(EventSink_AggressiveAction);
		}

		public static string Format(AccessLevel ax)
		{
			int userCount = NetState.Instances.Count;
			int itemCount = World.Items.Count;
			int mobileCount = World.Mobiles.Count;
			string sx;

			sx = string.Format("There {0} currently {1} user{2} online",
				userCount == 1 ? "is" : "are",
				userCount, userCount == 1 ? "" : "s");

			if (ax > AccessLevel.Player)
			{
				sx += string.Format(", with {0} item{1} and {2} mobile{3} in the world.",
					itemCount, itemCount == 1 ? "" : "s",
					mobileCount, mobileCount == 1 ? "" : "s");
			}
			else
				sx += ".";

			int pvpers = m_AggressiveActionMemory.Count;
			int adventurers = CountAdventurers();

			if (pvpers > 1)
				sx += string.Format(" {0} {1} to be involved in player conflict", pvpers, pvpers == 1 ? "appears" : "appear");

			if (adventurers > 1)
			{
				if (pvpers > 1)
					sx += ", ";
				sx += string.Format(" {0} {1} to be out adventuring", adventurers, adventurers == 1 ? "appears" : "appear");
			}

			if (pvpers > 1 || adventurers > 1)
				sx += ".";

			return sx;
		}

		private static int CountAdventurers()
		{
			int count = 0;
			foreach (NetState state in NetState.Instances)
			{
				if (state != null && state.Mobile != null)
				{	// sanity ok!
					if (state.Mobile.Map == Map.Felucca)
					{	// we're in felucca

						// don't count players in our PvP list
						if (m_AggressiveActionMemory.Recall(state.Mobile) == true)
							continue;

						if (state.Mobile.Region != null)
						{	// looks to be a valid region
							bool guarded = state.Mobile.Region as GuardedRegion != null && (state.Mobile.Region as GuardedRegion).IsGuarded == true;
							bool house = state.Mobile.Region as HouseRegion != null;
							if (!guarded && !house)
							{	// not guarded and not house
								count++;
							}
						}
						else
						{	// not sure if we ever have a null region
							count++;
						}
					}
				}
			}

			return count;
		}


		private static void EventSink_AggressiveAction(AggressiveActionEventArgs e)
		{
			Mobile aggressor = e.Aggressor;
			Mobile aggressed = e.Aggressed;

			if (!aggressor.Player || !aggressed.Player)
				return;

			// remember for 10 minutes
			m_AggressiveActionMemory.Remember(aggressor, 60 * 10);
			m_AggressiveActionMemory.Remember(aggressed, 60 * 10);
		}
	}
}