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

/* Scripts/Mobiles/Animals/Town Critters/Bird.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName("a bird corpse")]
	public class Bird : BaseCreature
	{
		[Constructable]
		public Bird()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			if (Utility.RandomBool())
			{
				Hue = 0x901;

				switch (Utility.Random(3))
				{
					case 0: Name = "a crow"; break;
					case 2: Name = "a raven"; break;
					case 1: Name = "a magpie"; break;
				}
			}
			else
			{
				Hue = Utility.RandomBirdHue();
				Name = NameList.RandomName("bird");
			}

			Body = 6;
			BaseSoundID = 0x1B;

			VirtualArmor = Utility.RandomMinMax(0, 6);

			SetStr(10);
			SetDex(25, 35);
			SetInt(10);

			SetDamage(0);


			SetSkill(SkillName.Wrestling, 4.2, 6.4);
			SetSkill(SkillName.Tactics, 4.0, 6.0);
			SetSkill(SkillName.MagicResist, 4.0, 5.0);

			SetFameLevel(1);
			SetKarmaLevel(0);

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -6.9;
		}

		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Meat { get { return 1; } }
		public override int Feathers { get { return 25; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public Bird(Serial serial)
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

			if (Hue == 0)
				Hue = Utility.RandomBirdHue();
		}
	}

	[CorpseName("a bird corpse")]
	public class TropicalBird : BaseCreature
	{
		[Constructable]
		public TropicalBird()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Hue = Utility.RandomBirdHue();
			Name = "a tropical bird";

			Body = 6;
			BaseSoundID = 0xBF;

			VirtualArmor = Utility.RandomMinMax(0, 6);

			SetStr(10);
			SetDex(25, 35);
			SetInt(10);

			SetDamage(0);


			SetSkill(SkillName.Wrestling, 4.2, 6.4);
			SetSkill(SkillName.Tactics, 4.0, 6.0);
			SetSkill(SkillName.MagicResist, 4.0, 5.0);

			SetFameLevel(1);
			SetKarmaLevel(0);

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = -6.9;
		}

		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Meat { get { return 1; } }
		public override int Feathers { get { return 25; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public TropicalBird(Serial serial)
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
