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

/* ./Scripts/Mobiles/Animals/Cows/Bull.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 3 lines removed.
*/

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a bull corpse")]
	public class Bull : BaseCreature
	{
		[Constructable]
		public Bull()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a bull";
			Body = Utility.RandomList(0xE8, 0xE9);
			BaseSoundID = 0x64;

			if (0.5 >= Utility.RandomDouble())
				Hue = 0x901;

			SetStr(77, 111);
			SetDex(56, 75);
			SetInt(47, 75);

			SetHits(50, 64);
			SetMana(0);

			SetDamage(4, 9);

			SetSkill(SkillName.MagicResist, 17.6, 25.0);
			SetSkill(SkillName.Tactics, 67.6, 85.0);
			SetSkill(SkillName.Wrestling, 40.1, 57.5);

			Fame = 600;
			Karma = 0;

			VirtualArmor = 28;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 71.1;
		}

		public override int Meat { get { return 10; } }
		public override int Hides { get { return 15; } }
		public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Bull; } }

		public Bull(Serial serial)
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
