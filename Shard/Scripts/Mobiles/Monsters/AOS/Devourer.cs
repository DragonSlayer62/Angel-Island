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

/* Scripts/Mobiles/Monsters/AOS/Devourer.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a devourer of souls corpse")]
	public class Devourer : BaseCreature
	{
		[Constructable]
		public Devourer()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a devourer of souls";
			Body = 303;
			BaseSoundID = 357;

			SetStr(801, 950);
			SetDex(126, 175);
			SetInt(201, 250);

			SetHits(650);

			SetDamage(22, 26);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 90.1, 105.0);
			SetSkill(SkillName.Tactics, 75.1, 85.0);
			SetSkill(SkillName.Wrestling, 80.1, 100.0);

			Fame = 9500;
			Karma = -9500;

			VirtualArmor = 44;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		//Pix - do we want this or not?  It was taken out of RunUO 1.0RC0
		//public override int TreasureMapLevel { get { return Core.AngelIsland ? 4 : 0; } }

		public override int Meat { get { return 3; } }

		public Devourer(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(500, 600);
			PackMagicEquipment(1, 3, 0.40, 0.40);
			// Category 3 MID
			PackMagicItem(1, 2, 0.10);
			PackMagicItem(1, 2, 0.05);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
