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
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class AnimalBuyInfo : GenericBuyInfo
	{
		private int m_ControlSlots;

		public AnimalBuyInfo(int controlSlots, Type type, int price, int amount, int itemID, int hue)
			: this(controlSlots, null, type, price, amount, itemID, hue)
		{
		}

		public AnimalBuyInfo(int controlSlots, string name, Type type, int price, int amount, int itemID, int hue)
			: base(name, type, price, amount, itemID, hue)
		{
			m_ControlSlots = controlSlots;
		}

		public override int ControlSlots
		{
			get
			{
				return m_ControlSlots;
			}
		}
	}
}