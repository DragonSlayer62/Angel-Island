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

/* Scripts/Mobiles/Vendors/SBInfo/SBButcher.cs
 * ChangeLog
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBButcher : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBButcher()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(RawRibs), 7, 20, 0x9F1, 0));
				Add(new GenericBuyInfo(typeof(RawLambLeg), 5, 20, 0x1609, 0));
				Add(new GenericBuyInfo(typeof(RawChickenLeg), 2, 20, 0x1607, 0));
				Add(new GenericBuyInfo(typeof(RawBird), 3, 20, 0x9B9, 0));
				Add(new GenericBuyInfo(typeof(Bacon), 3, 20, 0x979, 0));
				Add(new GenericBuyInfo(typeof(Sausage), 17, 20, 0x9C0, 0));
				Add(new GenericBuyInfo(typeof(Ham), 20, 20, 0x9C9, 0));
				Add(new GenericBuyInfo(typeof(ButcherKnife), 21, 20, 0x13F6, 0));
				Add(new GenericBuyInfo(typeof(Cleaver), 24, 20, 0xEC3, 0));
				Add(new GenericBuyInfo(typeof(SkinningKnife), 26, 20, 0xEC4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(RawRibs), 3);
					Add(typeof(RawLambLeg), 2);
					Add(typeof(RawChickenLeg), 1);
					Add(typeof(RawBird), 1);
					Add(typeof(Bacon), 1);
					Add(typeof(Sausage), 8);
					Add(typeof(Ham), 10);
					Add(typeof(ButcherKnife), 13);
					Add(typeof(Cleaver), 12);
					Add(typeof(SkinningKnife), 10);
				}
			}
		}
	}
}