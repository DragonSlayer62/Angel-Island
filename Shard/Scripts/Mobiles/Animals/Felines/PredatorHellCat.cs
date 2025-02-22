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

/* Scripts/Mobiles/Animals/Felines/PredatorHellCat.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a hell cat corpse")]
	[TypeAlias("Server.Mobiles.Preditorhellcat")]
	public class PredatorHellCat : BaseCreature
	{
		[Constructable]
		public PredatorHellCat()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a hell cat";
			Body = 127;
			BaseSoundID = 0xBA;

			SetStr(161, 185);
			SetDex(96, 115);
			SetInt(76, 100);

			SetHits(97, 131);

			SetDamage(5, 17);

			SetSkill(SkillName.MagicResist, 75.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 65.0);
			SetSkill(SkillName.Wrestling, 50.1, 65.0);

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 89.1;
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202090932/uo.stratics.com/hunters/hellcatlarge.shtml
					// Loot: none
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
					AddLoot(LootPack.Average);
				}
			}
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Feline; } }

		public PredatorHellCat(Serial serial)
			: base(serial)
		{
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
