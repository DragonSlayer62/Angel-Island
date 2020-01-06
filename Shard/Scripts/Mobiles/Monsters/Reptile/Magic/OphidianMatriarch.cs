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

/* Scripts/Mobiles/Monsters/Reptile/Magic/OphidianMatriarch.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
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
	[CorpseName("an ophidian corpse")]
	public class OphidianMatriarch : BaseCreature
	{
		[Constructable]
		public OphidianMatriarch()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "an ophidian matriarch";
			Body = 87;
			BaseSoundID = 644;

			SetStr(416, 505);
			SetDex(96, 115);
			SetInt(366, 455);

			SetHits(250, 303);

			SetDamage(11, 13);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.Meditation, 5.4, 25.0);
			SetSkill(SkillName.MagicResist, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 16000;
			Karma = -16000;

			VirtualArmor = 50;
		}

		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 4 : 0; } }
		public override int Meat { get { return 2; } }

		public OphidianMatriarch(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(300, 400);
				PackPotion();
				PackScroll(3, 7);
				PackMagicEquipment(1, 2, 0.20, 0.20);
				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020414130610/uo.stratics.com/hunters/ophmat.shtml
					// 200 to 250 Gold, Potions, Arrows, Gems, Scrolls (circles 3 to 7)
					if (Spawning)
					{
						PackGold(200, 250);
					}
					else
					{
						PackPotion(0.9);
						PackPotion(0.6);
						PackPotion(0.2);
						PackItem(new Arrow(Utility.Random(1, 4)));
						PackGem(1, .9);
						PackGem(1, .6);
						PackGem(1, .2);
						PackScroll(3, 7);
						PackScroll(3, 7, 0.8);
						PackScroll(3, 7, 0.3);
					}
				}
				else
				{	// Standard RunUO
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average, 2);
					AddLoot(LootPack.MedScrolls, 2);
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
