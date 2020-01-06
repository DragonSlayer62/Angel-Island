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
using Server.Mobiles;
using System.Collections;

namespace Server.Targets
{
	public class AIControlMobileTarget : Target
	{
		private ArrayList m_List;
		private OrderType m_Order;

		public OrderType Order
		{
			get
			{
				return m_Order;
			}
		}

		public AIControlMobileTarget(BaseAI ai, OrderType order)
			: base(-1, false, TargetFlags.None)
		{
			m_List = new ArrayList();
			m_Order = order;

			AddAI(ai);
		}

		public void AddAI(BaseAI ai)
		{
			if (!m_List.Contains(ai))
				m_List.Add(ai);
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o is Mobile)
			{
				for (int i = 0; i < m_List.Count; ++i)
					((BaseAI)m_List[i]).EndPickTarget(from, (Mobile)o, m_Order);
			}
		}
	}
}