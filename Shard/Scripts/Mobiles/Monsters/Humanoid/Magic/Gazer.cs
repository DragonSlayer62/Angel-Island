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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/Gazer.cs
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
	[CorpseName("a gazer corpse")]
	public class Gazer : BaseCreature
	{
		[Constructable]
		public Gazer()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a gazer";
			Body = 22;
			BaseSoundID = 377;

			SetStr(96, 125);
			SetDex(86, 105);
			SetInt(141, 165);

			SetHits(58, 75);

			SetDamage(5, 10);

			SetSkill(SkillName.EvalInt, 50.1, 65.0);
			SetSkill(SkillName.Magery, 50.1, 65.0);
			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 50.1, 70.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 36;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override int Meat { get { return 1; } }

		public Gazer(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new Nightshade(4));
				PackGold(50, 100);
				PackPotion();
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20011217230049/uo.stratics.com/hunters/gazer.shtml
					// 50 to 150 Gold, Potions, Arrows, Scrolls, Nightshade, Magic Items, 1 Raw Ribs (carved)
					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion();
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));
						PackScroll(1, 6);
						PackItem(new Nightshade(4));
						PackMagicItem(1, 1, 0.05);
					}
				}
				else
				{
					if (Spawning)
						PackItem(new Nightshade(4));

					AddLoot(LootPack.Average);
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
