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
using System.Collections;

namespace Server.Items
{
	/// <summary>
	/// Make your opponent bleed profusely with this wicked use of your weapon.
	/// When successful, the target will bleed for several seconds, taking damage as time passes for up to ten seconds.
	/// The rate of damage slows down as time passes, and the blood loss can be completely staunched with the use of bandages. 
	/// </summary>
	public class BleedAttack : WeaponAbility
	{
		public BleedAttack()
		{
		}

		public override int BaseMana { get { return 30; } }

		public override void OnHit(Mobile attacker, Mobile defender, int damage)
		{
			if (!Validate(attacker) || !CheckMana(attacker, true))
				return;

			ClearCurrentAbility(attacker);

			attacker.SendLocalizedMessage(1060159); // Your target is bleeding!
			defender.SendLocalizedMessage(1060160); // You are bleeding!

			defender.PlaySound(0x133);
			defender.FixedParticles(0x377A, 244, 25, 9950, 31, 0, EffectLayer.Waist);

			BeginBleed(defender, attacker);
		}

		private static Hashtable m_Table = new Hashtable();

		public static bool IsBleeding(Mobile m)
		{
			return m_Table.Contains(m);
		}

		public static void BeginBleed(Mobile m, Mobile from)
		{
			Timer t = (Timer)m_Table[m];

			if (t != null)
				t.Stop();

			t = new InternalTimer(from, m);
			m_Table[m] = t;

			t.Start();
		}

		public static void DoBleed(Mobile m, Mobile from, int level)
		{
			if (m.Alive)
			{
				int damage = Utility.RandomMinMax(level, level * 2);

				if (!m.Player)
					damage *= 2;

				m.PlaySound(0x133);
				m.Damage(damage, from);

				Blood blood = new Blood();

				blood.ItemID = Utility.Random(0x122A, 5);

				blood.MoveToWorld(m.Location, m.Map);
			}
			else
			{
				EndBleed(m, false);
			}
		}

		public static void EndBleed(Mobile m, bool message)
		{
			Timer t = (Timer)m_Table[m];

			if (t == null)
				return;

			t.Stop();
			m_Table.Remove(m);

			m.SendLocalizedMessage(1060167); // The bleeding wounds have healed, you are no longer bleeding!
		}

		private class InternalTimer : Timer
		{
			private Mobile m_From;
			private Mobile m_Mobile;
			private int m_Count;

			public InternalTimer(Mobile from, Mobile m)
				: base(TimeSpan.FromSeconds(2.0), TimeSpan.FromSeconds(2.0))
			{
				m_From = from;
				m_Mobile = m;
				Priority = TimerPriority.TwoFiftyMS;
			}

			protected override void OnTick()
			{
				DoBleed(m_Mobile, m_From, 5 - m_Count);

				if (++m_Count == 5)
					EndBleed(m_Mobile, true);
			}
		}
	}
}