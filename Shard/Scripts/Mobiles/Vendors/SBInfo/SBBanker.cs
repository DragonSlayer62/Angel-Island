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
/* Scripts/Mobiles/Vendors/SBInfo/SBBanker.cs
 * ChangeLog
 *	04/19/05, Kit
 *		Added  VendorRentalContract
 *	05/02/05 TK
 *		Added Account Book
 * */


using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBBanker : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBBanker()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO || (Core.UOSP && Core.Publish >= 13.5))
					Add(new GenericBuyInfo("1047016", typeof(CommodityDeed), 5, 20, 0x14F0, 0x47));

				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{
					Add(new GenericBuyInfo("1041243", typeof(ContractOfEmployment), 1025, 20, 0x14F0, 0));
					Add(new GenericBuyInfo("account book", typeof(AccountBook), 150, 10, 0xFF1, 0));
					Add(new GenericBuyInfo("vendor rental contract", typeof(VendorRentalContract), 1025, 20, 0x14F0, 0));
					Add(new GenericBuyInfo("certificate of identity", typeof(CertificateOfIdentity), 180, 20, 0x14F0, 0));
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
			}
		}
	}
}