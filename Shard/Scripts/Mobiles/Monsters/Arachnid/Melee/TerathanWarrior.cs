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

/* Scripts/Mobiles/Monsters/Arachnid/Melee/TerathanWarrior.cs
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
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName("a terathan warrior corpse")]
	public class TerathanWarrior : BaseCreature
	{
		[Constructable]
		public TerathanWarrior()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a terathan warrior";
			Body = 70;
			BaseSoundID = 589;

			SetStr(166, 215);
			SetDex(96, 145);
			SetInt(41, 65);

			SetHits(100, 129);
			SetMana(0);

			SetDamage(7, 17);

			SetSkill(SkillName.Poisoning, 60.1, 80.0);
			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 80.1, 90.0);

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 30;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override int Meat { get { return 4; } }

		public TerathanWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(60, 90);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	//http://web.archive.org/web/20020213035914/uo.stratics.com/hunters/terwarr.shtml
					// 50 to 150 Gold, Potions, Arrows, Magic Items
					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion();
						PackPotion(.5);
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));
						PackMagicItem(1, 1, 0.05);
					}
				}
				else
				{
					AddLoot(LootPack.Average);
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
