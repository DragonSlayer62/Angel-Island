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

/* Scripts/Items/Weapons/Swords/BaseKnife.cs
 * ChangeLog :
 *	10/16/05, Pix
 *		Streamlined applied poison code.
 *	09/13/05, erlein
 *		Reverted poisoning rules, applied same system as archery for determining
 *		poison level achieved.
 *	09/12/05, erlein
 *		Changed OnHit() code to utilise new poisoning rules.
 */


using System;
using Server;
using Server.Items;
using Server.Targets;

namespace Server.Items
{
	public abstract class BaseKnife : BaseMeleeWeapon
	{
		public override int DefHitSound { get { return 0x23B; } }
		public override int DefMissSound { get { return 0x238; } }

		public override SkillName DefSkill { get { return SkillName.Swords; } }
		public override WeaponType DefType { get { return WeaponType.Slashing; } }
		public override WeaponAnimation DefAnimation { get { return WeaponAnimation.Slash1H; } }

		public BaseKnife(int itemID)
			: base(itemID)
		{
		}

		public BaseKnife(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			from.SendLocalizedMessage(1010018); // What do you want to use this item on?

			from.Target = new BladedItemTarget(this);
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
	}
}