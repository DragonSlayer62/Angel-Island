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

/* Scripts/Mobiles/Monsters/Reptile/Melee/Wyvern.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a wyvern corpse")]
	public class Wyvern : BaseCreature
	{
		[Constructable]
		public Wyvern()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a wyvern";
			Body = 62;
			BaseSoundID = 362;

			SetStr(202, 240);
			SetDex(153, 172);
			SetInt(51, 90);

			SetHits(125, 141);

			SetDamage(8, 19);

			SetSkill(SkillName.Poisoning, 60.1, 80.0);
			SetSkill(SkillName.MagicResist, 65.1, 80.0);
			SetSkill(SkillName.Tactics, 65.1, 90.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 40;
		}

		public override bool ReAcquireOnMovement { get { return true; } }

		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 2; } }

		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Horned; } }

		public override int GetAttackSound()
		{
			return 713;
		}

		public override int GetAngerSound()
		{
			return 718;
		}

		public override int GetDeathSound()
		{
			return 716;
		}

		public override int GetHurtSound()
		{
			return 721;
		}

		public override int GetIdleSound()
		{
			return 725;
		}

		public Wyvern(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new LesserPoisonPotion());

				PackGold(100, 130);
				PackScroll(4, 7);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// 02 2002 docs won't load
					// http://web.archive.org/web/20020806212404/uo.stratics.com/hunters/wyvern.shtml
					// 100 to 250 Gold, Potions, Scrolls (circles 4-7), Treasure Maps
					// I can't find info on the treasure map level, but RunUO says 2, so we'll use that
					if (Spawning)
					{
						PackGold(100, 250);
					}
					else
					{
						PackPotion(0.9);
						PackPotion(0.6);

						PackScroll(4, 7);
						PackScroll(4, 7, 0.8);
						PackScroll(4, 7, 0.2);
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
						PackItem(new LesserPoisonPotion());

					AddLoot(LootPack.Average);
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.MedScrolls);
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
