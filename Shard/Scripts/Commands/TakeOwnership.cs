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

/* Scripts\Commands\TakeOwnership.cs
 * ChangeLog
 *  06/12/07, Adam
 *      First time checkin
 *      Takes ownership oif a house.
 *      Could be extended to take ownership of a boat as well.
 */

using System;
using System.Reflection;
using Server.Items;
using Server.Targeting;
using Server.Multis;        // HouseSign

namespace Server.Commands
{
	public class TakeOwnership
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("TakeOwnership", AccessLevel.GameMaster, new CommandEventHandler(TakeOwnership_OnCommand));
		}

		[Usage("TakeOwnership")]
		[Description("take ownership of a house.")]
		private static void TakeOwnership_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new TakeOwnershipTarget();
			e.Mobile.SendMessage("What do you wish to take ownership of?");
		}

		private class TakeOwnershipTarget : Target
		{
			public TakeOwnershipTarget()
				: base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object target)
			{
				if (target is HouseSign && (target as HouseSign).Structure != null)
				{
					try
					{
						BaseHouse bh = (target as HouseSign).Structure as BaseHouse;
						bh.AdminTransfer(from);
					}
					catch (Exception tse)
					{
						LogHelper.LogException(tse);
					}
				}
				else
				{
					from.SendMessage("That is not a house sign.");
				}
			}
		}

	}
}
