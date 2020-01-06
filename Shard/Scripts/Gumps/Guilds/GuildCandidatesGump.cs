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
using Server.Network;

namespace Server.Gumps
{
	public class GuildCandidatesGump : GuildMobileListGump
	{
		public GuildCandidatesGump(Mobile from, Guild guild)
			: base(from, guild, false, guild.Candidates)
		{
		}

		protected override void Design()
		{
			AddHtmlLocalized(20, 10, 500, 35, 1013030, false, false); // <center> Candidates </center>

			AddButton(20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, 400, 300, 35, 1011120, false, false); // Return to the main menu.
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (GuildGump.BadMember(m_Mobile, m_Guild))
				return;

			if (info.ButtonID == 1)
			{
				GuildGump.EnsureClosed(m_Mobile);
				m_Mobile.SendGump(new GuildGump(m_Mobile, m_Guild));
			}
		}
	}
}