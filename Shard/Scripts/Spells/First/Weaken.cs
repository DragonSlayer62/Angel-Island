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
/* ChangeLog
 * 5/30/05, Kit
 *		Changed check that only allowed spells effect to work if casted from a player
 *		Mobs can now cast weaken that works.
 *	4/27/05 smerX
 *		Low level debuffs always interrupt targets' spells, circa OSI15
 */

using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;

namespace Server.Spells.First
{
    public class WeakenSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Weaken", "Des Mani",
                SpellCircle.First,
                212,
                9031,
                Reagent.Garlic,
                Reagent.Nightshade
            );

        public WeakenSpell(Mobile caster, Item scroll)
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
            else if (CheckHSequence(m))
            {
                SpellHelper.Turn(Caster, m);

                if (m.Spell != null)
                    m.Spell.OnCasterHurt();

                SpellHelper.CheckReflect((int)this.Circle, Caster, ref m);

                if (SpellHelper.AddStatCurse(Caster, m, StatType.Str))
                {
                    m.FixedParticles(0x3779, 10, 15, 5009, EffectLayer.Waist);
                    m.PlaySound(0x1E6);
                }
                else
                {
                    Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 502632); // The spell fizzles.
                    Caster.FixedEffect(0x3735, 6, 30);
                    Caster.PlaySound(0x5C);
                }

                m.Paralyzed = false;

            }

            FinishSequence();
        }

        public class InternalTarget : Target
        {
            private WeakenSpell m_Owner;

            public InternalTarget(WeakenSpell owner)
                : base(12, false, TargetFlags.Harmful)
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