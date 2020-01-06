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
/* Changelog :
 *	03/25/05, erlein
 *		Integrated with LogHelper class.		
 *	03/23/05, erlein
 *		Moved to /Scripts/Commands/FindItemByName.cs (for Find* command normalization).
 *		Changed namespace to Server.Commands.
 */

using System;
using Server;

namespace Server.Commands
{
	public class FindItemByName
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindItemByName", AccessLevel.Administrator, new CommandEventHandler(FindItemByName_OnCommand));
		}

		[Usage("FindItemByName <name>")]
		[Description("Finds an item by name.")]
		public static void FindItemByName_OnCommand(CommandEventArgs e)
		{
			if (e.Length == 1)
			{
				LogHelper Logger = new LogHelper("FindItemByName.log", e.Mobile, false);

				string name = e.GetString(0).ToLower();

				foreach (Item item in World.Items.Values)
					if (item.Name != null && item.Name.ToLower().IndexOf(name) >= 0)
						Logger.Log(LogType.Item, item);

				Logger.Finish();


			}
			else
			{
				e.Mobile.SendMessage("Format: FindItemByName <name>");
			}
		}
	}
}