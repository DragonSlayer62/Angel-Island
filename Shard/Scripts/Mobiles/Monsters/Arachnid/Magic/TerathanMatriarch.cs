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

/* Scripts/Mobiles/Monsters/Arachnid/Magic/TerathanMatriarch.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
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
	[CorpseName("a terathan matriarch corpse")]
	public class TerathanMatriarch : BaseCreature
	{
		[Constructable]
		public TerathanMatriarch()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a terathan matriarch";
			Body = 72;
			BaseSoundID = 599;

			SetStr(316, 405);
			SetDex(96, 115);
			SetInt(366, 455);

			SetHits(190, 243);

			SetDamage(11, 14);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 10000;
			Karma = -10000;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 4 : 0; } }

		public TerathanMatriarch(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(300, 400);
				PackItem(new SpidersSilk(10));
				PackScroll(1, 7);
				PackScroll(1, 7);
				PackMagicEquipment(1, 2, 0.20, 0.20);
				PackPotion();
				// Category 3 MID 
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806192318/uo.stratics.com/hunters/termat.shtml
					// 150 to 300 Gold, Magic Items, Gems, Scrolls, 5 Spiders Silk
					if (Spawning)
					{
						PackGold(150, 300);
					}
					else
					{
						if (Utility.RandomBool())
							PackMagicEquipment(1, 2);
						else
							PackMagicItem(1, 2, 0.05);

						PackGem(2, .9);
						PackGem(1, .5);
						PackScroll(1, 7);
						PackScroll(1, 7, 0.8);
						PackItem(new SpidersSilk(5));
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new SpidersSilk(5));

						if (Core.AOS)
							PackNecroReg(Utility.RandomMinMax(4, 10));
					}

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average, 2);
					AddLoot(LootPack.MedScrolls, 2);
					AddLoot(LootPack.Potions);
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
