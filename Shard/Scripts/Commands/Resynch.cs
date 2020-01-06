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

/* Scripts/Commands/Resynch.cs
 * ChangeLog
 *	9/25/04 - Pix.
 *		Added 2 minute time period between uses of the command.
 *	9/16/04 - Pixie
 *		Resurrected and re-structured this command.
 *		Attempting to see if sending a MobileUpdate packet works.
 */


using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
	public class ResynchCommand
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Resynch", AccessLevel.Player, new CommandEventHandler(Resynch_OnCommand));
		}

		public static void Resynch_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			if (m is PlayerMobile)
			{
				if (((PlayerMobile)m).m_LastResynchTime < (DateTime.Now - TimeSpan.FromMinutes(2.0))
					|| (m.AccessLevel > AccessLevel.Player))
				{
					m.SendMessage("Resynchronizing server and client.");
					m.Send(new MobileUpdate(m));
					((PlayerMobile)m).m_LastResynchTime = DateTime.Now;
				}
				else
				{
					m.SendMessage("You must wait to use that command.");
				}
			}

		}

	}
}
