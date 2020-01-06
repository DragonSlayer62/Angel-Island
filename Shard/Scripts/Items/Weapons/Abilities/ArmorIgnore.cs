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

/* Items/Weapons/Abilties/ArmorIgnore.cs
 * CHANGELOG:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	/// <summary>
	/// This special move allows the skilled warrior to bypass his target's physical resistance, for one shot only.
	/// The Armor Ignore shot does slightly less damage than normal.
	/// Against a heavily armored opponent, this ability is a big win, but when used against a very lightly armored foe, it might be better to use a standard strike!
	/// </summary>
	public class ArmorIgnore : WeaponAbility
	{
		public ArmorIgnore()
		{
		}

		public override int BaseMana { get { return 30; } }
		public override double DamageScalar { get { return 0.9; } }

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
				return;

			ClearCurrentAbility(attacker);

			attacker.SendLocalizedMessage(1060076); // Your attack penetrates their armor!
			defender.SendLocalizedMessage(1060077); // The blow penetrated your armor!

			defender.PlaySound(0x56);
			defender.FixedParticles(0x3728, 200, 25, 9942, EffectLayer.Waist);
		}
	}
}