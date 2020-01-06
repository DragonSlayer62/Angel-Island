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
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0xf4b, 0xf4c)]
	public class DoubleAxe : BaseAxe
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.DoubleStrike; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.WhirlwindAttack; } }

		//		public override int AosStrengthReq{ get{ return 45; } }
		//		public override int AosMinDamage{ get{ return 15; } }
		//		public override int AosMaxDamage{ get{ return 17; } }
		//		public override int AosSpeed{ get{ return 33; } }
		//
		//		public override int OldMinDamage{ get{ return 5; } }
		//		public override int OldMaxDamage{ get{ return 35; } }
		public override int OldStrengthReq { get { return 45; } }
		public override int OldSpeed { get { return 37; } }

		public override int OldDieRolls { get { return 1; } }
		public override int OldDieMax { get { return 31; } }
		public override int OldAddConstant { get { return 1; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		[Constructable]
		public DoubleAxe()
			: base(0xF4B)
		{
			Weight = 8.0;
		}

		public DoubleAxe(Serial serial)
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