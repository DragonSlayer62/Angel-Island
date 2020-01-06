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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/BoneMagiLord.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/19/04, Adam
 *		1. Create from BoneMagi.cs
 *		2. stats and loot based on lich
 */

using System;
using Server;
using Server.Items;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a bone magi lord corpse")]
	public class BoneMagiLord : BaseCreature
	{
		[Constructable]
		public BoneMagiLord()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a bone magi lord";
			Body = 148;
			BaseSoundID = 451;
			IOBAlignment = IOBAlignment.Undead;
			ControlSlots = 3;

			SetStr(171, 200);
			SetDex(126, 145);
			SetInt(276, 305);

			SetHits(103, 120);

			SetDamage(24, 26);

			SetSkill(SkillName.EvalInt, 100.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.Meditation, 85.1, 95.0);
			SetSkill(SkillName.MagicResist, 80.1, 100.0);
			SetSkill(SkillName.Tactics, 70.1, 90.0);

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 50;
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackScroll(1, 5);
				PackReg(3);
				PackItem(new Bone(Utility.Random(10, 12)));

				PackGold(170, 220);
				PackMagicEquipment(1, 2, 0.25, 0.25);
				PackMagicEquipment(1, 2, 0.05, 0.05);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// ai special creature
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					AddLoot(LootPack.Average);
					AddLoot(LootPack.LowScrolls);
					AddLoot(LootPack.Potions);
				}
			}
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public BoneMagiLord(Serial serial)
			: base(serial)
		{
		}

		public override bool OnBeforeDeath()
		{
			return base.OnBeforeDeath();
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
