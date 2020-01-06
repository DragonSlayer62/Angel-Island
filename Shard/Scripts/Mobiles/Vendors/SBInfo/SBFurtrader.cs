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

/* Scripts/Mobiles/Vendors/SBInfo/SBFurTrader.cs
 * ChangeLog
 *  04/02/05 TK
 *		Added special leather types
 *	01/28/05 TK
 *		Added Leather, Hides
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBFurtrader : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBFurtrader()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	// balanced buyback system
					Add(new GenericBuyInfo(typeof(Leather)));
					Add(new GenericBuyInfo(typeof(SpinedLeather)));
					Add(new GenericBuyInfo(typeof(HornedLeather)));
					Add(new GenericBuyInfo(typeof(BarbedLeather)));
				}

				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(new GenericBuyInfo(typeof(Hides), 3, 40, 0x1079, 0));
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	// balanced buyback system
					Add(typeof(Leather));
					Add(typeof(Hides));
					Add(typeof(SpinedLeather));
					Add(typeof(HornedLeather));
					Add(typeof(BarbedLeather));
					Add(typeof(SpinedHides));
					Add(typeof(BarbedHides));
					Add(typeof(HornedHides));
				}

				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(Hides), 2);
				}
			}
		}
	}
}
