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

/* Scripts/Mobiles/Vendors/SBInfo/SBLeatherWorker.cs
 * ChangeLog
 *  04/02/05 TK
 *		Added special leather, hides
 *  01/28/05 TK
 *		Added hides
 *	01/23/05, Taran Kain
 *		Added leather.
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBLeatherWorker : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBLeatherWorker()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{

				/* Shopkeeper NPCs do not sell any resources (Ingots, Cloth, etc.))
				 * http://www.uoguide.com/Siege_Perilous
				 */
				if (!Core.UOSP && !Core.UOAI && !Core.UOAR && !Core.UOMO)
					Add(new GenericBuyInfo(typeof(Hides), 4, 999, 0x1078, 0));

				if (!Core.UOAI && !Core.UOAR && !Core.UOMO)
					// only sell these on some servers
					Add(new GenericBuyInfo(typeof(ThighBoots), 56, 10, 0x1711, 0));

				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	// balanced buyback support
					Add(new GenericBuyInfo(typeof(Leather)));
					Add(new GenericBuyInfo(typeof(SpinedLeather)));
					Add(new GenericBuyInfo(typeof(HornedLeather)));
					Add(new GenericBuyInfo(typeof(BarbedLeather)));
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
					Add(typeof(ThighBoots), 28);
				}
			}
		}
	}
}
