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

/* Scripts/Mobiles/Animals/Slimes/Jwilson.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 */

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a jwilson corpse")]
	public class Jwilson : BaseCreature
	{
		[Constructable]
		public Jwilson()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Hue = Utility.RandomList(0x89C, 0x8A2, 0x8A8, 0x8AE);
			this.Body = 0x33;
			this.Name = ("a jwilson");
			this.VirtualArmor = 8;

			this.InitStats(Utility.Random(22, 13), Utility.Random(16, 6), Utility.Random(16, 5));

			this.Skills[SkillName.Wrestling].Base = Utility.Random(24, 17);
			this.Skills[SkillName.Tactics].Base = Utility.Random(18, 14);
			this.Skills[SkillName.MagicResist].Base = Utility.Random(15, 6);
			this.Skills[SkillName.Poisoning].Base = Utility.Random(31, 20);

			this.Fame = Utility.Random(0, 1249);
			this.Karma = Utility.Random(0, -624);
		}

		public Jwilson(Serial serial)
			: base(serial)
		{
		}

		public override int GetAngerSound()
		{
			return 0x1C8;
		}

		public override int GetIdleSound()
		{
			return 0x1C9;
		}

		public override int GetAttackSound()
		{
			return 0x1CA;
		}

		public override int GetHurtSound()
		{
			return 0x1CB;
		}

		public override int GetDeathSound()
		{
			return 0x1CC;
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
