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

/* Scripts/Commands/PlayerQuest.cs
 * ChangeLog:
 *  8/11/07, Adam
 *      Protect against targeting a backpack
 *      Add assert to alert staff if the player is missing a backpack.
 *	8/11/07, Pixie
 *		Safeguarded PlayerQuestTarget.OnTarget.
 *  9/08/06, Adam
 *		Created.
 */

using System;
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Scripts.Gumps;
using Server.Multis;

namespace Server.Commands
{
	public class PlayerQuest
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			Server.CommandSystem.Register("Quest", AccessLevel.Player, new CommandEventHandler(PlayerQuest_OnCommand));
		}

		private class PlayerQuestTarget : Target
		{
			public PlayerQuestTarget()
				: base(-1, true, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object o)
			{
				try
				{
					if (from == null)
					{
						return;
					}

					if (o == null)
					{
						from.SendMessage("Target does not exist.");
						return;
					}


					if (o is BaseContainer == false)
					{
						from.SendMessage("That is not a container.");
						return;
					}

					BaseContainer bc = o as BaseContainer;

					if (Misc.Diagnostics.Assert(from.Backpack != null, "from.Backpack == null") == false)
					{
						from.SendMessage("You cannot use this deed without a backpack.");
						return;
					}

					// mobile backpacks may not be used
					if (bc == from.Backpack || bc.Parent is Mobile)
					{
						from.SendMessage("You may not use that container.");
						return;
					}

					// must not be locked down
					if (bc.IsLockedDown == true || bc.IsSecure == true)
					{
						from.SendMessage("That container is locked down.");
						return;
					}

					// if it's in your bankbox, or it's in YOUR house, you can deed it
					if ((bc.IsChildOf(from.BankBox) || CheckAccess(from)) == false)
					{
						from.SendMessage("The container must be in your bankbox, or a home you own.");
						return;
					}

					// cannot be in another container
					if (bc.RootParent is BaseContainer && bc.IsChildOf(from.BankBox) == false)
					{
						from.SendMessage("You must remove it from that container first.");
						return;
					}

					// okay, done with target checking, now deed the container.
					// place a special deed to reclaim the container in the players backpack
					PlayerQuestDeed deed = new PlayerQuestDeed(bc);
					if (from.Backpack.CheckHold(from, deed, true, false, 0, 0))
					{
						bc.PlayerQuest = true;							// mark as special
						bc.MoveToWorld(from.Location, Map.Internal);	// put it on the internal map
						bc.SetLastMoved();								// record the move (will use this in Heartbeat cleanup)
						//while (deed.Expires.Hours + deed.Expires.Minutes == 0)
						//Console.WriteLine("Waiting...");
						//int hours = deed.Expires.Hours;
						//int minutes = deed.Expires.Minutes;
						//string text = String.Format("{0} {1}, and {2} {3}",	hours, hours == 1 ? "hour" : "hours", minutes, minutes == 1 ? "minute" : "minutes");
						from.Backpack.DropItem(deed);
						from.SendMessage("A deed for the container has been placed in your backpack.");
						//from.SendMessage( "This quest will expire in {0}.", text);
					}
					else
					{
						from.SendMessage("Your backpack is full and connot hold the deed.");
						deed.Delete();
					}
				}
				catch (Exception e)
				{
					LogHelper.LogException(e);
				}
			}

			public bool CheckAccess(Mobile m)
			{	// Allow access if the player is owner of the house.
				BaseHouse house = BaseHouse.FindHouseAt(m);
				return (house != null && house.IsOwner(m));
			}
		}

		[Usage("PlayerQuest")]
		[Description("Allows a player to convert a container of items into a quest ticket.")]
		private static void PlayerQuest_OnCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			if (from.Alive == false)
			{
				e.Mobile.SendMessage("You are dead and cannot do that.");
				return;
			}

			from.SendMessage("Please target the container you would like to deed.");
			from.Target = new PlayerQuestTarget();
		}
	}
}