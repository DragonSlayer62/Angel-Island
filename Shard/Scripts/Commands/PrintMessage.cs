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

/* Scripts/Commands/PrintMessage.cs
 * ChangeLog
 *	3/17/05, Adam
 *		First time checkin.
 */
using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using System.IO;

namespace Server.Commands
{
	public class PrintMessage
	{

		public static void Initialize()
		{
			Server.CommandSystem.Register("PrintMessage", AccessLevel.GameMaster, new CommandEventHandler(PrintMessage_OnCommand));
		}

		[Usage("PrintMessage <msg_number>")]
		[Description("Print the localized message associated with msg_number.")]
		private static void PrintMessage_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;

			if (arg.Length <= 0)
			{
				from.SendMessage("Usage: PrintMessage <msg_number>");
				return;
			}

			// What message do we print
			int message = arg.GetInt32(0);
			from.SendLocalizedMessage(message);
			from.SendMessage("Done.");
		}

	}

}
