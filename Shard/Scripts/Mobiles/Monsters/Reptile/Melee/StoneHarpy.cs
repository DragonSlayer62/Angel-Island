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

/* Scripts/Mobiles/Monsters/Reptile/Melee/StoneHarpy.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a stone harpy corpse")]
	public class StoneHarpy : BaseCreature
	{
		[Constructable]
		public StoneHarpy()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a stone harpy";
			Body = 73;
			BaseSoundID = 402;

			SetStr(296, 320);
			SetDex(86, 110);
			SetInt(51, 75);

			SetHits(178, 192);
			SetMana(0);

			SetDamage(8, 16);

			SetSkill(SkillName.MagicResist, 50.1, 65.0);
			SetSkill(SkillName.Tactics, 70.1, 100.0);
			SetSkill(SkillName.Wrestling, 70.1, 100.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 50;
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

		public override int Meat { get { return 1; } }
		public override int Feathers { get { return 50; } }

		public StoneHarpy(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGem();
				PackGold(125, 175);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020606055038/uo.stratics.com/hunters/stoneharpy.shtml
					// 400 to 500 Gold, Gems
					if (Spawning)
					{
						PackGold(400, 500);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);
					}
				}
				else
				{	// Standard RunUO
					AddLoot(LootPack.Average, 2);
					AddLoot(LootPack.Gems, 2);
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
