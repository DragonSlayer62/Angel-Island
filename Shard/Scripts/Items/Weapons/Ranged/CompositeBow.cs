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
	[FlipableAttribute(0x26C2, 0x26CC)]
	public class CompositeBow : BaseRanged
	{
		public override int EffectID { get { return 0xF42; } }
		public override Type AmmoType { get { return typeof(Arrow); } }
		public override Item Ammo { get { return new Arrow(); } }

		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.MovingShot; } }

		//		public override int AosStrengthReq{ get{ return 45; } }
		//		public override int AosMinDamage{ get{ return 15; } }
		//		public override int AosMaxDamage{ get{ return 17; } }
		//		public override int AosSpeed{ get{ return 25; } }
		//
		//		public override int OldMinDamage{ get{ return 15; } }
		//		public override int OldMaxDamage{ get{ return 17; } }
		public override int OldStrengthReq { get { return 45; } }
		public override int OldSpeed { get { return 25; } }

		public override int OldDieRolls { get { return 4; } }
		public override int OldDieMax { get { return 9; } }
		public override int OldAddConstant { get { return 5; } }

		public override int DefMaxRange { get { return 10; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 70; } }

		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.ShootBow; } }

		[Constructable]
		public CompositeBow()
			: base(0x26C2)
		{
			Weight = 5.0;
		}

		public CompositeBow(Serial serial)
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