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
using Server.Gumps;
using Server.Network;

namespace Server.Engines.VeteranRewards
{
	public class RewardNoticeGump : Gump
	{
		private Mobile m_From;

		public RewardNoticeGump(Mobile from)
			: base(0, 0)
		{
			m_From = from;

			from.CloseGump(typeof(RewardNoticeGump));

			AddPage(0);

			AddBackground(10, 10, 500, 135, 2600);

			/* You have reward items available.
			 * Click 'ok' below to get the selection menu or 'cancel' to be prompted upon your next login.
			 */
			AddHtmlLocalized(52, 35, 420, 55, 1006046, true, true);

			AddButton(60, 95, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(95, 96, 150, 35, 1006044, false, false); // Ok

			AddButton(285, 95, 4017, 4019, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(320, 96, 150, 35, 1006045, false, false); // Cancel
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
				m_From.SendGump(new RewardChoiceGump(m_From));
		}
	}
}