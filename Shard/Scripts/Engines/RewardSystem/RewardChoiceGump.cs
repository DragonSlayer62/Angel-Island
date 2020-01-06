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

/* Scripts/Engines/Reward System/RewardChoiceGump.cs
 * Created 5/23/04 by mith
 * ChangeLog
 */

using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Network;

namespace Server.Engines.RewardSystem
{
	public class RewardChoiceGump : Gump
	{
		private Mobile m_From;

		private void RenderBackground()
		{
			AddPage(0);

			AddBackground(10, 10, 600, 450, 2600);

			AddButton(530, 415, 4017, 4019, 0, GumpButtonType.Reply, 0);
		}

		private int GetButtonID(int type, int index)
		{
			return 2 + (index * 20) + type;
		}

		private void RenderCategory(RewardCategory category, int index)
		{
			AddPage(1);

			ArrayList entries = category.Entries;

			for (int i = 0; i < entries.Count; ++i)
			{
				RewardEntry entry = (RewardEntry)entries[i];

				AddButton(55 + ((i / 4) * 250), 80 + ((i % 4) * 50), 5540, 5541, GetButtonID(index, i), GumpButtonType.Reply, 0);

				if (entry.NameString != null)
					AddHtml(80 + ((i / 4) * 250), 80 + ((i % 4) * 50), 250, 20, entry.NameString, false, false);
				else
					AddHtmlLocalized(80 + ((i / 4) * 250), 80 + ((i % 4) * 50), 250, 20, entry.Name, false, false);

				DrawItem(entry, 250 + ((i / 4) * 250), 50 + ((i % 4) * 50));
			}
		}

		public void DrawItem(RewardEntry entry, int x, int y)
		{
			if (entry != null)
			{
				Item item = Activator.CreateInstance(entry.ItemType, entry.Args) as Item;

				AddItem(x, y, item.ItemID);
				item.Delete();
			}
		}

		public RewardChoiceGump(Mobile from, string categoryName)
			: base(0, 0)
		{
			m_From = from;

			from.CloseGump(typeof(RewardChoiceGump));

			RenderBackground();

			RewardCategory[] categories = RewardSystem.Categories;

			for (int i = 0; i < categories.Length; ++i)
			{
				if (categories[i].Entries.Count == 0)
					continue;

				if (!RewardSystem.HasAccess(m_From, (RewardCategory)categories[i]))
					continue;

				if (categories[i].XmlString == categoryName)
					RenderCategory(categories[i], i);
			}
		}

		public override void OnResponse(NetState sender, RelayInfo info)
		{
			int buttonID = info.ButtonID - 1;

			if (buttonID == 0)
			{
				m_From.CloseGump(typeof(RewardChoiceGump));
			}
			else
			{
				--buttonID;

				int type = (buttonID % 20);
				int index = (buttonID / 20);

				RewardCategory[] categories = RewardSystem.Categories;

				if (type >= 0 && type < categories.Length)
				{
					RewardCategory category = categories[type];

					if (index >= 0 && index < category.Entries.Count)
					{
						RewardEntry entry = (RewardEntry)category.Entries[index];

						//						if ( !RewardSystem.HasAccess( m_From, entry ) )
						//							return;

						m_From.SendGump(new RewardConfirmGump(m_From, entry));
					}
				}
			}
		}
	}
}