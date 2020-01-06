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
	public class SBMetalShields : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMetalShields()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Buckler), 66, 20, 0x1B73, 0));
				Add(new GenericBuyInfo(typeof(BronzeShield), 91, 20, 0x1B72, 0));
				Add(new GenericBuyInfo(typeof(MetalShield), 98, 20, 0x1B7B, 0));
				Add(new GenericBuyInfo(typeof(MetalKiteShield), 135, 20, 0x1B74, 0));
				Add(new GenericBuyInfo(typeof(HeaterShield), 185, 20, 0x1B76, 0));
				Add(new GenericBuyInfo(typeof(WoodenKiteShield), 121, 20, 0x1B78, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( Buckler ), 33 );
				 *				Add( typeof( BronzeShield ), 45 );
				 *				Add( typeof( MetalShield ), 49 );
				 *				Add( typeof( MetalKiteShield ), 67 );
				 *				Add( typeof( HeaterShield ), 87 );
				 *				Add( typeof( WoodenKiteShield ), 60 );
				 */
			}
		}
	}
}