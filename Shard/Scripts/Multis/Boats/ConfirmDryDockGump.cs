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

namespace Server.Multis
{
	public class ConfirmDryDockGump : Gump
	{
		private Mobile m_From;
		private BaseBoat m_Boat;

		public ConfirmDryDockGump(Mobile from, BaseBoat boat)
			: base(150, 200)
		{
			m_From = from;
			m_Boat = boat;

			m_From.CloseGump(typeof(ConfirmDryDockGump));

			AddPage(0);

			AddBackground(0, 0, 220, 170, 5054);
			AddBackground(10, 10, 200, 150, 3000);

			AddHtmlLocalized(20, 20, 180, 80, 1018319, true, false); // Do you wish to dry dock this boat?

			AddHtmlLocalized(55, 100, 140, 25, 1011011, false, false); // CONTINUE
			AddButton(20, 100, 4005, 4007, 2, GumpButtonType.Reply, 0);

			AddHtmlLocalized(55, 125, 140, 25, 1011012, false, false); // CANCEL
			AddButton(20, 125, 4005, 4007, 1, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (info.ButtonID == 2)
				m_Boat.EndDryDock(m_From);
		}
	}
}