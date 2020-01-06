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

/* Scripts/Mobiles/Monsters/Elemental/Melee/EarthElemental.cs
 * ChangeLog
 *	7/16/10, adam
 *		o decrease average dex
 *		o decrease average int
 *		o increase average hp
 *		o decrease average damage
 *		o increase virtual armor
 *		o increase average wrestling
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	4/27/05, Kit
 *		Adjusted dispell difficulty
 *  10/3/04, Jade
 *      Added fertile dirt as loot.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("an earth elemental corpse")]
	public class EarthElemental : BaseCreature
	{
		public override double DispelDifficulty { get { return 56; } }
		public override double DispelFocus { get { return 45.0; } }

		[Constructable]
		public EarthElemental()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "an earth elemental";
			Body = 14;
			BaseSoundID = 268;

			SetStr(126, 155);
			SetDex(66, 85);
			SetInt(71, 92);
			SetHits(76, 93);
			SetDamage(9, 16);

			SetSkill(SkillName.MagicResist, 50.1, 95.0);
			SetSkill(SkillName.Tactics, 60.1, 100.0);
			SetSkill(SkillName.Wrestling, 60.1, 100.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 50;
			ControlSlots = 2;
		}

		public EarthElemental(bool summoned)
			: this()
		{
			if (summoned == true)
			{
				SetStr(126, 155);
				SetDex(50 - 10, 50 + 10);
				SetInt(50 - 10, 50 + 10);
				SetHits(150 - 10, 150 + 10);
				SetDamage(12 - 1, 12 + 1);

				SetSkill(SkillName.MagicResist, 85 - 10, 85 + 10);
				SetSkill(SkillName.Tactics, 60.1, 100.0);
				SetSkill(SkillName.Wrestling, 90 - 10, 90 + 10);

				VirtualArmor = 50;
			}
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public EarthElemental(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				// Jade: Add FertileDirt
				PackItem(new FertileDirt(Utility.RandomMinMax(15, 30)));
				PackItem(new IronOre(5)); // TODO: Five small iron ore
				PackGold(100, 150);
				PackItem(new MandrakeRoot());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202092911/uo.stratics.com/hunters/earthelemental.shtml
					// 200 - 350 Gold, Gems, Ore, Fertile Dirt reagent

					if (Spawning)
					{
						PackGold(200, 350);
					}
					else
					{
						PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));
						PackItem(new IronOre(3));
					}
				}
				else
				{	// standard runuo
					if (Spawning)
					{
						PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));
						PackItem(new IronOre(3)); // TODO: Five small iron ore
						PackItem(new MandrakeRoot());
					}

					AddLoot(LootPack.Average);
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.Gems);
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
