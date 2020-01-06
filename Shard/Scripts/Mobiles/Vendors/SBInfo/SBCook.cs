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

/* Scripts/Mobiles/Vendors/SBInfo/SBCook.cs
 * ChangeLog
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBCook : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBCook()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(CheeseWheel), 25, 20, 0x97E, 0));
				Add(new GenericBuyInfo("1044567", typeof(Skillet), 3, 20, 0x97F, 0));
				Add(new GenericBuyInfo(typeof(CookedBird), 17, 20, 0x9B7, 0));
				Add(new GenericBuyInfo(typeof(RoastPig), 106, 20, 0x9BB, 0));
				Add(new GenericBuyInfo(typeof(Cake), 11, 20, 0x9E9, 0));
				// TODO: Muffin @ 3gp
				Add(new GenericBuyInfo(typeof(JarHoney), 3, 20, 0x9EC, 0));
				Add(new GenericBuyInfo(typeof(SackFlour), 3, 20, 0x1039, 0));
				Add(new GenericBuyInfo(typeof(BreadLoaf), 7, 20, 0x103B, 0));
				Add(new GenericBuyInfo(typeof(FlourSifter), 2, 20, 0x103E, 0));
				//Add( new GenericBuyInfo( typeof( BakedPie ), 7, 20, 0x1041, 0 ) );
				Add(new GenericBuyInfo(typeof(RollingPin), 2, 20, 0x1043, 0));
				// TODO: Bowl of carrots/corn/lettuce/peas/potatoes/stew/tomato soup @ 3gp
				// TODO: Pewter bowl @ 2gp
				Add(new GenericBuyInfo(typeof(ChickenLeg), 6, 20, 0x1608, 0));
				Add(new GenericBuyInfo(typeof(LambLeg), 8, 20, 0x1609, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(CheeseWheel), 12);
					Add(typeof(CookedBird), 8);
					Add(typeof(RoastPig), 53);
					Add(typeof(Cake), 5);
					Add(typeof(JarHoney), 1);
					Add(typeof(SackFlour), 1);
					Add(typeof(BreadLoaf), 3);
					Add(typeof(ChickenLeg), 3);
					Add(typeof(LambLeg), 4);
					Add(typeof(Skillet), 1);
					Add(typeof(FlourSifter), 1);
					Add(typeof(RollingPin), 1);
				}
			}
		}
	}
}