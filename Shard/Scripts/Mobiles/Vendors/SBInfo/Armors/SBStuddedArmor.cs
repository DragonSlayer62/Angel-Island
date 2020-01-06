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
	public class SBStuddedArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBStuddedArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(StuddedArms), 87, 20, 0x13DC, 0));
				Add(new GenericBuyInfo(typeof(StuddedChest), 128, 20, 0x13DB, 0));
				Add(new GenericBuyInfo(typeof(StuddedGloves), 79, 20, 0x13D5, 0));
				Add(new GenericBuyInfo(typeof(StuddedGorget), 73, 20, 0x13D6, 0));
				Add(new GenericBuyInfo(typeof(StuddedLegs), 103, 20, 0x13DA, 0));
				Add(new GenericBuyInfo(typeof(FemaleStuddedChest), 142, 20, 0x1C02, 0));
				Add(new GenericBuyInfo(typeof(StuddedBustierArms), 120, 20, 0x1c0c, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( StuddedArms ), 43 );
				 *				Add( typeof( StuddedChest ), 64 );
				 *				Add( typeof( StuddedGloves ), 39 );
				 *				Add( typeof( StuddedGorget ), 36 );
				 *				Add( typeof( StuddedLegs ), 51 );
				 *				Add( typeof( FemaleStuddedChest ), 71 );
				 *				Add( typeof( StuddedBustierArms ), 60 );
				 */
			}
		}
	}
}
