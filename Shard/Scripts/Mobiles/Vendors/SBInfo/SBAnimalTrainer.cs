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

/* Scripts/Mobiles/Vendors/SBInfo/AnimalTrainer.cs
 * CHANGELOG:
 *	5/6/05: Pix
 *		Updated "ItemID" field for mobiles that the animal trainer sells
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBAnimalTrainer : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBAnimalTrainer()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new AnimalBuyInfo(1, typeof(Eagle), 402, 10, 5, 0));
				Add(new AnimalBuyInfo(1, typeof(Cat), 138, 10, 201, 0));
				Add(new AnimalBuyInfo(1, typeof(Horse), 602, 10, 204, 0));
				Add(new AnimalBuyInfo(1, typeof(Rabbit), 78, 10, 205, 0));
				Add(new AnimalBuyInfo(1, typeof(BrownBear), 855, 10, 167, 0));
				Add(new AnimalBuyInfo(1, typeof(GrizzlyBear), 1767, 10, 212, 0));
				Add(new AnimalBuyInfo(1, typeof(Panther), 1271, 10, 214, 0));
				Add(new AnimalBuyInfo(1, typeof(Dog), 181, 10, 217, 0));
				Add(new AnimalBuyInfo(1, typeof(TimberWolf), 768, 10, 225, 0));
				Add(new AnimalBuyInfo(1, typeof(PackHorse), 606, 10, 291, 0));
				Add(new AnimalBuyInfo(1, typeof(PackLlama), 491, 10, 292, 0));
				Add(new AnimalBuyInfo(1, typeof(Rat), 107, 10, 238, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}
