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
/* Scripts/Gumps/ForensicKillerGump.cs
 *
 *	ChangeLog:
 *	5/19/04 created by smerX
 */

using System;
using System.Collections;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class ForensicChoiceGump : Gump
	{
		private Mobile m_Owner;
		private Mobile m_Killer;
		private ArrayList m_Looters;
		private BountyLedger m_Book;

		public ForensicChoiceGump(Mobile owner, Mobile killer, ArrayList looters, BountyLedger book)
			: base(30, 30)
		{
			owner.CloseGump(typeof(ForensicChoiceGump));

			m_Owner = owner;
			m_Killer = killer;
			m_Looters = looters;
			m_Book = book;

			BuildChoiceGump();
		}

		public void BuildChoiceGump()
		{
			int gumpsizeX = 300;
			int gumpsizeY = 140;
			int borderwidth = 8;

			int firstbuttonY = 65;
			int secondbuttonY = 100;
			int bothX = 195;

			AddPage(0);

			AddBackground(0, 0, gumpsizeX, gumpsizeY, PropsConfig.BackGumpID);
			AddImageTiled(borderwidth - 1, borderwidth, 35 + ((gumpsizeX - (borderwidth * 2)) / 2), gumpsizeY - (borderwidth * 2), PropsConfig.HeaderGumpID);

			AddLabel(20, 20, 0x47e, "Do you seek information");
			AddLabel(20, 50, 0x47e, "about the killer, or those");
			AddLabel(20, 80, 0x47e, "who looted the corpse?");

			AddButton(bothX, firstbuttonY, 0xFA5, 0xFA7, 1, GumpButtonType.Reply, 0);
			AddLabel(bothX + 35, firstbuttonY, 0x0, "Killer");
			//AddHtml( firstbuttonX + 35, bothY, 90, 80, "Cancel", false, false );

			AddButton(bothX, secondbuttonY, 0xFA5, 0xFA7, 2, GumpButtonType.Reply, 0);
			AddLabel(bothX + 35, secondbuttonY, 0x0, "Looters");
			//AddHtml( secondbuttonX + 35, bothY, 90, 80, "Add", false, false );
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			Mobile from = state.Mobile;

			switch (info.ButtonID)
			{
				case 0: // Closed
					{
						from.SendMessage("You decide not to examine this corpse.");
						break;
					}
				case 1: // Killer
					{
						Mobile m = m_Killer;

						if (m.Deleted)
						{
							from.SendMessage("That player has deleted their character.");
							from.SendGump(new ForensicChoiceGump(from, m_Killer, m_Looters, m_Book));
						}
						else
						{
							from.SendGump(new ForensicKillerGump(m_Owner, m_Killer, m_Book));
						}

						break;
					}
				default: // Looters
					{
						from.SendGump(new ForensicLootGump(from, m_Looters, 0, m_Book));
						break;
					}
			}
		}
	}
}