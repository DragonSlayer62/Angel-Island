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

/* Scripts/Mobiles/Animals/Rodents/GiantRat.cs
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
	[CorpseName("a giant rat corpse")]
	[TypeAlias("Server.Mobiles.Giantrat")]
	public class GiantRat : BaseCreature
	{
		[Constructable]
		public GiantRat()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a giant rat";
			Body = 0xD7;
			BaseSoundID = 0x188;

			SetStr(32, 74);
			SetDex(46, 65);
			SetInt(16, 30);

			SetHits(26, 39);
			SetMana(0);

			SetDamage(4, 8);

			SetSkill(SkillName.MagicResist, 25.1, 30.0);
			SetSkill(SkillName.Tactics, 29.3, 44.0);
			SetSkill(SkillName.Wrestling, 29.3, 44.0);

			Fame = 300;
			Karma = -300;

			VirtualArmor = 18;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return Core.UOAI || Core.UOAR ? 6 : Utility.Random(3) == 0 ? 6 : 0; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies | FoodType.Eggs; } }

		public GiantRat(Serial serial)
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
				{	// http://web.archive.org/web/20020313115208/uo.stratics.com/hunters/giantrat.shtml
					// 1 Raw Ribs (carved), 6 Hides (carved) (sometimes)
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
