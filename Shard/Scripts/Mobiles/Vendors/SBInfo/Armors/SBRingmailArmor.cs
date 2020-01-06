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
	public class SBRingmailArmor : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBRingmailArmor()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(RingmailArms), 122, 20, 0x13EE, 0));
				Add(new GenericBuyInfo(typeof(RingmailChest), 218, 20, 0x13ec, 0));
				Add(new GenericBuyInfo(typeof(RingmailGloves), 122, 20, 0x13eb, 0));
				Add(new GenericBuyInfo(typeof(RingmailLegs), 147, 20, 0x13F0, 0));
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				/*				Add( typeof( RingmailArms ), 61 );
				 *				Add( typeof( RingmailChest ), 109 );
				 *				Add( typeof( RingmailGloves ), 61 );
				 *				Add( typeof( RingmailLegs ), 73 );
				 */
			}
		}
	}
}
