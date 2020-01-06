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

/* Scripts/Gumps/Guilds/JoinNEWGuildGump.cs
 * ChangeLog
 *  1/5/07, Adam
 *      Obsolete while we auto-add new players
 *  12/6/07, Adam
 *      First time check in.
 *      New gump to auto add players to the New Guild (a peaceful guild)
 */

using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Network;
using Server.Guilds;
using Server.Commands;			// log helper

namespace Server.Gumps
{
	/*public class  JoinNEWGuildGump : Gump
	{
		private static Guildstone m_Stone;

		public JoinNEWGuildGump( Mobile from ) : base( 50, 50 )
		{
			AddPage( 0 );

			AddBackground( 10, 10, 190, 140, 0x242C );

			AddHtml(30, 30, 150, 75, String.Format("<div align=CENTER>{0}</div>", "Would you like to join the guild for new players?"), false, false);

			AddButton( 40, 105, 0x81A, 0x81B, 0x1, GumpButtonType.Reply, 0 ); // Okay
			AddButton( 110, 105, 0x819, 0x818, 0x2, GumpButtonType.Reply, 0 ); // Cancel
		}

		public override void OnResponse( NetState state, RelayInfo info )
		{
			Mobile from = state.Mobile;

			if (info.ButtonID == 1)
			{
				Guildstone stone = FindGuild("new");
				if (stone != null && stone.Guild != null)
				{   // log it
					LogHelper logger = new LogHelper("PlayerAddedToNEWGuild.log", false, true);
					logger.Log(LogType.Mobile, from);
					logger.Finish();
					// do it
					stone.Guild.AddMember(from);
					from.DisplayGuildTitle = true;
					DateTime tx = DateTime.Now.AddDays(14);
					string title = String.Format("{0}/{1}", tx.Day, tx.Month);
					from.GuildTitle = title;
					from.GuildFealty = stone.Guild.Leader;
					stone.Guild.GuildMessage(String.Format("{0} has just joined {1}.", from.Name, stone.Guild.Abbreviation == null ? "your guild" : stone.Guild.Abbreviation));
				}
				else
					from.SendMessage("We're sorry, but the new player guild is temporarily unavailable.");
			}
		}

		private Guildstone FindGuild(string abv)
		{
			// cache the stone
			if (m_Stone != null && m_Stone.Deleted == false)
				return m_Stone;

			Guild guild = null;
			string name = abv.ToLower();
			foreach (Item n in World.Items.Values)
			{
				if (n is Guildstone && n != null)
				{
					if (((Guildstone)n).Guild != null)
						guild = ((Guildstone)n).Guild;

					if (guild.Abbreviation != null && guild.Peaceful == true && guild.Abbreviation.ToLower() == name)
					{   // cache the guildstone
						m_Stone = (Guildstone)guild.Guildstone;
						return m_Stone;
					}
				}
			}

			return null;
		}
	}
	*/
}