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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/SkeletalMage.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/16/04, Froste
 *      Added IOBAlignment=IOBAlignment.Undead, added the random IOB drop to loot
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a skeletal corpse")]
	public class SkeletalMage : BaseCreature
	{
		[Constructable]
		public SkeletalMage()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a skeletal mage";
			Body = 148;
			BaseSoundID = 451;
			IOBAlignment = IOBAlignment.Undead;
			ControlSlots = 3;

			SetStr(76, 100);
			SetDex(56, 75);
			SetInt(80, 100);

			SetHits(46, 60);

			SetDamage(3, 7);

			SetSkill(SkillName.EvalInt, 60.1, 70.0);
			SetSkill(SkillName.Magery, 60.1, 70.0);
			SetSkill(SkillName.MagicResist, 55.1, 70.0);
			SetSkill(SkillName.Tactics, 45.1, 60.0);
			SetSkill(SkillName.Wrestling, 45.1, 55.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 38;
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public SkeletalMage(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(60, 90);
				PackScroll(1, 5);
				PackReg(3);
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
				{	// http://web.archive.org/web/20021014235656/uo.stratics.com/hunters/skeletalmage.shtml
					// 50 to 200 Gold, Magic items, Gems, Scrolls, Bone
					if (Spawning)
					{
						PackGold(50, 200);
					}
					else
					{
						PackMagicItem(1, 1, 0.05);
						PackGem(1, .9);
						PackGem(1, .05);
						PackScroll(1, 5);
						PackScroll(1, 5, 0.05);
						PackItem(typeof(Bone), 0.8);
					}
				}
				else
				{
					if (Spawning)
					{
						PackReg(3);
						if (Core.AOS)
							PackNecroReg(3, 10);
						PackItem(new Bone());
					}

					AddLoot(LootPack.Average);
					AddLoot(LootPack.LowScrolls);
					AddLoot(LootPack.Potions);
				}
			}
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
