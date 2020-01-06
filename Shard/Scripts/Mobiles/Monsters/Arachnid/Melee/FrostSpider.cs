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

/* Scripts/Mobiles/Monsters/Arachnid/Melee/FrostSpider.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a frost spider corpse")]
	public class FrostSpider : BaseCreature
	{
		[Constructable]
		public FrostSpider()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a frost spider";
			Body = 20;
			BaseSoundID = 0x388;

			SetStr(76, 100);
			SetDex(126, 145);
			SetInt(36, 60);

			SetHits(46, 60);
			SetMana(0);

			SetDamage(6, 16);

			SetSkill(SkillName.MagicResist, 25.1, 40.0);
			SetSkill(SkillName.Tactics, 35.1, 50.0);
			SetSkill(SkillName.Wrestling, 50.1, 65.0);

			Fame = 775;
			Karma = -775;

			VirtualArmor = 28;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 74.7;
		}

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }

		public FrostSpider(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new SpidersSilk(7));
				PackGold(40, 75);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// // http://web.archive.org/web/20020202091536/uo.stratics.com/hunters/frostspider.shtml
					// 7 Spiders Silk
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
						PackItem(new SpidersSilk(7));
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new SpidersSilk(7));
					}

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

			if (BaseSoundID == 387)
				BaseSoundID = 0x388;
		}
	}
}
