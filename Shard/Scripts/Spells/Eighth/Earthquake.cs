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
/* 
	ChangeLog:
 *	07/23/08, weaver
 *		Automated IPooledEnumerable optimizations. 1 loops updated.
	6/5/04, Pix
		Merged in 1.0RC0 code.
*/

using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Targeting;

namespace Server.Spells.Eighth
{
    public class EarthquakeSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Earthquake", "In Vas Por",
                SpellCircle.Eighth,
                233,
                9012,
                false,
                Reagent.Bloodmoss,
                Reagent.Ginseng,
                Reagent.MandrakeRoot,
                Reagent.SulfurousAsh
            );

        public EarthquakeSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool DelayedDamage { get { return !Core.AOS; } }

        public override void OnCast()
        {
            if (SpellHelper.CheckTown(Caster, Caster) && CheckSequence())
            {
                ArrayList targets = new ArrayList();

                Map map = Caster.Map;

                if (map != null)
                {
                    IPooledEnumerable eable = Caster.GetMobilesInRange(1 + (int)(Caster.Skills[SkillName.Magery].Value / 15.0));
                    foreach (Mobile m in eable)
                    {
                        if (Caster != m && SpellHelper.ValidIndirectTarget(Caster, m) && Caster.CanBeHarmful(m, false) && (!Core.AOS || Caster.InLOS(m)))
                            targets.Add(m);
                    }
                    eable.Free();
                }

                Caster.PlaySound(0x2F3);

                for (int i = 0; i < targets.Count; ++i)
                {
                    Mobile m = (Mobile)targets[i];

                    int damage;

                    if (Core.AOS)
                    {
                        damage = m.Hits / 2;

                        if (m.Player)
                            damage += Utility.RandomMinMax(0, 15);

                        if (damage < 15)
                            damage = 15;
                        else if (damage > 100)
                            damage = 100;
                    }
                    else
                    {
                        damage = (m.Hits * 6) / 10;

                        if (!m.Player && damage < 10)
                            damage = 10;
                        else if (damage > 75)
                            damage = 75;
                    }

                    Caster.DoHarmful(m);
                    SpellHelper.Damage(TimeSpan.Zero, m, Caster, damage, 100, 0, 0, 0, 0);
                }
            }

            FinishSequence();
        }
    }
}
