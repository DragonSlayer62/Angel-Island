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
 *			March 27, 2007
 */

/* Scripts/Engines/RTT/RTTCommand.cs
 * CHANGELOG:
 *  8/26/2007, Pix
 *      Moved command to own class.
 *      Now takes optional 'mode' argument.
 */

using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Targeting;

namespace Server.RTT
{
	class RTTCommand
	{
		#region Command
		public static void Initialize()
		{
			Server.CommandSystem.Register("RTT", AccessLevel.Counselor, new CommandEventHandler(RTT_OnCommand));
		}

		[Usage("RTT")]
		[Description("Does a RTT on yourself, or if you're staff, does an RTT on a target player.")]
		public static void RTT_OnCommand(CommandEventArgs e)
		{
			int mode = 0;
			try
			{
				if (e.Arguments.Length > 0)
				{
					try
					{
						mode = int.Parse(e.Arguments[0]);
					}
					catch //(Exception e1)
					{
						e.Mobile.SendMessage("Error with argument - using default test.");
					}
				}

				if (e.Mobile.AccessLevel > AccessLevel.Player)
				{
					e.Mobile.SendMessage("Target a player to RTT");
					e.Mobile.Target = new RTTTarget(mode);
				}
				else
				{
					if (e.Mobile is PlayerMobile)
					{
						((PlayerMobile)e.Mobile).RTT("Forced AFK check!", true, mode, "Command");
					}
				}
			}
			catch (Exception ex)
			{
				Server.Commands.LogHelper.LogException(ex);
			}
		}

		private class RTTTarget : Target
		{
			private int m_Mode = 0;

			public RTTTarget(int mode)
				: base(11, false, TargetFlags.None)
			{
				m_Mode = mode;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (!(targeted is PlayerMobile))
				{
					from.SendMessage("You can only target players!");
					return;
				}
				else
				{
					((PlayerMobile)targeted).RTT("AFK check!", true, m_Mode, "Command");
				}
			}
		}
		#endregion
	}
}
