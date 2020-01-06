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

/* ./Scripts/Engines/Plants/MiscMobiles/GiantIceWorm.cs
 *	ChangeLog :
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
*/


using System;
using Server;

namespace Server.Mobiles
{
	[CorpseName("a giant ice worm corpse")]
	public class GiantIceWorm : BaseCreature
	{
		public override bool SubdueBeforeTame { get { return true; } }

		[Constructable]
		public GiantIceWorm()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Body = 89;
			Name = "a giant ice worm";
			BaseSoundID = 0xDC;

			SetStr(216, 245);
			SetDex(76, 100);
			SetInt(66, 85);

			SetHits(130, 147);

			SetDamage(7, 17);

			SetSkill(SkillName.Poisoning, 75.1, 95.0);
			SetSkill(SkillName.MagicResist, 45.1, 60.0);
			SetSkill(SkillName.Tactics, 75.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 71.1;
		}

		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public override Poison HitPoison { get { return Poison.Greater; } }

		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public GiantIceWorm(Serial serial)
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
