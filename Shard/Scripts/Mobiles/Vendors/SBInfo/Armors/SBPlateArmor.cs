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
	public class SBPlateArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBPlateArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(PlateArms), 181, 20, 0x1410, 0));
				Add(new GenericBuyInfo(typeof(PlateChest), 273, 20, 0x1415, 0));
				Add(new GenericBuyInfo(typeof(PlateGloves), 145, 20, 0x1414, 0));
				Add(new GenericBuyInfo(typeof(PlateGorget), 124, 20, 0x1413, 0));
				Add(new GenericBuyInfo(typeof(PlateLegs), 218, 20, 0x1411, 0));

				Add(new GenericBuyInfo(typeof(PlateHelm), 170, 20, 0x1412, 0));
				Add(new GenericBuyInfo(typeof(FemalePlateChest), 245, 20, 0x1C04, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( PlateArms ), 90 );
				 *				Add( typeof( PlateChest ), 136 );
				 *				Add( typeof( PlateGloves ), 72 );
				 *				Add( typeof( PlateGorget ), 70 );
				 *				Add( typeof( PlateLegs ), 109 );
				 *
				 *				Add( typeof( PlateHelm ), 85 );
				 *				Add( typeof( FemalePlateChest ), 122 );
				 */
			}
		}
	}
}
