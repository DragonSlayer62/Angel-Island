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

/* Scripts/Mobiles/Monsters/Plant/Melee/SwampTentacle.cs
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
	[CorpseName("a swamp tentacle corpse")]
	public class SwampTentacle : BaseCreature
	{
		[Constructable]
		public SwampTentacle()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a swamp tentacle";
			Body = 66;
			BaseSoundID = 352;

			SetStr(96, 120);
			SetDex(66, 85);
			SetInt(16, 30);

			SetHits(58, 72);
			SetMana(0);

			SetDamage(6, 12);

			SetSkill(SkillName.MagicResist, 15.1, 20.0);
			SetSkill(SkillName.Tactics, 65.1, 80.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 28;
		}

		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public SwampTentacle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackReg(3);
				PackGold(60, 90);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020223144930/uo.stratics.com/hunters/swamptentacles.shtml
					// 50 to 150 Gold, 3 Garlic
					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackItem(new Garlic(3));
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
						PackReg(3);

					AddLoot(LootPack.Average);
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
