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

/* Scripts/Mobiles/Monsters/Reptile/Melee/Scorpion.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	9/30/04, Adam
 *		Scale HitPoison (poison strength) based on poisoning skill
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a scorpion corpse")]
	public class Scorpion : BaseCreature
	{
		//private bool m_wasPoisoned = false;

		[Constructable]
		public Scorpion()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a scorpion";
			Body = 48;
			BaseSoundID = 397;

			SetStr(73, 115);
			SetDex(76, 95);
			SetInt(16, 30);

			SetHits(50, 63);
			SetMana(0);

			SetDamage(5, 10);

			SetSkill(SkillName.Poisoning, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 30.1, 35.0);
			SetSkill(SkillName.Tactics, 60.3, 75.0);
			SetSkill(SkillName.Wrestling, 50.3, 65.0);

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 28;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 47.1;
		}

		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Arachnid; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public override Poison HitPoison
		{
			get
			{
				if (Core.UOAI || Core.UOAR)
				{
					if (Skills[SkillName.Poisoning].Base == 100.0)
						return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly);
					if (Skills[SkillName.Poisoning].Base > 90.0)
						return (0.8 >= Utility.RandomDouble() ? Poison.Regular : Poison.Greater);
					if (Skills[SkillName.Poisoning].Base > 80.0)
						return (0.8 >= Utility.RandomDouble() ? Poison.Lesser : Poison.Regular);
					return (Poison.Lesser);
				}
				else
					return base.HitPoison;
			}
		}

		public Scorpion(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
				PackItem(new LesserPoisonPotion());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020207054347/uo.stratics.com/hunters/giantscorpion.shtml
					// loot: None
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
						PackItem(new LesserPoisonPotion());

					AddLoot(LootPack.Meager);
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
