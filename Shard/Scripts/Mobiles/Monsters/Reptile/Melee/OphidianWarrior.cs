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

/* Scripts/Mobiles/Monsters/Reptile/Melee/OphidianWarrior.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("an ophidian corpse")]
	public class OphidianWarrior : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian warrior",
				"an ophidian enforcer"
			};

		[Constructable]
		public OphidianWarrior()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = m_Names[Utility.Random(m_Names.Length)];
			Body = 86;
			BaseSoundID = 634;

			SetStr(150, 320);
			SetDex(94, 190);
			SetInt(64, 160);

			SetHits(128, 155);
			SetMana(0);

			SetDamage(5, 11);

			SetSkill(SkillName.MagicResist, 70.1, 85.0);
			SetSkill(SkillName.Swords, 60.1, 85.0);
			SetSkill(SkillName.Tactics, 75.1, 90.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 36;
		}

		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public OphidianWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(100, 140);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020213035113/uo.stratics.com/hunters/ophwarr.shtml
					// 50 to 150 Gold, Potions, Arrows, Gems, Weapons
					// http://web.archive.org/web/20020414104242/uo.stratics.com/hunters/ophenfor.shtml
					// 50 to 150 Gold, Potions, Arrows, Gems, Weapons
					// (same creature)
					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion(0.9);
						PackPotion(0.6);

						PackItem(new Arrow(Utility.Random(1, 4)));

						PackGem(1, .9);
						PackGem(1, .05);

						PackMagicEquipment(1, 2, 0.00, 0.25);	// no chance at armor, 25% (12.5% actual) at low end magic weapon
					}
				}
				else
				{	// Standard RunUO
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.Average);
					AddLoot(LootPack.Gems);
				}
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.TerathansAndOphidians; }
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
