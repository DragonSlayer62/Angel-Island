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

/* Scripts/Mobiles/Monsters/Reptile/Melee/Harpy.cs
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

namespace Server.Mobiles
{
	[CorpseName("a harpy corpse")]
	public class Harpy : BaseCreature
	{
		[Constructable]
		public Harpy()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a harpy";
			Body = 30;
			BaseSoundID = 402;

			SetStr(96, 120);
			SetDex(86, 110);
			SetInt(51, 75);

			SetHits(58, 72);

			SetDamage(5, 7);

			SetSkill(SkillName.MagicResist, 50.1, 65.0);
			SetSkill(SkillName.Tactics, 70.1, 100.0);
			SetSkill(SkillName.Wrestling, 60.1, 90.0);

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 28;
		}

		public override int GetAttackSound()
		{
			return 916;
		}

		public override int GetAngerSound()
		{
			return 916;
		}

		public override int GetDeathSound()
		{
			return 917;
		}

		public override int GetHurtSound()
		{
			return 919;
		}

		public override int GetIdleSound()
		{
			return 918;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Meat { get { return Core.UOAI || Core.UOAR ? 4 : 2; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 50; } }

		public Harpy(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(50, 100);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020403074327/uo.stratics.com/hunters/harpy.shtml
					// 50 to 150 Gold, 2 raw bird (carved), 50 Feathers (carved)

					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						// no lootz
					}
				}
				else
				{
					AddLoot(LootPack.Meager, 2);
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
