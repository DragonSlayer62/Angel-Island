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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/StrongMongbat.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a mongbat corpse")]
	public class StrongMongbat : BaseCreature
	{
		[Constructable]
		public StrongMongbat()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a mongbat";
			Body = 39;
			BaseSoundID = 422;

			SetStr(6, 10);
			SetDex(26, 38);
			SetInt(6, 14);

			SetHits(4, 6);
			SetMana(0);

			SetDamage(5, 7);

			SetSkill(SkillName.MagicResist, 15.1, 30.0);
			SetSkill(SkillName.Tactics, 35.1, 50.0);
			SetSkill(SkillName.Wrestling, 20.1, 35.0);

			Fame = 150;
			Karma = -150;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 71.1;
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 6; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public StrongMongbat(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(1, 25);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020210133039/uo.stratics.com/hunters/mongbat.shtml
					// 1 Raw Ribs (carved), 6 Hides (carved) off of stronger Mongbat. The stronger version also give 0 to 50 Gold.

					if (Spawning)
					{
						PackGold(0, 50);
					}
					else
					{
						// no other lootz
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
