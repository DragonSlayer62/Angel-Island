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

/* Scripts/Gumps/ViewHouseGump.cs
 * ChangeLog
 *	02/11/06, Adam
 *		Make common the formatting of sextant coords.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.Gumps
{
	public class ViewHousesGump : Gump
	{
		public static void Initialize()
		{
			CommandSystem.Register("ViewHouses", AccessLevel.GameMaster, new CommandEventHandler(ViewHouses_OnCommand));
		}

		[Usage("ViewHouses")]
		[Description("Displays a menu listing all houses of a targeted player. The menu also contains specific house details, and options to: go to house, open house menu, and demolish house.")]
		public static void ViewHouses_OnCommand(CommandEventArgs e)
		{
			e.Mobile.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(ViewHouses_OnTarget));
		}

		public static void ViewHouses_OnTarget(Mobile from, object targeted)
		{
			if (targeted is Mobile)
				from.SendGump(new ViewHousesGump(from, GetHouses((Mobile)targeted), null));
		}

		private class HouseComparer : IComparer
		{
			public static readonly IComparer Instance = new HouseComparer();

			public int Compare(object x, object y)
			{
				return ((BaseHouse)x).BuiltOn.CompareTo(((BaseHouse)y).BuiltOn);
			}
		}

		public static ArrayList GetHouses(Mobile owner)
		{
			ArrayList list = BaseHouse.GetHouses(owner);

			list.Sort(HouseComparer.Instance);

			return list;
		}

		private Mobile m_From;
		private ArrayList m_List;
		private BaseHouse m_Selection;

		public ViewHousesGump(Mobile from, ArrayList list, BaseHouse sel)
			: base(50, 40)
		{
			m_From = from;
			m_List = list;
			m_Selection = sel;

			from.CloseGump(typeof(ViewHousesGump));

			AddPage(0);

			AddBackground(0, 0, 240, 360, 5054);
			AddBlackAlpha(10, 10, 220, 340);

			if (sel == null || sel.Deleted)
			{
				m_Selection = null;

				AddHtml(35, 15, 120, 20, Color("House Type", White), false, false);

				if (list.Count == 0)
					AddHtml(35, 40, 160, 40, Color("There were no houses found for that player.", White), false, false);

				AddImage(190, 17, 0x25EA);
				AddImage(207, 17, 0x25E6);

				int page = 0;

				for (int i = 0; i < list.Count; ++i)
				{
					if ((i % 15) == 0)
					{
						if (page > 0)
							AddButton(207, 17, 0x15E1, 0x15E5, 0, GumpButtonType.Page, page + 1);

						AddPage(++page);

						if (page > 1)
							AddButton(190, 17, 0x15E3, 0x15E7, 0, GumpButtonType.Page, page - 1);
					}

					object name = FindHouseName((BaseHouse)list[i]);

					AddHtml(15, 40 + ((i % 15) * 20), 20, 20, Color(String.Format("{0}.", i + 1), White), false, false);

					if (name is int)
						AddHtmlLocalized(35, 40 + ((i % 15) * 20), 160, 20, (int)name, White16, false, false);
					else if (name is string)
						AddHtml(35, 40 + ((i % 15) * 20), 160, 20, Color((string)name, White), false, false);

					AddButton(198, 39 + ((i % 15) * 20), 4005, 4007, i + 1, GumpButtonType.Reply, 0);
				}
			}
			else
			{
				string houseName, owner, location;
				Map map = sel.Map;

				Item sign = sel.Sign;

				if (sign == null || sign.Name == null || sign.Name == "a house sign")
					houseName = "nothing";
				else
					houseName = sign.Name;

				Mobile houseOwner = sel.Owner;

				if (houseOwner == null)
					owner = "nobody";
				else
					owner = houseOwner.Name;

				int xLong = 0, yLat = 0, xMins = 0, yMins = 0;
				bool xEast = false, ySouth = false;

				bool valid = Sextant.Format(sel.Location, map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth);

				if (valid)
					location = Sextant.Format(xLong, yLat, xMins, yMins, xEast, ySouth);
				else
					location = "????";

				AddHtml(10, 15, 220, 20, Color(Center("House Properties"), White), false, false);

				AddHtml(15, 40, 210, 20, Color("Facet:", White), false, false);
				AddHtml(15, 40, 210, 20, Color(Right(map == null ? "(null)" : map.Name), White), false, false);

				AddHtml(15, 60, 210, 20, Color("Location:", White), false, false);
				AddHtml(15, 60, 210, 20, Color(Right(sel.Location.ToString()), White), false, false);

				AddHtml(15, 80, 210, 20, Color("Sextant:", White), false, false);
				AddHtml(15, 80, 210, 20, Color(Right(location), White), false, false);

				AddHtml(15, 100, 210, 20, Color("Owner:", White), false, false);
				AddHtml(15, 100, 210, 20, Color(Right(owner), White), false, false);

				AddHtml(15, 120, 210, 20, Color("Name:", White), false, false);
				AddHtml(15, 120, 210, 20, Color(Right(houseName), White), false, false);

				AddHtml(15, 140, 210, 20, Color("Friends:", White), false, false);
				AddHtml(15, 140, 210, 20, Color(Right(sel.Friends.Count.ToString()), White), false, false);

				AddHtml(15, 160, 210, 20, Color("Co-Owners:", White), false, false);
				AddHtml(15, 160, 210, 20, Color(Right(sel.CoOwners.Count.ToString()), White), false, false);

				AddHtml(15, 180, 210, 20, Color("Bans:", White), false, false);
				AddHtml(15, 180, 210, 20, Color(Right(sel.Bans.Count.ToString()), White), false, false);

				AddButton(15, 205, 4005, 4007, 1, GumpButtonType.Reply, 0);
				AddHtml(50, 205, 120, 20, Color("Go to house", White), false, false);

				AddButton(15, 225, 4005, 4007, 2, GumpButtonType.Reply, 0);
				AddHtml(50, 225, 120, 20, Color("Open house menu", White), false, false);

				AddButton(15, 245, 4005, 4007, 3, GumpButtonType.Reply, 0);
				AddHtml(50, 245, 120, 20, Color("Demolish house", White), false, false);
			}
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (m_Selection == null)
			{
				int v = info.ButtonID - 1;

				if (v >= 0 && v < m_List.Count)
					m_From.SendGump(new ViewHousesGump(m_From, m_List, (BaseHouse)m_List[v]));
			}
			else if (!m_Selection.Deleted)
			{
				switch (info.ButtonID)
				{
					case 0:
						{
							m_From.SendGump(new ViewHousesGump(m_From, m_List, null));
							break;
						}
					case 1:
						{
							Map map = m_Selection.Map;

							if (map != null && map != Map.Internal)
								m_From.MoveToWorld(m_Selection.BanLocation, map);

							m_From.SendGump(new ViewHousesGump(m_From, m_List, m_Selection));

							break;
						}
					case 2:
						{
							m_From.SendGump(new ViewHousesGump(m_From, m_List, m_Selection));

							HouseSign sign = m_Selection.Sign;

							if (sign != null && !sign.Deleted)
								sign.OnDoubleClick(m_From);

							break;
						}
					case 3:
						{
							m_From.SendGump(new ViewHousesGump(m_From, m_List, m_Selection));
							m_From.SendGump(new HouseDemolishGump(m_From, m_Selection));

							break;
						}
				}
			}
		}

		public object FindHouseName(BaseHouse house)
		{
			int multiID = house.ItemID & 0x3FFF;
			HousePlacementEntry[] entries;

			entries = HousePlacementEntry.ClassicHouses;

			for (int i = 0; i < entries.Length; ++i)
			{
				if (entries[i].MultiID == multiID)
					return entries[i].Description;
			}

			entries = HousePlacementEntry.TwoStoryFoundations;

			for (int i = 0; i < entries.Length; ++i)
			{
				if (entries[i].MultiID == multiID)
					return entries[i].Description;
			}

			entries = HousePlacementEntry.ThreeStoryFoundations;

			for (int i = 0; i < entries.Length; ++i)
			{
				if (entries[i].MultiID == multiID)
					return entries[i].Description;
			}

			return house.GetType().Name;
		}

		private const int White16 = 0x7FFF;
		private const int White = 0xFFFFFF;

		public string Right(string text)
		{
			return String.Format("<div align=right>{0}</div>", text);
		}

		public string Center(string text)
		{
			return String.Format("<CENTER>{0}</CENTER>", text);
		}

		public string Color(string text, int color)
		{
			return String.Format("<BASEFONT COLOR=#{0:X6}>{1}</BASEFONT>", color, text);
		}

		public void AddBlackAlpha(int x, int y, int width, int height)
		{
			AddImageTiled(x, y, width, height, 2624);
			AddAlphaRegion(x, y, width, height);
		}
	}
}