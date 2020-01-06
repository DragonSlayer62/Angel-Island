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
/* ChangeLog:
 *  10/14/06, Rhiannon
 *		File created
 */


using System;
using Server;
using Server.Guilds;
using Server.Mobiles;

namespace Server.Gumps
{
	public class ConfirmResignGump : Gump
	{
		private Mobile m_From;

		public ConfirmResignGump(Mobile from)
			: base(50, 50)
		{
			m_From = from;

			m_From.CloseGump(typeof(ConfirmResignGump));

			AddPage(0);

			AddBackground(0, 0, 215, 110, 5054);
			AddBackground(10, 10, 195, 90, 3000);

			AddHtml(20, 15, 175, 50, "Are you sure you wish to resign from your guild?", true, false); // Are you sure you want to resign from your guild?

			AddButton(20, 70, 4005, 4007, 2, GumpButtonType.Reply, 0);
			AddHtml(55, 70, 75, 20, "Yes", false, false);

			AddButton(135, 70, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtml(170, 70, 75, 20, "No", false, false);
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 2)
			{
				if (m_From.Guild != null)
					((Guild)m_From.Guild).RemoveMember(m_From);
			}
		}
	}
}
