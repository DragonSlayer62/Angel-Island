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
using Server.Items;

namespace Server.ContextMenus
{
	public class EatEntry : ContextMenuEntry
	{
		private Mobile m_From;
		private Food m_Food;

		public EatEntry(Mobile from, Food food)
			: base(6135, 1)
		{
			m_From = from;
			m_Food = food;
		}

		public override void OnClick()
		{
			if (m_Food.Deleted || !m_Food.Movable || !m_From.CheckAlive())
				return;

			m_Food.Eat(m_From);
		}
	}
}