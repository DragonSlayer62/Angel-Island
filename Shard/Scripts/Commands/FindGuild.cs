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

/* Scripts/Commands/FindGuild.cs
 * Changelog
 *	06/14/06, Adam
 *		Add the account name to the display
 *	05/17/06, Kit
 *		Initial creation.
 */
using System;
using Server;
using System.Collections;
using Server.Items;
using Server.Mobiles;
using Server.Guilds;

namespace Server.Commands
{
	public class FindGuild
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindGuild", AccessLevel.GameMaster, new CommandEventHandler(FindGuild_OnCommand));
		}

		[Usage("FindGuild <abbrevation>")]
		[Description("Finds a guild by abbrevation.")]
		public static void FindGuild_OnCommand(CommandEventArgs e)
		{
			Guild temp = null;
			PlayerMobile ptemp = null;

			if (e.Length == 1)
			{
				string name = e.GetString(0).ToLower();

				foreach (Item n in World.Items.Values)
				{
					if (n is Guildstone && n != null)
					{
						if (((Guildstone)n).Guild != null)
							temp = ((Guildstone)n).Guild;

						if (temp.Abbreviation.ToLower() == name)
						{
							if (n.Parent != null && n.Parent is PlayerMobile)
							{
								ptemp = (PlayerMobile)n.Parent;
								e.Mobile.SendMessage("Guild Stone Found on Mobile {2}:{0}, {1}", ptemp.Name, ptemp.Location, ptemp.Account);
							}
							else
							{
								e.Mobile.SendMessage("Guild Stone {1} Found at: {0} ({2})", n.Location, n.Serial, n.Map);
							}
							return;
						}
					}
				}
				e.Mobile.SendMessage("Guild Stone not found in world");
			}
			else
			{
				e.Mobile.SendMessage("Format: FindGuild <abbreviation>");
			}
		}
	}
}
