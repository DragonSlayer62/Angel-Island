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
using Server.Items;

namespace Server.Engines.Craft
{
	public class QueryMakersMarkGump : Gump
	{
		private int m_Quality;
		private Mobile m_From;
		private CraftItem m_CraftItem;
		private CraftSystem m_CraftSystem;
		private Type m_TypeRes;
		private BaseTool m_Tool;

		public QueryMakersMarkGump(int quality, Mobile from, CraftItem craftItem, CraftSystem craftSystem, Type typeRes, BaseTool tool)
			: base(100, 200)
		{
			from.CloseGump(typeof(QueryMakersMarkGump));

			m_Quality = quality;
			m_From = from;
			m_CraftItem = craftItem;
			m_CraftSystem = craftSystem;
			m_TypeRes = typeRes;
			m_Tool = tool;

			AddPage(0);

			AddBackground(0, 0, 220, 170, 5054);
			AddBackground(10, 10, 200, 150, 3000);

			AddHtmlLocalized(20, 20, 180, 80, 1018317, false, false); // Do you wish to place your maker's mark on this item?

			AddHtmlLocalized(55, 100, 140, 25, 1011011, false, false); // CONTINUE
			AddButton(20, 100, 4005, 4007, 1, GumpButtonType.Reply, 0);

			AddHtmlLocalized(55, 125, 140, 25, 1011012, false, false); // CANCEL
			AddButton(20, 125, 4005, 4007, 0, GumpButtonType.Reply, 0);
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			bool makersMark = (info.ButtonID == 1);

			if (makersMark)
				m_From.SendLocalizedMessage(501808); // You mark the item.
			else
				m_From.SendLocalizedMessage(501809); // Cancelled mark.

			m_CraftItem.CompleteCraft(m_Quality, makersMark, m_From, m_CraftSystem, m_TypeRes, m_Tool, null);
		}
	}

	public class OldSchoolMakersMarkGump : Gump
	{
		private Item m_item;
		private Mobile m_from;

		public OldSchoolMakersMarkGump(Mobile from, Item item)
			: base(100, 200)
		{
			m_item = item;
			m_from = from;

			from.CloseGump(typeof(OldSchoolMakersMarkGump));

			// old school looking gump
			AddImage(273, 227, 2070);
			AddButton(306, 302, 2071, 2072, 0, GumpButtonType.Reply, 0);	// Cancel
			AddButton(370, 302, 2074, 2075, 1, GumpButtonType.Reply, 0);	// OK
			AddLabel(317, 253, 0, @"Do you wish to");
			AddLabel(310, 270, 0, @" mark the item?");
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			bool makersMark = (info.ButtonID == 1);

			if (makersMark && m_item != null && m_item.Deleted == false)
			{
				if (m_item is BaseArmor)
					(m_item as BaseArmor).Crafter = m_from;
				if (m_item is BaseWeapon)
					(m_item as BaseWeapon).Crafter = m_from;
			}
		}
	}
}