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
	[FlipableAttribute(0x13B0, 0x13AF)]
	public class WarAxe : BaseBashing
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.ArmorIgnore; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.BleedAttack; } }

		//		public override int AosStrengthReq{ get{ return 35; } }
		//		public override int AosMinDamage{ get{ return 14; } }
		//		public override int AosMaxDamage{ get{ return 15; } }
		//		public override int AosSpeed{ get{ return 33; } }
		//
		//		public override int OldMinDamage{ get{ return 9; } }
		//		public override int OldMaxDamage{ get{ return 27; } }
		public override int OldStrengthReq { get { return 35; } }
		public override int OldSpeed { get { return 40; } }

		public override int OldDieRolls { get { return 6; } }
		public override int OldDieMax { get { return 4; } }
		public override int OldAddConstant { get { return 3; } }

		public override int DefHitSound { get { return 0x233; } }
		public override int DefMissSound { get { return 0x239; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 80; } }

		public override SkillName DefSkill { get { return SkillName.Macing; } }
		public override WeaponType DefType { get { return WeaponType.Bashing; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Bash1H; } }


		[Constructable]
		public WarAxe()
			: base(0x13B0)
		{
			Weight = 8.0;
		}

		public WarAxe(Serial serial)
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