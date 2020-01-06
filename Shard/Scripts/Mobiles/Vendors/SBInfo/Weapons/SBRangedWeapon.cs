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

/* ChangeLog
 *  05/02/05 TK
 *		Removed arrow, bolt, shaft, feather from list - they're covered in Bowyer
 *		Bowyer was selling arrows twice
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBRangedWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBRangedWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Crossbow), 55, 20, 0xF50, 0));
				Add(new GenericBuyInfo(typeof(HeavyCrossbow), 55, 20, 0x13FD, 0));
				if (Core.AOS)
				{
					Add(new GenericBuyInfo(typeof(RepeatingCrossbow), 46, 20, 0x26C3, 0));
					Add(new GenericBuyInfo(typeof(CompositeBow), 45, 20, 0x26C2, 0));
				}
				Add(new GenericBuyInfo(typeof(Bolt), 2, Utility.Random(30, 60), 0x1BFB, 0));
				Add(new GenericBuyInfo(typeof(Bow), 40, 20, 0x13B2, 0));
				Add(new GenericBuyInfo(typeof(Arrow), 2, Utility.Random(30, 60), 0xF3F, 0));
				Add(new GenericBuyInfo(typeof(Feather), 2, Utility.Random(30, 60), 0x1BD1, 0));
				Add(new GenericBuyInfo(typeof(Shaft), 3, Utility.Random(30, 60), 0x1BD4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(Bolt), 1);
					Add(typeof(Arrow), 1);
					Add(typeof(Shaft), 1);
					Add(typeof(Feather), 1);

					Add(typeof(HeavyCrossbow), 27);
					Add(typeof(Bow), 17);
					Add(typeof(Crossbow), 25);

					if (Core.AOS)
					{
						Add(typeof(CompositeBow), 23);
						Add(typeof(RepeatingCrossbow), 22);
					}
				}
			}
		}
	}
}
