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

/* Scripts/Commands/CheckLOS.cs
 * ChangeLog
 *	4/28/08, Adam
 *		First time checkin
 */


using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Misc;						// TestCenter

namespace Server.Commands
{
	public class CheckLOSCommand
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("CheckLOS", AccessLevel.Player, new CommandEventHandler(CheckLOS_OnCommand));
		}

		public static void CheckLOS_OnCommand(CommandEventArgs e)
		{
			if (e.Mobile.AccessLevel == AccessLevel.Player && TestCenter.Enabled == false)
			{	// Players can only test this on Test Center
				e.Mobile.SendMessage("Not available here.");
				return;
			}

			if (e.Mobile.AccessLevel > AccessLevel.Player)
			{	// you will not get good results if you test this with AccessLevel > Player
				e.Mobile.SendMessage("You should test this with AccessLevel.Player.");
				return;
			}

			try
			{
				e.Mobile.Target = new LOSTarget();
				e.Mobile.SendMessage("Check LOS to which object?");
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
			}

		}

		private class LOSTarget : Target
		{
			public LOSTarget()
				: base(15, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targ)
			{
				from.SendMessage("You {0} see that.", from.Map.LineOfSight(from, targ) ? "can" : "cannot");
				return;
			}
		}

	}
}
