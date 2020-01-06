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
using Server.Targeting;

namespace Server
{
	public delegate void BoundingBoxCallback(Mobile from, Map map, Point3D start, Point3D end, object state);

	public class BoundingBoxPicker
	{
		public static void Begin(Mobile from, BoundingBoxCallback callback, object state)
		{
			from.SendMessage("Target the first location of the bounding box.");
			from.Target = new PickTarget(callback, state);
		}

		private class PickTarget : Target
		{
			private Point3D m_Store;
			private bool m_First;
			private Map m_Map;
			private BoundingBoxCallback m_Callback;
			private object m_State;

			public PickTarget(BoundingBoxCallback callback, object state)
				: this(Point3D.Zero, true, null, callback, state)
			{
			}

			public PickTarget(Point3D store, bool first, Map map, BoundingBoxCallback callback, object state)
				: base(-1, true, TargetFlags.None)
			{
				m_Store = store;
				m_First = first;
				m_Map = map;
				m_Callback = callback;
				m_State = state;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				IPoint3D p = targeted as IPoint3D;

				if (p == null)
					return;
				else if (p is Item)
					p = ((Item)p).GetWorldTop();

				if (m_First)
				{
					from.SendMessage("Target another location to complete the bounding box.");
					from.Target = new PickTarget(new Point3D(p), false, from.Map, m_Callback, m_State);
				}
				else if (from.Map != m_Map)
				{
					from.SendMessage("Both locations must reside on the same map.");
				}
				else if (m_Map != null && m_Map != Map.Internal && m_Callback != null)
				{
					Point3D start = m_Store;
					Point3D end = new Point3D(p);

					Utility.FixPoints(ref start, ref end);

					m_Callback(from, m_Map, start, end, m_State);
				}
			}
		}
	}
}