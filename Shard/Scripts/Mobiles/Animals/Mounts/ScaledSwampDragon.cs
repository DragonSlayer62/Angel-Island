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

/* ./Scripts/Mobiles/Animals/Mounts/ScaledSwampDragon.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
*/

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a swamp dragon corpse")]
	public class ScaledSwampDragon : BaseMount
	{
		[Constructable]
		public ScaledSwampDragon()
			: this("a swamp dragon")
		{
		}

		[Constructable]
		public ScaledSwampDragon(string name)
			: base(name, 0x31F, 0x3EBE, AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			SetStr(201, 300);
			SetDex(66, 85);
			SetInt(61, 100);

			SetHits(121, 180);

			SetDamage(3, 4);

			SetSkill(SkillName.Anatomy, 45.1, 55.0);
			SetSkill(SkillName.MagicResist, 45.1, 55.0);
			SetSkill(SkillName.Tactics, 45.1, 55.0);
			SetSkill(SkillName.Wrestling, 45.1, 55.0);

			Fame = 2000;
			Karma = -2000;

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 93.9;
		}

		public override double GetControlChance(Mobile m)
		{
			return 1.0;
		}

		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public ScaledSwampDragon(Serial serial)
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
