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

/* Scripts/Commands/Listen.cs
 * CHANGELOG:
 *	7/11/05 - Pix
 *		Initial Version
 */

using System;
using Server;
using Server.Targeting;

namespace Server.Commands
{
	/// <summary>
	/// Summary description for Listen.
	/// </summary>
	public class Listen
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Listen", AccessLevel.GameMaster, new CommandEventHandler(Listen_OnCommand));
		}

		public static void Listen_OnCommand(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(Listen_OnTarget));
			e.Mobile.SendMessage("Target a player.");
		}

		public static void Listen_OnTarget(Mobile from, object obj)
		{
			if (obj is Mobile)
			{
				Server.Engines.PartySystem.Party.ListenToParty_OnTarget(from, obj);
				Server.Guilds.Guild.ListenToGuild_OnTarget(from, obj);
			}
		}

	}
}
