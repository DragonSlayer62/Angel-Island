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
	public class SBAxeWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBAxeWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BattleAxe), 38, 20, 0xF47, 0));
				Add(new GenericBuyInfo(typeof(DoubleAxe), 32, 20, 0xF4B, 0));
				Add(new GenericBuyInfo(typeof(ExecutionersAxe), 38, 20, 0xF45, 0));
				Add(new GenericBuyInfo(typeof(LargeBattleAxe), 43, 20, 0x13FB, 0));
				Add(new GenericBuyInfo(typeof(Pickaxe), 32, 20, 0xE86, 0));
				Add(new GenericBuyInfo(typeof(TwoHandedAxe), 42, 20, 0x1443, 0));
				Add(new GenericBuyInfo(typeof(WarAxe), 38, 20, 0x13B0, 0));
				Add(new GenericBuyInfo(typeof(Axe), 48, 20, 0xF49, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( BattleAxe ), 36 );
				 *				Add( typeof( DoubleAxe ), 16 );
				 *				Add( typeof( ExecutionersAxe ), 33 );
				 *				Add( typeof( LargeBattleAxe ), 21 );
				 *				Add( typeof( Pickaxe ), 16 );
				 *				Add( typeof( TwoHandedAxe ), 21 );
				 *				Add( typeof( WarAxe ), 19 );
				 *				Add( typeof( Axe ), 24 );
				 */
			}
		}
	}
}
