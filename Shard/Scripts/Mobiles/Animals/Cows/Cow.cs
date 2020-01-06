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

/* Scripts/Mobiles/Animals/Cows/Cow.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a cow corpse")]
	public class Cow : BaseCreature
	{
		[Constructable]
		public Cow()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a cow";
			Body = Utility.RandomList(0xD8, 0xE7);
			BaseSoundID = 0x78;

			SetStr(30);
			SetDex(15);
			SetInt(5);

			SetHits(18);
			SetMana(0);

			SetDamage(1, 4);

			SetDamage(1, 4);

			SetSkill(SkillName.MagicResist, 5.5);
			SetSkill(SkillName.Tactics, 5.5);
			SetSkill(SkillName.Wrestling, 5.5);

			Fame = 300;
			Karma = 0;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;

			if (Core.AOS && Utility.Random(1000) == 0) // 0.1% chance to have mad cows
				FightMode = FightMode.All | FightMode.Closest;
		}

		public override int Meat { get { return 8; } }
		public override int Hides { get { return 12; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public override void OnDoubleClick(Mobile from)
		{
			base.OnDoubleClick(from);

			int random = Utility.Random(100);

			if (random < 5)
				Tip();
			else if (random < 20)
				PlaySound(120);
			else if (random < 40)
				PlaySound(121);
		}

		public void Tip()
		{
			PlaySound(121);
			Animate(8, 0, 3, true, false, 0);
		}

		public Cow(Serial serial)
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
