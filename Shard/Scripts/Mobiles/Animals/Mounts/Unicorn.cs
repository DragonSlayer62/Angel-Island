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

/* Scripts/Mobiles/Animals/Mounts/Kirin.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  3/23/05, Kit 
 * 		Added VirtualArmor value of 60
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a unicorn corpse")]
	public class Unicorn : BaseMount
	{
		public override bool AllowMaleRider { get { return false; } }
		public override bool AllowMaleTamer { get { return false; } }

		public override bool InitialInnocent { get { return true; } }

		public override void OnDisallowedRider(Mobile m)
		{
			m.SendLocalizedMessage(1042318); // The unicorn refuses to allow you to ride it.
		}

		[Constructable]
		public Unicorn()
			: this("a unicorn")
		{
		}

		[Constructable]
		public Unicorn(string name)
			: base(name, 0x7A, 0x3EB4, AIType.AI_Mage, FightMode.Aggressor | FightMode.Evil, 10, 1, 0.25, 0.5)
		{
			BaseSoundID = 0x4BC;

			SetStr(296, 325);
			SetDex(96, 115);
			SetInt(186, 225);

			SetHits(191, 210);

			SetDamage(16, 22);

			SetSkill(SkillName.EvalInt, 80.1, 90.0);
			SetSkill(SkillName.Magery, 60.2, 80.0);
			SetSkill(SkillName.Meditation, 50.1, 60.0);
			SetSkill(SkillName.MagicResist, 75.3, 90.0);
			SetSkill(SkillName.Tactics, 20.1, 22.5);
			SetSkill(SkillName.Wrestling, 80.5, 92.5);
			VirtualArmor = 60;
			Fame = 9000;
			Karma = 9000;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 95.1;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int Meat { get { return 3; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Unicorn(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(175, 225);
				PackScroll(1, 3);
				PackPotion();
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20011205233902/uo.stratics.com/hunters/unicorn.shtml
					// 200 - 400 Gold, scrolls, potions, gems, magic item (rarely)
					if (Spawning)
					{
						PackGold(200, 400);
					}
					else
					{
						PackScroll(1, 7);
						PackScroll(1, 7, 0.05);
						PackPotion();
						PackGem(1, .9);
						PackGem(1, .05);
						PackMagicItem(1, 1, 0.05);
					}
				}
				else
				{
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.LowScrolls);
					AddLoot(LootPack.Potions);
				}
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
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
