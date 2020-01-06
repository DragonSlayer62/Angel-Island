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

/* Scripts\Spells\NPC\InvisibleShield.cs
 * ChangeLog:
 * 	6/29/10, adam
 * 		Initial creation
 *		This spell is used by guards on Suicide Bombers to contain the blast
 */

using System;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Fifth
{
    public class InvisibleShieldSpell : Spell
    {
        // Kal (summon)
        // Sanct (Protection)
        // Grav (field)
        private static SpellInfo m_Info = new SpellInfo(
                "InvisibleShield", "Kal Sanct Grav",
                SpellCircle.Fifth,
                221,
                9022,
                true,
                Reagent.BlackPearl,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk,
                Reagent.SulfurousAsh
            );

        public InvisibleShieldSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
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
            else if (m.Spell != null && m.Spell.IsCasting)
            {
                // too busy casting
            }
            else if (m.InvisibleShield)
            {
                Caster.SendMessage("They are already covered with an invisible shield.");
            }
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);
                m.Shield(TimeSpan.FromMinutes(2.0));

                InvisibleShieldInfo info = new InvisibleShieldInfo(Caster, m, TimeSpan.FromMinutes(2.0));
                info.m_Timer = Timer.DelayCall(TimeSpan.Zero, TimeSpan.FromSeconds(1.25), new TimerStateCallback(ProcessInvisibleShieldInfo), info);

                // wall of stone sound: point, map, sound
                Effects.PlaySound(m, Caster.Map, 0x1F6);
            }

            FinishSequence();
        }

        public class InternalTarget : Target
        {
            private InvisibleShieldSpell m_Owner;

            public InternalTarget(InvisibleShieldSpell owner)
                : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class InvisibleShieldInfo
        {
            public Mobile m_From;
            public Mobile m_Target;
            public DateTime m_EndTime;
            public Timer m_Timer;

            public InvisibleShieldInfo(Mobile from, Mobile target, TimeSpan duration)
            {
                m_From = from;
                m_Target = target;
                m_EndTime = DateTime.Now + duration;
            }
        }

        private static void ProcessInvisibleShieldInfo(object state)
        {
            InvisibleShieldInfo info = (InvisibleShieldInfo)state;
            Mobile from = info.m_From;
            Mobile targ = info.m_Target;

            if (DateTime.Now >= info.m_EndTime || targ.Deleted || from.Map != targ.Map ||
                targ.GetDistanceToSqrt(from) > 16 || targ.InvisibleShield == false)
            {
                if (info.m_Timer != null)
                    info.m_Timer.Stop();

                targ.InvisibleShield = false;
            }
            else
            {
                targ.FixedEffect(0x376A, 1, 32, 51, 0);
            }
        }
    }
}