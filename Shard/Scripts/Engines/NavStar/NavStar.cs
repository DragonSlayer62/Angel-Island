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

/* Scripts/Engines/NavStar/Navstar.cs
 * CHANGELOG
 * 12/31/05, Kit
 *		Bug fix with random beacon selection.
 * 12/05/05, Kit
 *		Updated NavStar to set home area of creature to location of beacon returned.
 * 11/30/05, Kit
 *		Updated due to changes to NavBeacon, do not add beacons of inactive or -1 weight to beacon list,
 *		if multiple beacons same weight choose random beacon of said weight.
 * 11/18/05, Kit
 * 	Initial Creation
 */
using System;
using Server;
using Server.Regions;
using Server.Items;
using System.Collections;
using Server.Mobiles;

namespace Server.Engines
{

	public enum NavDestinations : int
	{
		None = 0,
		Britan,
		Vesper,
		Cove,
		Minoc,
		Skara,
		Wrong,
		Shame,
		BritGY,
		Covet
	};

	public class NavList
	{
		Mobile requester;
		NavDestinations Nav;
		public NavList(Mobile m, NavDestinations dest)
		{
			requester = m;
			Nav = dest;
		}
		public Mobile GetRequester()
		{
			return requester;
		}
		public NavDestinations GetDestination()
		{
			return Nav;
		}
	}

	public class NavStar
	{
		private static ArrayList Navs = new ArrayList();


		public static void AddRequest(Mobile m, NavDestinations nav)
		{
			NavList n = new NavList(m, nav);
			Navs.Add(n);
		}

		private static void RemoveRequest(NavList m)
		{
			Navs.Remove(m);
		}

		private static NavList GetRequest()
		{
			return (NavList)Navs[0];
		}

		private static ArrayList GetBeacons(Mobile m, NavDestinations dest)
		{
			Map map = m.Map;
			ArrayList list = new ArrayList();
			IPooledEnumerable eable = map.GetItemsInRange(m.Location, 256);

			//add each NavBeacon found add it to are list
			foreach (Item n in eable)
			{
				if (n != null && n is NavBeacon && ((NavBeacon)n).Active && ((NavBeacon)n).GetWeight((int)dest) != -1)
					list.Add(n);
			}
			return list;
		}


		public enum SortDirection
		{
			Ascending,
			Descending
		}

		public class Beacon_Sort : IComparer
		{

			private SortDirection m_direction = SortDirection.Ascending;

			private NavDestinations nav;

			public Beacon_Sort(SortDirection direction, NavDestinations dest)
			{
				m_direction = direction;
				nav = dest;
			}

			int IComparer.Compare(object x, object y)
			{
				NavBeacon mobileX = (NavBeacon)x;
				NavBeacon mobileY = (NavBeacon)y;

				if (mobileX == null && mobileY == null)
				{
					return 0;
				}
				else if (mobileX == null && mobileY != null)
				{
					return (this.m_direction == SortDirection.Ascending) ? -1 : 1;
				}
				else if (mobileX != null && mobileY == null)
				{
					return (this.m_direction == SortDirection.Ascending) ? 1 : -1;
				}
				else
				{

					return (this.m_direction == SortDirection.Ascending) ?
						mobileX.GetWeight((int)nav).CompareTo(mobileY.GetWeight((int)nav)) :
						mobileY.GetWeight((int)nav).CompareTo(mobileX.GetWeight((int)nav));

				}
			}
		}


		public static void DoRequest()
		{

			NavList request = GetRequest();
			if (request != null)
			{
				ArrayList list = GetBeacons(request.GetRequester(), request.GetDestination());
				if (list.Count >= 1)
				{
					list.Sort(new Beacon_Sort(SortDirection.Ascending, request.GetDestination()));

					Mobile m = request.GetRequester();

					if (((Item)list[0]) != ((BaseCreature)m).Beacon)
					{
						//loop through and for each beacon of same weight add it to a 2nd list then choose a random one from it
						ArrayList randombeacon = new ArrayList();

						int weight = ((NavBeacon)list[0]).GetWeight((int)request.GetDestination());

						foreach (NavBeacon n in list)
						{
							if (n.GetWeight((int)request.GetDestination()) == weight)
								randombeacon.Add(n);
						}

						int i = Utility.Random(randombeacon.Count);

						((BaseCreature)m).NavPoint = ((Item)list[i]).Location;
						((BaseCreature)m).Beacon = ((NavBeacon)list[i]);
						((BaseCreature)m).Home = ((Item)list[i]).Location; ;
						RemoveRequest(request);
					}
					else
					{
						((BaseCreature)m).NavPoint = Point3D.Zero;
						((BaseCreature)m).NavDestination = NavDestinations.None;
						((BaseCreature)m).Beacon = null;
						RemoveRequest(request);
					}
				}
				else
				{
					Mobile m = request.GetRequester();
					((BaseCreature)m).NavDestination = NavDestinations.None;
					((BaseCreature)m).NavPoint = Point3D.Zero;
					((BaseCreature)m).Beacon = null;
					RemoveRequest(request);
				}

			}


		}

	}
}
