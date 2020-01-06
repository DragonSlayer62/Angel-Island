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

/* Scripts/Mobiles/Vendors/SBInfo/SBMagiSupplier.cs
 * ChangeLog
 *	2/10/05, Adam
 *		Restore Moonstones
 *	2/6/05, Adam
 *		Remove Moonstones temporarily
 *  1/23/05, Froste
 *      Modified version of SBOrcMerchant.cs
 *  10/18/04, Froste
 *      Added a MinValue param equal to the amount param
 *	10/18/04, Adam
 *		Reduce bulk prices a tad, and increase quantities
 *  10/17/04, Froste
 *      Modified version of SBImporter.cs
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	5/22/04, mith
 *		Modified so that Mages only sell up to level 3 scrolls.
 *	4/24/04, mith
 *		Commented all items from SellList so that NPCs don't buy from players.
 */

using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
	public class SBMagiSupplier : SBInfo
	{
		private ArrayList m_BuyInfo = new InternalBuyInfo();
		private IShopSellInfo m_SellInfo = new InternalSellInfo();

		public SBMagiSupplier()
		{
		}

		public override IShopSellInfo SellInfo { get { return m_SellInfo; } }
		public override ArrayList BuyInfo { get { return m_BuyInfo; } }

		public class InternalBuyInfo : ArrayList
		{
			public InternalBuyInfo()
			{
				Add(new GenericBuyInfo("Arms of the Bone Magi", typeof(BoneMagiArms), 360, 20, 0x144E, 0));
				Add(new GenericBuyInfo("Armor of the Bone Magi", typeof(BoneMagiArmor), 500, 20, 0x144F, 0));
				Add(new GenericBuyInfo("Gloves of the Bone Magi", typeof(BoneMagiGloves), 270, 20, 0x1450, 0));
				Add(new GenericBuyInfo("Legs of the Bone Magi", typeof(BoneMagiLegs), 360, 20, 0x1452, 0));
				Add(new GenericBuyInfo("Helm of the Bone Magi", typeof(BoneMagiHelm), 45, 20, 0x1451, 0));
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