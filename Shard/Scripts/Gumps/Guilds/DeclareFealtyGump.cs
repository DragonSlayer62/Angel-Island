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

/* Scripts/Gumps/Guilds/DeclareFealtyGump.cs
 * CHANGELOG:
 *  01/14/08 Pix
 *      Now anyone declared to you changes votes when you change yours.
 */

using System;
using Server;
using Server.Guilds;
using Server.Network;

namespace Server.Gumps
{
	public class DeclareFealtyGump : GuildMobileListGump
	{
		public DeclareFealtyGump(Mobile from, Guild guild)
			: base(from, guild, true, guild.Members)
		{
		}

		protected override void Design()
		{
			AddHtmlLocalized(20, 10, 400, 35, 1011097, false, false); // Declare your fealty

			AddButton(20, 400, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, 400, 250, 35, 1011098, false, false); // I have selected my new lord.

			AddButton(300, 400, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(335, 400, 100, 35, 1011012, false, false); // CANCEL
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (GuildGump.BadMember(m_Mobile, m_Guild))
				return;

			if (info.ButtonID == 1)
			{
				int[] switches = info.Switches;

				if (switches.Length > 0)
				{
					int index = switches[0];

					if (index >= 0 && index < m_List.Count)
					{
						Mobile m = (Mobile)m_List[index];

						if (m != null && !m.Deleted)
						{
							state.Mobile.GuildFealty = m;

							m_Guild.UpdateFealtiesFor(state.Mobile, m);
						}
					}
				}
			}

			GuildGump.EnsureClosed(m_Mobile);
			m_Mobile.SendGump(new GuildGump(m_Mobile, m_Guild));
		}
	}
}
