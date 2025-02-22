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

/* ./Scripts/Mobiles/Animals/Mounts/DesertOstard.cs
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
	[CorpseName("an ostard corpse")]
	public class DesertOstard : BaseMount
	{
		[Constructable]
		public DesertOstard()
			: this("a desert ostard")
		{
		}

		[Constructable]
		public DesertOstard(string name)
			: base(name, 0xD2, 0x3EA3, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			BaseSoundID = 0x270;

			SetStr(94, 170);
			SetDex(56, 75);
			SetInt(6, 10);

			SetHits(71, 88);
			SetMana(0);

			SetDamage(5, 11);

			SetSkill(SkillName.MagicResist, 25.1, 30.0);
			SetSkill(SkillName.Tactics, 25.3, 40.0);
			SetSkill(SkillName.Wrestling, 29.3, 44.0);

			Fame = 450;
			Karma = 0;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 29.1;
		}

		public override int Meat { get { return 3; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Ostard; } }

		public DesertOstard(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
