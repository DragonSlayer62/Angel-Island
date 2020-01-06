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

/* Scripts/Mobiles/Vendors/SBInfo/SBHouseDeed.cs
 * CHANGELOG:
 *  6/12/07, adam
 *      add check for TestCenter.Enabled == true before adding Static houses for sale.
 *      We don't want this on until we have checked in a valid StaticHousing*.xml
 *	6/11/07 - Pix
 *		Added our static house deeds!
 *	11/22/06 - Pix
 *		Added missing TwoStoryStonePlasterHouseDeed
 */

using System;
using System.Collections;
using Server.Multis.Deeds;
using Server.Misc;              // test center

namespace Server.Mobiles
{
	public class SBHouseDeed : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBHouseDeed()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("deed to a stone and plaster house", typeof(StonePlasterHouseDeed), StonePlasterHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a field stone house", typeof(FieldStoneHouseDeed), FieldStoneHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a wooden house", typeof(WoodHouseDeed), WoodHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a wood and plaster house", typeof(WoodPlasterHouseDeed), WoodPlasterHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a thatched roof cottage", typeof(ThatchedRoofCottageDeed), ThatchedRoofCottageDeed.m_price, 20, 0x14F0, 0));

				Add(new GenericBuyInfo("deed to a small brick house", typeof(SmallBrickHouseDeed), SmallBrickHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a small stone workshop", typeof(StoneWorkshopDeed), StoneWorkshopDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a small marble workshop", typeof(MarbleWorkshopDeed), MarbleWorkshopDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a small stone tower", typeof(SmallTowerDeed), SmallTowerDeed.m_price, 20, 0x14F0, 0));

				Add(new GenericBuyInfo("deed to a sandstone house with patio", typeof(SandstonePatioDeed), SandstonePatioDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a large house with patio", typeof(LargePatioDeed), LargePatioDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a marble house with patio", typeof(LargeMarbleDeed), LargeMarbleDeed.m_price, 20, 0x14F0, 0));

				Add(new GenericBuyInfo("deed to a brick house", typeof(BrickHouseDeed), BrickHouseDeed.m_price, 20, 0x14F0, 0));

				Add(new GenericBuyInfo("deed to a two-story log cabin", typeof(LogCabinDeed), LogCabinDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a two-story wood and plaster house", typeof(TwoStoryWoodPlasterHouseDeed), TwoStoryWoodPlasterHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a two-story stone and plaster house", typeof(TwoStoryStonePlasterHouseDeed), TwoStoryStonePlasterHouseDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a two-story villa", typeof(VillaDeed), VillaDeed.m_price, 20, 0x14F0, 0));

				Add(new GenericBuyInfo("deed to a tower", typeof(TowerDeed), TowerDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a small stone keep", typeof(KeepDeed), KeepDeed.m_price, 20, 0x14F0, 0));
				Add(new GenericBuyInfo("deed to a castle", typeof(CastleDeed), CastleDeed.m_price, 20, 0x14F0, 0));

				if (Core.UOAI || Core.UOAR || Core.UOMO)
				{
					System.Collections.Generic.List<Server.Multis.StaticHousing.StaticHouseDescription> shList = Server.Multis.StaticHousing.StaticHouseHelper.GetAllStaticHouseDescriptions();
					foreach (Server.Multis.StaticHousing.StaticHouseDescription shd in shList)
					{
						//Server.Multis.StaticHousing.StaticDeed
						Add(new GenericBuyInfo("deed to a " + shd.Description,
								typeof(Server.Multis.StaticHousing.StaticDeed),
								shd.Price,
								20,
								0,
								0,
								0x14F0,
								0,
								new object[] { shd.ID, shd.Description }
								)
							);
					}
				}
			}
		}

		public class InternalSellInfo : GenericSellInfo
		{
			public InternalSellInfo()
			{
				if (!Core.UOAI && !Core.UOAR && !Core.UOSP && !Core.UOMO)
				{	// cash buyback
					Add(typeof(StonePlasterHouseDeed), StonePlasterHouseDeed.m_price);
					Add(typeof(FieldStoneHouseDeed), FieldStoneHouseDeed.m_price);
					Add(typeof(SmallBrickHouseDeed), SmallBrickHouseDeed.m_price);
					Add(typeof(WoodHouseDeed), WoodHouseDeed.m_price);
					Add(typeof(WoodPlasterHouseDeed), WoodPlasterHouseDeed.m_price);
					Add(typeof(ThatchedRoofCottageDeed), ThatchedRoofCottageDeed.m_price);
					Add(typeof(BrickHouseDeed), BrickHouseDeed.m_price);
					Add(typeof(TwoStoryWoodPlasterHouseDeed), TwoStoryWoodPlasterHouseDeed.m_price);
					Add(typeof(TowerDeed), TowerDeed.m_price);
					Add(typeof(KeepDeed), KeepDeed.m_price);
					Add(typeof(CastleDeed), CastleDeed.m_price);
					Add(typeof(LargePatioDeed), LargePatioDeed.m_price);
					Add(typeof(LargeMarbleDeed), LargeMarbleDeed.m_price);
					Add(typeof(SmallTowerDeed), SmallTowerDeed.m_price);
					Add(typeof(LogCabinDeed), LogCabinDeed.m_price);
					Add(typeof(SandstonePatioDeed), SandstonePatioDeed.m_price);
					Add(typeof(VillaDeed), VillaDeed.m_price);
					Add(typeof(StoneWorkshopDeed), StoneWorkshopDeed.m_price);
					Add(typeof(MarbleWorkshopDeed), MarbleWorkshopDeed.m_price);
					Add(typeof(SmallBrickHouseDeed), SmallBrickHouseDeed.m_price);
				}
			}
		}
	}
}
