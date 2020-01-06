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
/*      changelog.
 *      
 *    9/23/04 Lego Eater.
 *            Made Survey Tool Gump From Warninggump.cs
 *
 *
 *
 *
 */

using System;
using Server;

namespace Server.Gumps
{
	public delegate void SurveyToolGumpCallback(Mobile from, bool okay, object state);

	public class SurveyToolGump : Gump
	{
		private SurveyToolGumpCallback m_Callback;
		private object m_State;

		public SurveyToolGump(int header, int headerColor, object content, int contentColor, int width, int height, SurveyToolGumpCallback callback, object state)
			: base(50, 50)
		{
			m_Callback = callback;
			m_State = state;

			Closable = true;

			AddPage(0);

			AddBackground(10, 10, 190, 140, 0x242C);


			AddHtml(30, 30, 150, 75, String.Format("<div align=CENTER>{0}</div>", "This house seems to fit here."), false, false);


			AddButton(40, 85, 4005, 4007, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(40, 107, 0x81A, 0x81B, 1011036, 32767, false, false); // okay
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1 && m_Callback != null)
				m_Callback(sender.Mobile, true, m_State);
			else if (m_Callback != null)
				m_Callback(sender.Mobile, false, m_State);
		}
	}
}