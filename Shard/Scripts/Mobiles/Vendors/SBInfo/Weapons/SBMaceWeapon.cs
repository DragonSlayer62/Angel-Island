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
	public class SBMaceWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMaceWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(Club), 27, 20, 0x13B4, 0));
				Add(new GenericBuyInfo(typeof(HammerPick), 31, 20, 0x143D, 0));
				Add(new GenericBuyInfo(typeof(Mace), 38, 20, 0xF5C, 0));
				Add(new GenericBuyInfo(typeof(Maul), 31, 20, 0x143B, 0));
				Add(new GenericBuyInfo(typeof(WarHammer), 27, 20, 0x1439, 0));
				Add(new GenericBuyInfo(typeof(WarMace), 37, 20, 0x1407, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( Club ), 13 );
				 *				Add( typeof( HammerPick ), 15 );
				 *				Add( typeof( Mace ), 30 );
				 *				Add( typeof( Maul ), 15 );
				 *				Add( typeof( WarHammer ), 13 );
				 *				Add( typeof( WarMace ), 18 );
				 */
			}
		}
	}
}
