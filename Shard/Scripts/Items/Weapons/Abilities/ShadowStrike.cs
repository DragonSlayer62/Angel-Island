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
	/// This powerful ability requires secondary skills to activate.
	/// Successful use of Shadowstrike deals extra damage to the target — and renders the attacker invisible!
	/// Only those who are adept at the art of stealth will be able to use this ability.
	/// </summary>
	public class ShadowStrike : WeaponAbility
	{
		public ShadowStrike()
		{
		}

		public override int BaseMana { get { return 20; } }
		public override double DamageScalar { get { return 1.25; } }

		public override bool CheckSkills(Mobile from)
		{
			if (!base.CheckSkills(from))
				return false;

			Skill skill = from.Skills[SkillName.Stealth];

			if (skill != null && skill.Value >= 80.0)
				return true;

			from.SendLocalizedMessage(1060183); // You lack the required stealth to perform that attack

			return false;
		}

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
				return;

			ClearCurrentAbility(attacker);

			attacker.SendLocalizedMessage(1060078); // You strike and hide in the shadows!
			defender.SendLocalizedMessage(1060079); // You are dazed by the attack and your attacker vanishes!

			Effects.SendLocationParticles(EffectItem.Create(attacker.Location, attacker.Map, EffectItem.DefaultDuration), 0x376A, 8, 12, 9943);
			attacker.PlaySound(0x482);

			defender.FixedEffect(0x37BE, 20, 25);

			attacker.Combatant = null;
			attacker.Warmode = false;
			attacker.Hidden = true;
		}
	}
}