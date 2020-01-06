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

/* Scripts/Mobiles/Animals/Misc/Goat.cs
 *	ChangeLog :
 *	3/4/11, adam
 *		goats eat leather
 *		http://vboards.stratics.com/uo-tamer/38862-amazing-pet-colour-charts.html
 *		http://update.uo.com/design_411.html
 *		Added for Siege accuracy, but enabled for all shards since it's probably still there.
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 2 lines removed.
*/

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a goat corpse")]
	public class Goat : BaseCreature
	{
		[Constructable]
		public Goat()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a goat";
			Body = 0xD1;
			BaseSoundID = 0x99;

			SetStr(19);
			SetDex(15);
			SetInt(5);

			SetHits(12);
			SetMana(0);

			SetDamage(3, 4);

			SetSkill(SkillName.MagicResist, 5.0);
			SetSkill(SkillName.Tactics, 5.0);
			SetSkill(SkillName.Wrestling, 5.0);

			Fame = 150;
			Karma = 0;

			VirtualArmor = 10;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 11.1;
		}

		public override int Meat { get { return 2; } }
		public override int Hides { get { return 8; } }
		public override FoodType FavoriteFood { get { return FoodType.GrainsAndHay | FoodType.FruitsAndVegies | FoodType.Leather; } }

		public Goat(Serial serial)
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
