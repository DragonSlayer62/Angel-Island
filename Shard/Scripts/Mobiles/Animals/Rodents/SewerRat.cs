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

/* Scripts/Mobiles/Animals/Rodents/SewerRat.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 4 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a sewer rat corpse")]
	public class Sewerrat : BaseCreature
	{
		[Constructable]
		public Sewerrat()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a sewer rat";
			Body = 238;
			BaseSoundID = 0xCC;

			SetStr(9);
			SetDex(25);
			SetInt(6, 10);

			SetHits(6);
			SetMana(0);

			SetDamage(1, 2);

			SetSkill(SkillName.MagicResist, 5.0);
			SetSkill(SkillName.Tactics, 5.0);
			SetSkill(SkillName.Wrestling, 5.0);

			Fame = 300;
			Karma = -300;

			VirtualArmor = 6;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -0.9;
		}

		public override int Meat { get { return 1; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Eggs | FoodType.FruitsAndVegies; } }

		public Sewerrat(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(0, 25);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// Note: Stratics folds the rat and sewer rat into one page .. is the loot *really* the same?
					// http://web.archive.org/web/20020202091202/uo.stratics.com/hunters/rat.shtml
					// 	1 Raw Ribs (carved)

					// okay, 2003 has the sewer rat listed with no loot. We'll go with that
					// http://web.archive.org/web/20030210154510/uo.stratics.com/hunters/sewerrat.shtml
					// 	1 Raw Ribs (carved)

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
