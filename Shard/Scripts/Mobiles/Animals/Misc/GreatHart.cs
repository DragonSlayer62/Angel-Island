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

/* Scripts/Mobiles/Animals/Misc/GreatHart.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 3 lines removed.
 *	1/14/05, Jade
 *      Changed taming requirement to give tamers a new creature to train taming skill on.
 *      This is being done to provide a replacement for ridgeback-level taming training, because ridgebacks no longer spawn.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a deer corpse")]
	[TypeAlias("Server.Mobiles.Greathart")]
	public class GreatHart : BaseCreature
	{
		[Constructable]
		public GreatHart()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a great hart";
			Body = 0xEA;

			SetStr(41, 71);
			SetDex(47, 77);
			SetInt(27, 57);

			SetHits(27, 41);
			SetMana(0);

			SetDamage(5, 9);

			SetSkill(SkillName.MagicResist, 26.8, 44.5);
			SetSkill(SkillName.Tactics, 29.8, 47.5);
			SetSkill(SkillName.Wrestling, 29.8, 47.5);

			Fame = 300;
			Karma = 0;

			VirtualArmor = 24;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 83.1;//Jade: Change to give tamers a ridgeback replacement for training.
		}

		public override int Meat { get { return 6; } }
		public override int Hides { get { return 15; } }
		public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies | FoodType.GrainsAndHay; } }

		public GreatHart(Serial serial)
			: base(serial)
		{
		}

		public override int GetAttackSound()
		{
			return 0x82;
		}

		public override int GetHurtSound()
		{
			return 0x83;
		}

		public override int GetDeathSound()
		{
			return 0x84;
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
