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

namespace Server.Engines.Help
{
	public class PageResponseGump : Gump
	{
		private Mobile m_From;
		private string m_Name, m_Text;

		public PageResponseGump(Mobile from, string name, string text)
			: base(0, 0)
		{
			m_From = from;
			m_Name = name;
			m_Text = text;

			AddBackground(50, 25, 540, 430, 2600);

			AddPage(0);

			AddHtmlLocalized(150, 40, 360, 40, 1062610, false, false); // <CENTER><U>Ultima Online Help Response</U></CENTER>

			AddHtml(80, 90, 480, 290, String.Format("{0} tells {1}: {2}", name, from.Name, text), true, true);

			AddHtmlLocalized(80, 390, 480, 40, 1062611, false, false); // Clicking the OKAY button will remove the reponse you have received.
			AddButton(400, 417, 2074, 2075, 1, GumpButtonType.Reply, 0); // OKAY

			AddButton(475, 417, 2073, 2072, 0, GumpButtonType.Reply, 0); // CANCEL
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID != 1)
				m_From.SendGump(new MessageSentGump(m_From, m_Name, m_Text));
		}
	}
}
