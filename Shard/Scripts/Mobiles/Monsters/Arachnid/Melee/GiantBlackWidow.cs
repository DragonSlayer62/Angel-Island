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

/* Scripts/Mobiles/Monsters/Arachnid/Melee/GiantBlackWidow.cs
 * ChangeLog
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
	[CorpseName("a giant black widow spider corpse")] // stupid corpse name
	public class GiantBlackWidow : BaseCreature
	{
		[Constructable]
		public GiantBlackWidow()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a giant black widow";
			Body = 0x9D;
			BaseSoundID = 0x388; // TODO: validate

			SetStr(76, 100);
			SetDex(96, 115);
			SetInt(36, 60);

			SetHits(46, 60);

			SetDamage(5, 17);

			SetSkill(SkillName.Anatomy, 30.3, 75.0);
			SetSkill(SkillName.Poisoning, 60.1, 80.0);
			SetSkill(SkillName.MagicResist, 45.1, 60.0);
			SetSkill(SkillName.Tactics, 65.1, 80.0);
			SetSkill(SkillName.Wrestling, 70.1, 85.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 24;
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }

		public GiantBlackWidow(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new SpidersSilk(5));
				PackGold(60, 90);
				PackItem(new LesserPoisonPotion());
				PackItem(new LesserPoisonPotion());
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20021015000846/uo.stratics.com/hunters/blackwidow.shtml
					// 5 Spider Silk, 2 Poison Potions

					if (Spawning)
					{
						// no gold
					}
					else
					{
						PackItem(new SpidersSilk(5));
						PackItem(new PoisonPotion(2));
					}
				}
				else
				{
					if (Spawning)
						PackItem(new SpidersSilk(7));

					AddLoot(LootPack.Meager);
					AddLoot(LootPack.Poor);
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
