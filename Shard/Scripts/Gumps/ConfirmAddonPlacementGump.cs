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

/* Gumps/ConfirmAddonPlacement.cs
 * ChangeLog
 *  12/21/06, Kit
 *      Made cancel happen anytime Okay not pushed.
 *	9/19/04
 *		Created by mith
 */

using System;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Multis;
using Server.Network;

namespace Server.Gumps
{
	public class ConfirmAddonPlacementGump : Gump
	{
		//private Mobile m_Mobile;
		private BaseAddon m_Addon;

		public ConfirmAddonPlacementGump(Mobile from, BaseAddon addon)
			: base(50, 50)
		{
			from.CloseGump(typeof(ConfirmAddonPlacementGump));

			m_Addon = addon;

			AddPage(0);

			AddBackground(10, 10, 190, 140, 0x242C);

			AddHtml(30, 30, 150, 75, String.Format("<div align=CENTER>{0}</div>", "Are you sure you want to place this addon here?"), false, false);

			AddButton(40, 105, 0x81A, 0x81B, 0x1, GumpButtonType.Reply, 0); // Okay
			AddButton(110, 105, 0x819, 0x818, 0x2, GumpButtonType.Reply, 0); // Cancel
		}

		public override void OnResponse(NetState state, RelayInfo info)
		{
			if (m_Addon.Deleted)
				return;

			Mobile from = state.Mobile;

			if (info.ButtonID != 1)
				CancelPlacement(from);
		}

		private void CancelPlacement(Mobile from)
		{
			BaseHouse house = BaseHouse.FindHouseAt(m_Addon);

			if (house != null && house.IsOwner(from) && house.Addons.Contains(m_Addon))
			{
				int hue = 0;

				if (m_Addon.RetainDeedHue)
				{
					for (int i = 0; hue == 0 && i < m_Addon.Components.Count; ++i)
					{
						AddonComponent c = (AddonComponent)m_Addon.Components[i];

						if (c.Hue != 0)
							hue = c.Hue;
					}
				}

				m_Addon.Delete();

				house.Addons.Remove(m_Addon);

				BaseAddonDeed deed = m_Addon.Deed;

				if (deed != null)
				{
					if (m_Addon.RetainDeedHue)
						deed.Hue = hue;

					from.AddToBackpack(deed);
				}
			}
		}
	}
}