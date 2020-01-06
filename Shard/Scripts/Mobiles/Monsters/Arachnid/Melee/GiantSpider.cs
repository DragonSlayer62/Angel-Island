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

/* Scripts/Mobiles/Monsters/Arachnid/Melee/GiantSpider.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 3 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;
using Server.Targeting;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName("a giant spider corpse")]
	public class GiantSpider : BaseCreature
	{
		[Constructable]
		public GiantSpider()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a giant spider";
			Body = 28;
			BaseSoundID = 0x388;

			SetStr(76, 100);
			SetDex(76, 95);
			SetInt(36, 60);

			SetHits(46, 60);
			SetMana(0);

			SetDamage(5, 13);

			SetSkill(SkillName.Poisoning, 60.1, 80.0);
			SetSkill(SkillName.MagicResist, 25.1, 40.0);
			SetSkill(SkillName.Tactics, 35.1, 50.0);
			SetSkill(SkillName.Wrestling, 50.1, 65.0);

			Fame = 600;
			Karma = -600;

			VirtualArmor = 16;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 59.1;
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override Poison HitPoison { get { return Poison.Regular; } }

		public GiantSpider(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new SpidersSilk(5));
				PackGold(0, 25);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020212101250/uo.stratics.com/hunters/giantspider.shtml
					// None
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
					if (Spawning)
						PackItem(new SpidersSilk(5));

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
