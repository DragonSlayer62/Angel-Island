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

/* Scripts/Mobiles/Vendors/SBInfo/SBGypsyTrader.cs
 * ChangeLog
 *	9/19/06, Pix
 *		Added TeleporterAddonDyeTub for sale for 15K
 *	9/13/06, Pix
 *		Removed old TeleporterDeed and replaced it with TeleporterAddonDeed
 *	3/29/06 Taran Kain
 *		Add NPCNameChangeDeed for 4000
 *	3/27/05, Kitaras
 *		Add the NpcTitleChangeDeed for sale for 1500
 *	3/15/05, Adam
 *		Add the OrcishBodyDeed for sale for 1500
 *	11/16/04, Froste
 *      Created from SBCarpenter.cs
 */

using System;
using System.Collections;
using Server.Items;


namespace Server.Mobiles
{
	public class SBGypsyTrader : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBGypsyTrader()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{	
					Add(new GenericBuyInfo("Gender Change Deed", typeof(GenderChangeDeed), 100000, 20, 0x14F0, 0x0));
					Add(new GenericBuyInfo("Name Change Deed", typeof(NameChangeDeed), 100000, 20, 0x14F0, 0x0));
					//Add(new GenericBuyInfo("Teleporter Deed", typeof(TeleporterDeed), 500000, 20, 0x14F0, 0x0));
					Add(new GenericBuyInfo("Teleporter Addon Deed", typeof(TeleporterAddonDeed), 500000, 20, 0x14F0, 0x0));
					Add(new GenericBuyInfo("Teleporter Dye Tub", typeof(TeleporterAddonDyeTub), 15000, 20, 0xFAB, 0x0));
					Add(new GenericBuyInfo("Orcish Vendor Body Deed", typeof(OrcishBodyDeed), 1500, 20, 0x14F0, 0x0));
					Add(new GenericBuyInfo("Vendor Title Change Deed", typeof(NpcTitleChangeDeed), 1500, 20, 0x14F0, 0x0));
					Add(new GenericBuyInfo("Vendor Name Change Deed", typeof(NpcNameChangeDeed), 4000, 20, 0x14F0, 0x0));
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
