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
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute(0xE89, 0xE8a)]
	public class QuarterStaff : BaseStaff
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.ConcussionBlow; } }

		//		public override int AosStrengthReq{ get{ return 30; } }
		//		public override int AosMinDamage{ get{ return 11; } }
		//		public override int AosMaxDamage{ get{ return 14; } }
		//		public override int AosSpeed{ get{ return 48; } }
		//
		//		public override int OldMinDamage{ get{ return 8; } }
		//		public override int OldMaxDamage{ get{ return 28; } }
		public override int OldStrengthReq { get { return 30; } }
		public override int OldSpeed { get { return 48; } }

		public override int OldDieRolls { get { return 5; } }
		public override int OldDieMax { get { return 5; } }
		public override int OldAddConstant { get { return 3; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 60; } }

		[Constructable]
		public QuarterStaff()
			: base(0xE89)
		{
			Weight = 4.0;
		}

		public QuarterStaff(Serial serial)
			: base(serial)
		{
		}

// old name removed, see base class

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