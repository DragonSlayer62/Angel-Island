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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/Gargoyle.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
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
	[CorpseName("a gargoyle corpse")]
	public class Gargoyle : BaseCreature
	{
		[Constructable]
		public Gargoyle()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a gargoyle";
			Body = 4;
			BaseSoundID = 372;

			SetStr(146, 175);
			SetDex(76, 95);
			SetInt(81, 105);

			SetHits(88, 105);

			SetDamage(7, 14);

			SetSkill(SkillName.EvalInt, 70.1, 85.0);
			SetSkill(SkillName.Magery, 70.1, 85.0);
			SetSkill(SkillName.MagicResist, 70.1, 85.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 40.1, 80.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override int Meat { get { return 1; } }

		public Gargoyle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGem();
				PackGem();
				PackGem();
				PackGold(60, 100);
				PackScroll(1, 7);

				if (0.025 > Utility.RandomDouble())
					PackItem(new GargoylesPickaxe());

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20011205084450/uo.stratics.com/hunters/gargoyle.shtml
					// 50 to 150 Gold, Potions, Arrows, Scrolls, Gems, "a gargoyle's pickaxe"

					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion();
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));
						PackScroll(1, 6);
						PackGem(Utility.RandomMinMax(1, 4));
						PackItem(typeof(GargoylesPickaxe), 0.025);
					}
				}
				else
				{
					if (Spawning)
					{
						if (0.025 > Utility.RandomDouble())
							PackItem(new GargoylesPickaxe());
					}

					AddLoot(LootPack.Average);
					AddLoot(LootPack.MedScrolls);
					AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 4));
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
