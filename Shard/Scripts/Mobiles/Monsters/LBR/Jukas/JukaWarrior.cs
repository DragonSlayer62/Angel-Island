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

/* Scripts/Mobiles/Monsters/LBR/Jukas/JukaWarrior.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName("a jukan corpse")]
	public class JukaWarrior : BaseCreature
	{
		[Constructable]
		public JukaWarrior()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a juka warrior";
			Body = 764;

			SetStr(251, 350);
			SetDex(61, 80);
			SetInt(101, 150);

			SetHits(151, 210);

			SetDamage(7, 9);

			SetSkill(SkillName.Anatomy, 80.1, 90.0);
			SetSkill(SkillName.Fencing, 80.1, 90.0);
			SetSkill(SkillName.Macing, 80.1, 90.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Swords, 80.1, 90.0);
			SetSkill(SkillName.Tactics, 80.1, 90.0);
			SetSkill(SkillName.Wrestling, 80.1, 90.0);

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 22;
		}

		public override int GetIdleSound()
		{
			return 0x1CD;
		}

		public override int GetAngerSound()
		{
			return 0x1CD;
		}

		public override int GetHurtSound()
		{
			return 0x1D0;
		}

		public override int GetDeathSound()
		{
			return 0x28D;
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Meat { get { return 1; } }

		public JukaWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(100, 150);
				PackGem();
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// no LBR
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{	// standard run uo
					if (Spawning)
					{
						if (Utility.RandomDouble() < 0.1)
							PackItem(new ArcaneGem());
					}

					AddLoot(LootPack.Average);
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.Gems, 1);
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
