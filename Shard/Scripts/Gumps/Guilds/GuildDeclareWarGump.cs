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

/* Scripts/Gumps/Guilds/GuildDeclareWarGump.cs
 * ChangeLog:
 *	12/4/07, Adam
 *		Add support for peaceful guilds (no notoriety)
 *  12/14/05, Kit
 *		Added check to prevent declareing war on a guild your allied with
 */

using System;
using System.Collections;
using Server;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
	public class GuildDeclareWarGump : GuildListGump
	{
		public GuildDeclareWarGump(Mobile from, Guild guild, ArrayList list)
			: base(from, guild, true, list)
		{
		}

		protected override void Design()
		{
			AddHtmlLocalized(20, 10, 400, 35, 1011065, false, false); // Select the guild you wish to declare war on.

			AddButton(20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, 400, 245, 30, 1011068, false, false); // Send the challenge!

			AddButton(300, 400, 4005, 4007, 2, GumpButtonType.Reply, 0);
			AddHtmlLocalized(335, 400, 100, 35, 1011012, false, false); // CANCEL
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (GuildGump.BadLeader(m_Mobile, m_Guild))
				return;

			if (info.ButtonID == 1)
			{
				int[] switches = info.Switches;

				if (switches.Length > 0)
				{
					int index = switches[0];

					if (index >= 0 && index < m_List.Count)
					{
						Guild g = (Guild)m_List[index];

						if (g != null)
						{
							if (g == m_Guild)
							{
								m_Mobile.SendLocalizedMessage(501184); // You cannot declare war against yourself!
							}
							else if (m_Guild.Peaceful == true)
							{
								m_Mobile.SendMessage("You belong to a peaceful guild and may not do this.");
							}
							else if ((g.WarInvitations.Contains(m_Guild) && m_Guild.WarDeclarations.Contains(g)) || m_Guild.IsWar(g))
							{
								m_Mobile.SendLocalizedMessage(501183); // You are already at war with that guild.
							}
							else if ((g.AllyInvitations.Contains(m_Guild) && m_Guild.AllyDeclarations.Contains(g)) || m_Guild.IsAlly(g))
							{
								m_Mobile.SendMessage("You cannot war with a guild you are allied with!."); // cant allie with a guild we are at war with
							}
							else
							{
								if (!m_Guild.WarDeclarations.Contains(g))
								{
									m_Guild.WarDeclarations.Add(g);
									m_Guild.GuildMessage(1018019, "{0} ({1})", g.Name, g.Abbreviation); // Guild Message: Your guild has sent an invitation for war:
								}

								if (!g.WarInvitations.Contains(m_Guild))
								{
									g.WarInvitations.Add(m_Guild);
									g.GuildMessage(1018021, "{0} ({1})", m_Guild.Name, m_Guild.Abbreviation); // Guild Message: Your guild has received an invitation to war:
								}
							}

							GuildGump.EnsureClosed(m_Mobile);
							m_Mobile.SendGump(new GuildWarAdminGump(m_Mobile, m_Guild));
						}
					}
				}
			}
			else if (info.ButtonID == 2)
			{
				GuildGump.EnsureClosed(m_Mobile);
				m_Mobile.SendGump(new GuildmasterGump(m_Mobile, m_Guild));
			}
		}
	}
}