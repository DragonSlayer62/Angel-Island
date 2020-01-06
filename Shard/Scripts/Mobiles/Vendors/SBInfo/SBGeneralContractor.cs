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

/* Scripts/Mobiles/Vendors/SBInfo/SBGeneralContractor.cs
 * ChangeLog
 *  5/8/07, Adam
 *      First time checkin
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBGeneralContractor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBGeneralContractor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	
					Add(new GenericBuyInfo(typeof(BookofUpgradeContracts), 100, 20, 0xFF0, 0));
					Add(new GenericBuyInfo(typeof(ModestUpgradeContract), 82562, 20, 0x14F0, 0));
					Add(new GenericBuyInfo(typeof(ModerateUpgradeContract), 195750, 20, 0x14F0, 0));
					Add(new GenericBuyInfo(typeof(PremiumUpgradeContract), 498900, 20, 0x14F0, 0));
					Add(new GenericBuyInfo(typeof(ExtravagantUpgradeContract), 767100, 20, 0x14F0, 0));
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(InteriorDecorator), 5000);
				}

				if (Core.AOS)
				{	// cash buyback (AOS)
					Add(typeof(HousePlacementTool), 301);
				}
			}
		}
	}
}