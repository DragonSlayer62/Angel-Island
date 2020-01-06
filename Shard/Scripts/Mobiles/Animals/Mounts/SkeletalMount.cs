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

/* ./Scripts/Mobiles/Animals/Mounts/SkeletalMount.cs
 *	ChangeLog :
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
*/

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("an undead horse corpse")]
	public class SkeletalMount : BaseMount
	{
		[Constructable]
		public SkeletalMount()
			: this("a skeletal steed")
		{
		}

		[Constructable]
		public SkeletalMount(string name)
			: base(name, 793, 0x3EBB, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			SetStr(91, 100);
			SetDex(46, 55);
			SetInt(46, 60);

			SetHits(41, 50);

			SetDamage(5, 12);

			SetSkill(SkillName.MagicResist, 95.1, 100.0);
			SetSkill(SkillName.Tactics, 50.0);
			SetSkill(SkillName.Wrestling, 70.1, 80.0);

			Fame = 0;
			Karma = 0;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public SkeletalMount(Serial serial)
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

			Name = "a skeletal steed";
			Tamable = false;
			MinTameSkill = 0.0;
			ControlSlots = 0;
		}
	}
}
