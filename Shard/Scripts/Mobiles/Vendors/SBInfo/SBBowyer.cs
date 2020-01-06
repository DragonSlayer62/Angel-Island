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

/* Scripts/Mobiles/SBInfo/SBBowyer.cs
 * Changelog
 *	01/23/05	Taran Kain
 *		Added arrows, bolts, shafts and feathers.
 */
using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBBowyer : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBowyer()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	// balanced buyback
					Add(new GenericBuyInfo(typeof(Arrow)));
					Add(new GenericBuyInfo(typeof(Bolt)));
					Add(new GenericBuyInfo(typeof(Shaft)));
					Add(new GenericBuyInfo(typeof(Feather)));
				}

				if (!Core.UOAI && !Core.UOAR && !Core.UOMO)
				{	
					Add(new GenericBuyInfo(typeof(FletcherTools), 20, 20, 0x1022, 0));
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	// balanced buyback system
					Add(typeof(Arrow));
					Add(typeof(Bolt));
					Add(typeof(Shaft));
					Add(typeof(Feather));
				}

				if (!Core.UOSP && !Core.UOAI && !Core.UOAR && !Core.UOMO)
				{	// cash buyback
					Add(typeof(FletcherTools), 1);
				}
			}
		}
	}
}
