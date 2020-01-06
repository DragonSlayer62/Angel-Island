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

/* Scripts/Engines/Help/PageEnableCommand.cs
 * CHANGELOG:
 *	3/5/05: Pix
 *		Initial Version.
 */

using System;

namespace Server.Commands
{
	public class PageEnableCommand
	{
		public static bool Enabled;

		public static void Initialize()
		{
			PageEnableCommand.Enabled = true;
			Server.CommandSystem.Register("PageEnable", AccessLevel.Administrator, new CommandEventHandler(PageEnable_OnCommand));
		}

		public static void PageEnable_OnCommand(CommandEventArgs e)
		{
			if (e.Arguments.Length > 0)
			{
				if (e.Arguments[0].ToLower() == "on")
				{
					PageEnableCommand.Enabled = true;
				}
				else if (e.Arguments[0].ToLower() == "off")
				{
					PageEnableCommand.Enabled = false;
				}
				else
				{
					e.Mobile.SendMessage("[pageenable takes either 'on' or 'off' as a parameter.");
				}
			}

			e.Mobile.SendMessage("PageEnable is {0}.", PageEnableCommand.Enabled ? "ON" : "OFF");
		}
	}
}
