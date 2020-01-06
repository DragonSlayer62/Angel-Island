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

/* Scripts/Gumps/Guilds/MoveGuildstoneGump.cs
 * ChangeLog
 *	3/10/05, mith
 *		Script Created. Called from Guildstone.OnDoubleClick()
 */

using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Network;

namespace Server.Gumps
{
	public class MoveGuildstoneGump : Gump
	{
		private Guildstone m_Stone;

		public MoveGuildstoneGump(Mobile from, Guildstone stone)
			: base(50, 50)
		{
			m_Stone = stone;

			AddPage(0);

			AddBackground(10, 10, 190, 140, 0x242C);

			AddHtml(30, 30, 150, 75, String.Format("<div align=CENTER>{0}</div>", "Are you sure you want to re-deed this guildstone?"), false, false);

			AddButton(40, 105, 0x81A, 0x81B, 0x1, GumpButtonType.Reply, 0); // Okay
			AddButton(110, 105, 0x819, 0x818, 0x2, GumpButtonType.Reply, 0); // Cancel
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (m_Stone.Deleted)
				return;

			Mobile from = state.Mobile;

			if (info.ButtonID == 1)
				m_Stone.OnPrepareMove(from);
		}
	}
}