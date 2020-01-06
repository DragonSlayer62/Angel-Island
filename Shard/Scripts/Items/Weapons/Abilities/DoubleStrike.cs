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

namespace Server.Items
{
	/// <summary>
	/// The highly skilled warrior can use this special attack to make two quick swings in succession.
	/// Landing both blows would be devastating! 
	/// </summary>
	public class DoubleStrike : WeaponAbility
	{
		public DoubleStrike()
		{
		}

		public override int BaseMana { get { return 30; } }
		public override double DamageScalar { get { return 0.9; } }

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
				return;

			ClearCurrentAbility(attacker);

			attacker.SendLocalizedMessage(1060084); // You attack with lightning speed!
			defender.SendLocalizedMessage(1060085); // Your attacker strikes with lightning speed!

			defender.PlaySound(0x3BB);
			defender.FixedEffect(0x37B9, 244, 25);

			// Swing again:

			// If no combatant, wrong map, one of us is a ghost, or cannot see, or deleted, then stop combat
			if (defender == null || defender.Deleted || attacker.Deleted || defender.Map != attacker.Map || !defender.Alive || !attacker.Alive || !attacker.CanSee(defender))
			{
				attacker.Combatant = null;
				return;
			}

			IWeapon weapon = attacker.Weapon;

			if (weapon == null)
				return;

			if (!attacker.InRange(defender, weapon.MaxRange))
				return;

			if (attacker.InLOS(defender))
			{
				BaseWeapon.InDoubleStrike = true;
				attacker.RevealingAction();
				attacker.NextCombatTime = DateTime.Now + weapon.OnSwing(attacker, defender);
				BaseWeapon.InDoubleStrike = false;
			}
		}
	}
}