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

/* Scripts/Commands/Time.cs
 * ChangeLog
 *	3/25/08 - Pix
 *		Changed to use new AdjustedDateTime utility class.
 *	12/06/05 - Pigpen
 *		Created.
 *		Time command works as follows. '[time' Displays Date then time.
 *	3/10/07 - Cyrun
 *		Edited message displayed to include "PST".
 */


using System;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
	public class TimeCommand
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Time", AccessLevel.Player, new CommandEventHandler(Time_OnCommand));
		}

		public static void Time_OnCommand(CommandEventArgs e)
		{
			Mobile m = e.Mobile;

			if (m is PlayerMobile)
			{
				//m.SendMessage("Server time is: {0} PST.", DateTime.Now);

				AdjustedDateTime ddt = new AdjustedDateTime(DateTime.Now);
				m.SendMessage("Server time is: {0} {1}.", ddt.Value.ToShortTimeString(), ddt.TZName);
			}
		}

	}
}
