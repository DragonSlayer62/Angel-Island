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

/* ./Scripts/Mobiles/Animals/Misc/BullFrog.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
*/

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a bull frog corpse")]
	[TypeAlias("Server.Mobiles.Bullfrog")]
	public class BullFrog : BaseCreature
	{
		[Constructable]
		public BullFrog()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a bull frog";
			Body = 81;
			Hue = Utility.RandomList(0x5AC, 0x5A3, 0x59A, 0x591, 0x588, 0x57F);
			BaseSoundID = 0x266;

			SetStr(46, 70);
			SetDex(6, 25);
			SetInt(11, 20);

			SetHits(28, 42);
			SetMana(0);

			SetDamage(1, 2);

			SetSkill(SkillName.MagicResist, 25.1, 40.0);
			SetSkill(SkillName.Tactics, 40.1, 60.0);
			SetSkill(SkillName.Wrestling, 40.1, 60.0);

			Fame = 350;
			Karma = 0;

			VirtualArmor = 6;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 23.1;
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 4; } }
		public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat; } }

		public BullFrog(Serial serial)
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
				{	// http://web.archive.org/web/20020214230233/uo.stratics.com/hunters/bullfrog.shtml
					// 4 Hides (carved)

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
