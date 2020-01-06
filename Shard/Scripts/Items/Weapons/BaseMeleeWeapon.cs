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
using Server;
using Server.Items;
using Server.Engines.Harvest;

namespace Server.Items
{
	public abstract class BaseMeleeWeapon : BaseWeapon
	{
		public BaseMeleeWeapon(int itemID)
			: base(itemID)
		{
		}

		public BaseMeleeWeapon(Serial serial)
			: base(serial)
		{
		}

		public override int AbsorbDamage(Mobile attacker, Mobile defender, int damage)
		{
			damage = base.AbsorbDamage(attacker, defender, damage);

			if (Core.AOS)
				return damage;

			int absorb = defender.MeleeDamageAbsorb;

			if (absorb > 0)
			{
				if (absorb > damage)
				{
					int react = damage / 5;

					if (react <= 0)
						react = 1;

					defender.MeleeDamageAbsorb -= damage;
					damage = 0;

					attacker.Damage(react, defender);

					attacker.PlaySound(0x1F1);
					attacker.FixedEffect(0x374A, 10, 16);
				}
				else
				{
					defender.MeleeDamageAbsorb = 0;
					defender.SendLocalizedMessage(1005556); // Your reactive armor spell has been nullified.
					DefensiveSpell.Nullify(defender);
				}
			}

			return damage;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}