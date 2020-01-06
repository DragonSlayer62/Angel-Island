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
	public class GuildTitlePrompt : Prompt
	{
		private Mobile m_Leader, m_Target;
		private Guild m_Guild;

		public GuildTitlePrompt(Mobile leader, Mobile target, Guild g)
		{
			m_Leader = leader;
			m_Target = target;
			m_Guild = g;
		}

		public override void OnCancel(Mobile from)
		{
			if (GuildGump.BadLeader(m_Leader, m_Guild))
				return;
			else if (m_Target.Deleted || !m_Guild.IsMember(m_Target))
				return;

			GuildGump.EnsureClosed(m_Leader);
			m_Leader.SendGump(new GuildmasterGump(m_Leader, m_Guild));
		}

		public override void OnResponse(Mobile from, string text)
		{
			if (GuildGump.BadLeader(m_Leader, m_Guild))
				return;
			else if (m_Target.Deleted || !m_Guild.IsMember(m_Target))
				return;

			text = text.Trim();

			if (text.Length > 20)
				text = text.Substring(0, 20);

			if (text.Length > 0)
				m_Target.GuildTitle = text;

			GuildGump.EnsureClosed(m_Leader);
			m_Leader.SendGump(new GuildmasterGump(m_Leader, m_Guild));
		}
	}
}