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
using Server.Multis;

namespace Server.Mobiles
{
	public class SBShipwright : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBShipwright()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("1041205", typeof(SmallBoatDeed), 12500, 20, 0x14F2, 0));
				Add(new GenericBuyInfo("1041206", typeof(SmallDragonBoatDeed), 12500, 20, 0x14F2, 0));
				Add(new GenericBuyInfo("1041207", typeof(MediumBoatDeed), 14200, 20, 0x14F2, 0));
				Add(new GenericBuyInfo("1041208", typeof(MediumDragonBoatDeed), 14200, 20, 0x14F2, 0));
				Add(new GenericBuyInfo("1041209", typeof(LargeBoatDeed), 15900, 20, 0x14F2, 0));
				Add(new GenericBuyInfo("1041210", typeof(LargeDragonBoatDeed), 15900, 20, 0x14F2, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(SmallBoatDeed), 6250);
					Add(typeof(SmallDragonBoatDeed), 6250);
					Add(typeof(MediumBoatDeed), 7100);
					Add(typeof(MediumDragonBoatDeed), 7100);
					Add(typeof(LargeBoatDeed), 7950);
					Add(typeof(LargeDragonBoatDeed), 7950);
				}
			}
		}
	}
}