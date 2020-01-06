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

/* /sandbox/ai/Scripts/Items/Weapons/Abilities/WhirlwindAttack.cs
 *	ChangeLog :
 *	07/23/08, weaver
 *		Automated IPooledEnumerable optimizations. 1 loops updated.
 */

using System;
using System.Collections;
using Server;
using Server.Spells;
using Server.Engines.PartySystem;

namespace Server.Items
{
	/// <summary>
	/// A godsend to a warrior surrounded, the Whirlwind Attack allows the fighter to strike at all nearby targets in one mighty spinning swing.
	/// </summary>
	public class WhirlwindAttack : WeaponAbility
	{
		public WhirlwindAttack()
		{
		}

		public override int BaseMana { get { return 15; } }

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker))
				return;

			ClearCurrentAbility(attacker);

			Map map = attacker.Map;

			if (map == null)
				return;

			BaseWeapon weapon = attacker.Weapon as BaseWeapon;

			if (weapon == null)
				return;

			if (!CheckMana(attacker, true))
				return;

			attacker.FixedEffect(0x3728, 10, 15);
			attacker.PlaySound(0x2A1);

			ArrayList list = new ArrayList();

			IPooledEnumerable eable = attacker.GetMobilesInRange(1);
			foreach (Mobile m in eable)
				list.Add(m);
			eable.Free();

			Party p = Party.Get(attacker);

			for (int i = 0; i < list.Count; ++i)
			{
				Mobile m = (Mobile)list[i];

				if (m != defender && m != attacker && SpellHelper.ValidIndirectTarget(attacker, m) && (p == null || !p.Contains(m)))
				{
					if (m == null || m.Deleted || attacker.Deleted || m.Map != attacker.Map || !m.Alive || !attacker.Alive || !attacker.CanSee(m))
						continue;

					if (!attacker.InRange(m, weapon.MaxRange))
						continue;

					if (attacker.InLOS(m))
					{
						attacker.RevealingAction();

						attacker.SendLocalizedMessage(1060161); // The whirling attack strikes a target!
						m.SendLocalizedMessage(1060162); // You are struck by the whirling attack and take damage!

						weapon.OnHit(attacker, m);
					}
				}
			}
		}
	}
}
