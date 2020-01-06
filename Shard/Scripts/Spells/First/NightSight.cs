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

/* Scripts/Spells/First/NightSight.cs
 * CHANGELOG:
 *	8/18/2004 - Pixie
 *		Changed lightlevel to be higher (so it's useful at low levels of magery)
 */

using System;
using Server.Targeting;
using Server.Network;
using Server;

namespace Server.Spells.First
{
    public class NightSightSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Night Sight", "In Lor",
                SpellCircle.First,
                236,
                9031,
                Reagent.SulfurousAsh,
                Reagent.SpidersSilk
            );

        public NightSightSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new NightSightTarget(this);
        }

        private class NightSightTarget : Target
        {
            private Spell m_Spell;

            public NightSightTarget(Spell spell)
                : base(10, false, TargetFlags.None)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile && m_Spell.CheckSequence())
                {
                    Mobile targ = (Mobile)targeted;

                    SpellHelper.Turn(m_Spell.Caster, targ);

                    if (targ.BeginAction(typeof(LightCycle)))
                    {
                        new LightCycle.NightSightTimer(targ).Start();
                        int level = (int)Math.Abs(LightCycle.DungeonLevel * (.5 + m_Spell.Caster.Skills[SkillName.Magery].Base / 200));

                        if (level > 25 || level < 0)
                            level = 25;

                        targ.LightLevel = level;

                        targ.FixedParticles(0x376A, 9, 32, 5007, EffectLayer.Waist);
                        targ.PlaySound(0x1E3);
                    }
                    else
                    {
                        from.SendMessage("{0} already have nightsight.", from == targ ? "You" : "They");
                    }
                }

                m_Spell.FinishSequence();
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Spell.FinishSequence();
            }
        }
    }
}
