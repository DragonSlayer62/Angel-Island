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

/* Scripts/Engines/Reward System/RewardConfirmGump.cs
 * Created 5/23/04 by mith
 * ChangeLog
 */

using System;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.RewardSystem
{
	public class RewardConfirmGump : Gump
	{
		private Mobile m_From;
		private RewardEntry m_Entry;

		public RewardConfirmGump(Mobile from, RewardEntry entry)
			: base(0, 0)
		{
			m_From = from;
			m_Entry = entry;

			from.CloseGump(typeof(RewardConfirmGump));

			AddPage(0);

			AddBackground(10, 10, 500, 300, 2600);

			AddHtmlLocalized(30, 55, 300, 35, 1006000, false, false); // You have selected:

			if (entry.NameString != null)
				AddHtml(335, 55, 150, 35, entry.NameString, false, false);
			else
				AddHtmlLocalized(335, 55, 150, 35, entry.Name, false, false);

			AddHtmlLocalized(30, 95, 300, 35, 1006001, false, false); // This will be assigned to this character:
			AddLabel(335, 95, 0, from.Name);

			AddHtmlLocalized(35, 160, 450, 90, 1006002, true, true); // Are you sure you wish to select this reward for this character?  You will not be able to transfer this reward to another character on another shard.  Click 'ok' below to confirm your selection or 'cancel' to go back to the selection screen.

			AddButton(60, 265, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(95, 266, 150, 35, 1006044, false, false); // Ok

			AddButton(295, 265, 4017, 4019, 0, GumpButtonType.Reply, 0);
			AddHtmlLocalized(330, 266, 150, 35, 1006045, false, false); // Cancel
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 1)
			{
				Item item = m_Entry.Construct();

				if (item != null)
				{
					item.Weight = 10.0;
					item.Name = "A statue honoring " + m_From.Name + ", Angel Island Pioneer";
					if (RewardSystem.UpdateRewardCodes(m_From))
						m_From.AddToBackpack(item);
					else
						item.Delete();
				}
			}
		}
	}
}