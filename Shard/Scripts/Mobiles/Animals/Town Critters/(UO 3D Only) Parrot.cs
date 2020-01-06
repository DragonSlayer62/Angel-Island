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

using System;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a parrot corpse")]
	public class Parrot : BaseCreature
	{
		[Constructable]
		public Parrot()
			: base(AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			this.Body = 831;
			this.Name = ("a parrot");
			this.VirtualArmor = Utility.Random(0, 6);

			this.InitStats((10), Utility.Random(25, 16), (10));

			this.Skills[SkillName.Wrestling].Base = (6);
			this.Skills[SkillName.Tactics].Base = (6);
			this.Skills[SkillName.MagicResist].Base = (5);

			this.Fame = Utility.Random(0, 1249);
			this.Karma = Utility.Random(0, -624);

			Tamable = true;
			ControlSlots = 1;
			MinTameSkill = 0.0;
		}

		public Parrot(Serial serial)
			: base(serial)
		{
		}

		public override int GetAngerSound()
		{
			return 0x1B;
		}

		public override int GetIdleSound()
		{
			return 0x1C;
		}

		public override int GetAttackSound()
		{
			return 0x1D;
		}

		public override int GetHurtSound()
		{
			return 0x1E;
		}

		public override int GetDeathSound()
		{
			return 0x1F;
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