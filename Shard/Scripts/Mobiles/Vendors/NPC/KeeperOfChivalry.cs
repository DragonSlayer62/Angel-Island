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
using Server;
using Server.Items;

namespace Server.Mobiles
{
	public class KeeperOfChivalry : BaseVendor
	{
		private ArrayList m_SBInfos = new ArrayList();
		protected override ArrayList SBInfos { get { return m_SBInfos; } }

		[Constructable]
		public KeeperOfChivalry()
			: base("the Keeper of Chivalry")
		{
			SetSkill(SkillName.Fencing, 75.0, 85.0);
			SetSkill(SkillName.Macing, 75.0, 85.0);
			SetSkill(SkillName.Swords, 75.0, 85.0);
			SetSkill(SkillName.Chivalry, 100.0);
		}

		public override void InitSBInfo()
		{
			m_SBInfos.Add(new SBKeeperOfChivalry());
		}

		public override void InitOutfit()
		{
			AddItem(new PlateArms());
			AddItem(new PlateChest());
			AddItem(new PlateGloves());
			AddItem(new StuddedGorget());
			AddItem(new PlateLegs());

			switch (Utility.Random(4))
			{
				case 0: AddItem(new PlateHelm()); break;
				case 1: AddItem(new NorseHelm()); break;
				case 2: AddItem(new CloseHelm()); break;
				case 3: AddItem(new Helmet()); break;
			}

			switch (Utility.Random(3))
			{
				case 0: AddItem(new BodySash(0x482)); break;
				case 1: AddItem(new Doublet(0x482)); break;
				case 2: AddItem(new Tunic(0x482)); break;
			}

			AddItem(new Broadsword());

			Item shield = new MetalKiteShield();

			shield.Hue = Utility.RandomNondyedHue();

			AddItem(shield);

			switch (Utility.Random(2))
			{
				case 0: AddItem(new Boots()); break;
				case 1: AddItem(new ThighBoots()); break;
			}

			PackGold(100, 200);
		}

		public KeeperOfChivalry(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}