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
	public class SBStavesWeapon : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBStavesWeapon()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(BlackStaff), 27, 20, 0xDF1, 0));
				Add(new GenericBuyInfo(typeof(GnarledStaff), 24, 20, 0x13F8, 0));
				Add(new GenericBuyInfo(typeof(QuarterStaff), 30, 20, 0xE89, 0));
				Add(new GenericBuyInfo(typeof(ShepherdsCrook), 24, 20, 0xE81, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( BlackStaff ), 24 );
				 *				Add( typeof( GnarledStaff ), 12 );
				 *				Add( typeof( QuarterStaff ), 15 );
				 *				Add( typeof( ShepherdsCrook ), 12 );
				 */
			}
		}
	}
}
