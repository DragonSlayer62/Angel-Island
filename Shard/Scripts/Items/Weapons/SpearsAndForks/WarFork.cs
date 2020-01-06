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

/* Scripts/Items/Weapons/SpearsAndForks/WarFork.cs
 * ChangeLog :
 *	10/16/05, Pix
 *		Streamlined applied poison code.
 *	09/13/05, erlein
 *		Reverted poisoning rules, applied same system as archery for determining
 *		poison level achieved.
 *	09/12/05, erlein
 *		Changed OnHit() code to utilise new poisoning rules.
 *	4/23/04, Pulse
 *		Added OnHit() function to call Base.OnHit() and then perform poison check
 *		prior to this change, war forks were poisonable, but would never apply poison
 *		to victim or consume charges, it will now do both of those things properly.
 */

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	[FlipableAttribute(0x1405, 0x1404)]
	public class WarFork : BaseSpear
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.BleedAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		//		public override int AosStrengthReq{ get{ return 45; } }
		//		public override int AosMinDamage{ get{ return 12; } }
		//		public override int AosMaxDamage{ get{ return 13; } }
		//		public override int AosSpeed{ get{ return 43; } }
		//
		//		public override int OldMinDamage{ get{ return 4; } }
		//		public override int OldMaxDamage{ get{ return 32; } }
		public override int OldStrengthReq { get { return 35; } }
		public override int OldSpeed { get { return 45; } }

		public override int OldDieRolls { get { return 1; } }
		public override int OldDieMax { get { return 29; } }
		public override int OldAddConstant { get { return 3; } }

		public override int DefHitSound { get { return 0x236; } }
		public override int DefMissSound { get { return 0x238; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 110; } }

		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Pierce1H; } }

		[Constructable]
		public WarFork()
			: base(0x1405)
		{
			Weight = 9.0;
		}

		public WarFork(Serial serial)
			: base(serial)
		{
		}

		public override void OnHit(Mobile attacker, Mobile defender)
		{
			base.OnHit(attacker, defender);

			if (!Core.AOS && Poison != null && PoisonCharges > 0)
			{
				--PoisonCharges;

				if (Utility.RandomDouble() >= 0.5) // 50% chance to poison
				{
					defender.ApplyPoison(attacker, GetPoisonBasedOnSkillAndPoison(attacker, Poison));
				}
			}
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