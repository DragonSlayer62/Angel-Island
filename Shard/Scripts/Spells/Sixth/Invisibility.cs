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
/* ChangeLog:
	6/5/04, Pix
		Merged in 1.0RC0 code.
*/

using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Mobiles;

namespace Server.Spells.Sixth
{
    public class InvisibilitySpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Invisibility", "An Lor Xen",
                SpellCircle.Sixth,
                206,
                9002,
                Reagent.Bloodmoss,
                Reagent.Nightshade
            );

        public InvisibilitySpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {

            return base.CheckCast();
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
			else if (m.CheckState(Mobile.ExpirationFlagID.EvilCrim))
			{	// Evils that kill innocents are flagged with a special criminal flag the prevents them from gate/hide
				
				// question(6)
				Caster.SendLocalizedMessage(501855);	// "Your magic seems to have no effect."

				DoFizzle();
			}
			else if (CheckBSequence(m))
			{
				SpellHelper.Turn(Caster, m);

				Effects.SendLocationParticles(EffectItem.Create(new Point3D(m.X, m.Y, m.Z + 16), Caster.Map, EffectItem.DefaultDuration), 0x376A, 10, 15, 5045);
				m.PlaySound(0x3C4);

				m.Hidden = true;

				RemoveTimer(m);

				TimeSpan duration = TimeSpan.FromSeconds(((6 * Caster.Skills.Magery.Fixed) / 50) + 1);

				Timer t = new InternalTimer(m, duration);

				m_Table[m] = t;

				t.Start();
			}

            FinishSequence();
        }

        private static Hashtable m_Table = new Hashtable();

        public static void RemoveTimer(Mobile m)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                t.Stop();
                m_Table.Remove(m);
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Mobile;

            public InternalTimer(Mobile m, TimeSpan duration)
                : base(duration)
            {
                Priority = TimerPriority.OneSecond;
                m_Mobile = m;
            }

            protected override void OnTick()
            {
                m_Mobile.RevealingAction();
                RemoveTimer(m_Mobile);
            }
        }

        public class InternalTarget : Target
        {
            private InvisibilitySpell m_Owner;

            public InternalTarget(InvisibilitySpell owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                {
                    m_Owner.Target((Mobile)o);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}