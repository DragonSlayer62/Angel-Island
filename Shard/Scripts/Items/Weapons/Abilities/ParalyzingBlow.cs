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

/* Items/Weapons/Abilties/ParalyzingBlow.cs
 * CHANGELOG:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	/// <summary>
	/// A successful Paralyzing Blow will leave the target stunned, unable to move, attack, or cast spells, for a few seconds.
	/// </summary>
	public class ParalyzingBlow : WeaponAbility
	{
		public ParalyzingBlow()
		{
		}

		public override int BaseMana { get { return 30; } }

		public static readonly TimeSpan PlayerFreezeDuration = TimeSpan.FromSeconds(3.0);
		public static readonly TimeSpan NPCFreezeDuration = TimeSpan.FromSeconds(6.0);

		// No longer active in pub21:
		//BUT WE want it on Angel Island
		public override bool CheckSkills(Mobile from)
		{
			if (!base.CheckSkills(from))
				return false;

			if (!(from.Weapon is Fists))
				return true;

			Skill skill = from.Skills[SkillName.Anatomy];

			if (skill != null && skill.Base >= 80.0)
				return true;

			from.SendLocalizedMessage(1061811); // You lack the required anatomy skill to perform that attack!

			return false;
		}

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
				return;

			ClearCurrentAbility(attacker);

			attacker.SendLocalizedMessage(1060163); // You deliver a paralyzing blow!
			defender.SendLocalizedMessage(1060164); // The attack has temporarily paralyzed you!

			defender.Freeze(defender.Player ? PlayerFreezeDuration : NPCFreezeDuration);

			defender.FixedEffect(0x376A, 9, 32);
			defender.PlaySound(0x204);
		}
	}
}