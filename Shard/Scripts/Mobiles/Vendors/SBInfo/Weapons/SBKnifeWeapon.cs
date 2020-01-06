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
	public class SBKnifeWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBKnifeWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(ButcherKnife), 21, 20, 0x13F6, 0));
				Add(new GenericBuyInfo(typeof(Cleaver), 24, 20, 0xEC3, 0));
				Add(new GenericBuyInfo(typeof(Dagger), 33, 20, 0xF52, 0));
				Add(new GenericBuyInfo(typeof(SkinningKnife), 26, 20, 0xEC4, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( ButcherKnife ), 10 );
				 *				Add( typeof( Cleaver ), 12 );
				 *				Add( typeof( Dagger ), 21 );
				 *				Add( typeof( SkinningKnife ), 13 );
				 */
			}
		}
	}
}
