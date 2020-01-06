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

/* Scripts/Mobiles/Vendors/SBInfo/SBScribe.cs
 * ChangeLog
 *  10/14/04, Froste
 *      Changed the amount argument to GenericBuyInfo from 999 to 20 for reagents, so the argument means something in GenericBuy.cs
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBScribe : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBScribe()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo(typeof(ScribesPen), 8, 20, 0xFBF, 0));
				Add(new GenericBuyInfo(typeof(BrownBook), 15, 10, 0xFEF, 0));
				Add(new GenericBuyInfo(typeof(TanBook), 15, 10, 0xFF0, 0));
				Add(new GenericBuyInfo(typeof(BlueBook), 15, 10, 0xFF2, 0));
				Add(new GenericBuyInfo(typeof(BlankScroll), 5, 20, 0x0E34, 0));
				Add(new GenericBuyInfo(typeof(Spellbook), 18, 10, 0xEFA, 0));
				Add(new GenericBuyInfo(typeof(RecallRune), 15, 10, 0x1F14, 0));
				//Add( new GenericBuyInfo( "1041267", typeof( Runebook ), 3500, 10, 0xEFA, 0x461 ) );
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(ScribesPen), 4);
					Add(typeof(BrownBook), 7);
					Add(typeof(TanBook), 7);
					Add(typeof(BlueBook), 7);
					Add(typeof(BlankScroll), 3);
					Add(typeof(Spellbook), 9);
					Add(typeof(RecallRune), 8);
				}
			}
		}
	}
}