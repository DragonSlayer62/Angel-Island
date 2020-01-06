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

/* Scripts/Gumps/Guilds/GuildDeclareWarPrompt.cs
 * Changelog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Guilds;
using Server.Prompts;

namespace Server.Gumps
{
	public class GuildDeclareWarPrompt : Prompt
	{
		private Mobile m_Mobile;
		private Guild m_Guild;

		public GuildDeclareWarPrompt(Mobile m, Guild g)
		{
			m_Mobile = m;
			m_Guild = g;
		}

		public override void OnCancel(Mobile from)
		{
			if (GuildGump.BadLeader(m_Mobile, m_Guild))
				return;

			GuildGump.EnsureClosed(m_Mobile);
			m_Mobile.SendGump(new GuildWarAdminGump(m_Mobile, m_Guild));
		}

		public override void OnResponse(Mobile from, string text)
		{
			if (GuildGump.BadLeader(m_Mobile, m_Guild))
				return;

			text = text.Trim();

			if (text.Length >= 3)
			{
				BaseGuild[] guilds = Guild.Search(text);

				GuildGump.EnsureClosed(m_Mobile);

				if (guilds.Length > 0)
				{
					m_Mobile.SendGump(new GuildDeclareWarGump(m_Mobile, m_Guild, new ArrayList(guilds)));
				}
				else
				{
					m_Mobile.SendGump(new GuildWarAdminGump(m_Mobile, m_Guild));
					m_Mobile.SendLocalizedMessage(1018003); // No guilds found matching - try another name in the search
				}
			}
			else
			{
				m_Mobile.SendMessage("Search string must be at least three letters in length.");
			}
		}
	}
}