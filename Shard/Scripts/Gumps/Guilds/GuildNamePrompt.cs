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

using System;
using Server;
using Server.Guilds;
using Server.Prompts;

namespace Server.Gumps
{
	public class GuildNamePrompt : Prompt
	{
		private Mobile m_Mobile;
		private Guild m_Guild;

		public GuildNamePrompt(Mobile m, Guild g)
		{
			m_Mobile = m;
			m_Guild = g;
		}

		public override void OnCancel(Mobile from)
		{
			if (GuildGump.BadLeader(m_Mobile, m_Guild))
				return;

			GuildGump.EnsureClosed(m_Mobile);
			m_Mobile.SendGump(new GuildmasterGump(m_Mobile, m_Guild));
		}

		public override void OnResponse(Mobile from, string text)
		{
			if (GuildGump.BadLeader(m_Mobile, m_Guild))
				return;

			text = text.Trim();

			if (text.Length > 40)
				text = text.Substring(0, 40);

			if (text.Length > 0)
			{
				if (Guild.FindByName(text) != null)
				{
					m_Mobile.SendMessage("{0} conflicts with the name of an existing guild.", text);
				}
				else
				{
					m_Guild.Name = text;
					m_Guild.GuildMessage(1018024, text); // The name of your guild has changed:
				}
			}

			GuildGump.EnsureClosed(m_Mobile);
			m_Mobile.SendGump(new GuildmasterGump(m_Mobile, m_Guild));
		}
	}
}