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

/* Spells/Fourth/ArchCure.cs
 * CHANGELOG:
 *	3/22/10, Adam
 *		Rename mantra to "An Vas Nox" and add the tag "[Greater Cure]"
 *		It all shows up now in game correctly
 *  11/07/05, Kit
 *		Restored former cure rates.
 *	10/16/05, Pix
 *		Change to chance to cure.
 *  6/4/04, Pixie
 *		Changed to Greater Cure type spell (no more area effect)
 *		with greater chance to cure than Cure.
 *		Added debugging for cure chance for people > playerlevel
 *	5/25/04, Pixie
 *		Changed formula for success curing poison
 *	5/22/04, Pixie
 *		Made it so chance to cure poison was based on the caster's magery vs the level of poison
 */


using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;

namespace Server.Spells.Fourth
{
    public class ArchCureSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Greater Cure", "An Vas Nox [Greater Cure]",
                SpellCircle.Fourth,
                215,
                9061,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.MandrakeRoot
            );
        private static SpellInfo m_InfoIS = new SpellInfo(
                "Arch Cure", "Vas An Nox",
                SpellCircle.Fourth,
                215,
                9061,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.MandrakeRoot
            );

        public ArchCureSpell(Mobile caster, Item scroll)
            : base(caster, scroll, Core.UOSP ? m_InfoIS : m_Info)
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
            else if (CheckBSequence(m))
            {
                SpellHelper.Turn(Caster, m);
                //chance to cure poison is ((caster's magery/poison level) - 20%)

                double chance = 100;
                try //I threw this try-catch block in here because Poison is whacky... there'll be a tiny 
                {   //race condition if multiple people are casting cure on the same target... 
                    if (m.Poison != null)
                    {
                        //desired is: LP: 50%, DP: 90% GP-: 100%
                        double multiplier = 0.5 + 0.4 * (4 - m.Poison.Level);
                        chance = Caster.Skills[SkillName.Magery].Value * multiplier;
                    }

                    if (Caster.AccessLevel > AccessLevel.Player)
                    {
                        Caster.SendMessage("Chance to cure is " + chance + "%");
                    }
                }
                catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

                /*
                //new cure rates
                int chance = 100;
                Poison p = m.Poison;
                try
                {
                    if( p != null )
                    {
                        chance = 10000 
                            + (int)(Caster.Skills[SkillName.Magery].Value * 75) 
                            - ((p.Level + 1) * 1750);
                        chance /= 100;
                        if( p.Level > 3 ) //lethal poison further penalty
                        {
                            chance -= 35; //@ GM magery, chance will be 52%
                        }
                    }

                    if( Caster.AccessLevel > AccessLevel.Player )
                    {
                        Caster.SendMessage("Chance to cure is " + chance + "%");
                    }
                }
                catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
                */

                if (Utility.Random(0, 100) <= chance)
                {
                    if (m.CurePoison(Caster))
                    {
                        if (Caster != m)
                            Caster.SendLocalizedMessage(1010058); // You have cured the target of all poisons!

                        m.SendLocalizedMessage(1010059); // You have been cured of all poisons.
                    }
                }
                else
                {
                    Caster.SendLocalizedMessage(1010060); // You have failed to cure your target!
                }

                m.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                m.PlaySound(0x1E0);
            }

            FinishSequence();
        }

        public void TargetOnIslandSiege(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                SpellHelper.GetSurfaceTop(ref p);

                List<Mobile> targets = new List<Mobile>();

                Map map = Caster.Map;
                Mobile m_directtarget = p as Mobile;

                if (map != null)
                {
                    //you can target directly someone/something and become criminal if it's a criminal action
                    if (m_directtarget != null)
                        targets.Add(m_directtarget);

                    IPooledEnumerable eable = map.GetMobilesInRange(new Point3D(p), 2);

                    foreach (Mobile m in eable)
                    {
                        //Pix - below is the current RunUO code - change it to be simpler :)
                        //                        // Archcure area effect won't cure aggressors or victims, nor murderers, criminals or monsters 
                        //                        // plus Arch Cure Area will NEVER work on summons/pets if you are in Felucca facet
                        //                        // red players can cure only themselves and guildies with arch cure area.
                        //
                        //                        if (map.Rules == MapRules.FeluccaRules)
                        //                        {
                        //                            if (Caster.CanBeBeneficial(m, false) && (!Core.AOS || !IsAggressor(m) && !IsAggressed(m) && ((IsInnocentTo(Caster, m) && IsInnocentTo(m, Caster)) || (IsAllyTo(Caster, m))) && m != m_directtarget && m is PlayerMobile || m == Caster && m != m_directtarget))
                        //                                targets.Add(m);
                        //                        }
                        //                        else if (Caster.CanBeBeneficial(m, false) && (!Core.AOS || !IsAggressor(m) && !IsAggressed(m) && ((IsInnocentTo(Caster, m) && IsInnocentTo(m, Caster)) || (IsAllyTo(Caster, m))) && m != m_directtarget || m == Caster && m != m_directtarget))
                        //                            targets.Add(m);

                        if (Caster.CanBeBeneficial(m, false))
                        {
                            targets.Add(m);
                        }
                    }

                    eable.Free();
                }

                Effects.PlaySound(p, Caster.Map, 0x299);

                if (targets.Count > 0)
                {
                    int cured = 0;

                    for (int i = 0; i < targets.Count; ++i)
                    {
                        Mobile m = targets[i];

                        Caster.DoBeneficial(m);

                        Poison poison = m.Poison;

                        if (poison != null)
                        {
                            int chanceToCure = 10000 + (int)(Caster.Skills[SkillName.Magery].Value * 75) - ((poison.Level + 1) * 1750);
                            chanceToCure /= 100;
                            chanceToCure -= 1;

                            if (chanceToCure > Utility.Random(100) && m.CurePoison(Caster))
                                ++cured;
                        }

                        m.FixedParticles(0x373A, 10, 15, 5012, EffectLayer.Waist);
                        m.PlaySound(0x1E0);
                    }

                    if (cured > 0)
                        Caster.SendLocalizedMessage(1010058); // You have cured the target of all poisons!
                }
            }

            FinishSequence();
        }


        public class InternalTarget : Target
        {
            private ArchCureSpell m_Owner;

            public InternalTarget(ArchCureSpell owner)
                : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (Core.UOSP) //switch targetting for Siege
                {
                    IPoint3D p = o as IPoint3D;
                    if (p != null)
                    {
                        m_Owner.TargetOnIslandSiege(p);
                    }
                }
                else
                {
                    if (o is Mobile)
                    {
                        m_Owner.Target((Mobile)o);
                    }
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}