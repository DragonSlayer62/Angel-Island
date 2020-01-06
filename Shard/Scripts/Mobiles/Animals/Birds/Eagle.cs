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

/* ./Scripts/Mobiles/Animals/Birds/Eagle.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("an eagle corpse")]
	public class Eagle : BaseCreature
	{
		[Constructable]
		public Eagle()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "an eagle";
			Body = 5;
			BaseSoundID = 0x2EE;

			SetStr(31, 47);
			SetDex(36, 60);
			SetInt(8, 20);

			SetHits(20, 27);
			SetMana(0);

			SetDamage(5, 10);

			SetSkill(SkillName.MagicResist, 15.3, 30.0);
			SetSkill(SkillName.Tactics, 18.1, 37.0);
			SetSkill(SkillName.Wrestling, 20.1, 30.0);

			Fame = 300;
			Karma = 0;

			VirtualArmor = 22;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 17.1;
		}

		public override int Meat { get { return 1; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 36; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

		public Eagle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
				PackItem(new BlackPearl(4));
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020221204551/uo.stratics.com/hunters/eagle.shtml
					// 1 Raw bird (carved), 36 Feathers (carved)

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
					// no loot in RunUO 2.0
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
