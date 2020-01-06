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
using Server.Targeting;
using Server.Network;
using Server.Items;
using Server.Misc;

namespace Server.Spells.Fifth
{
    public class DispelFieldSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Dispel Field", "An Grav",
                SpellCircle.Fifth,
                206,
                9002,
                Reagent.BlackPearl,
                Reagent.SpidersSilk,
                Reagent.SulfurousAsh,
                Reagent.Garlic
            );

        public DispelFieldSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Item item)
        {
            Type t = item.GetType();

            if (!Caster.CanSee(item))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (!t.IsDefined(typeof(DispellableFieldAttribute), false))
            {
                Caster.SendLocalizedMessage(1005049); // That cannot be dispelled.
            }
            else if (item is Moongate && !((Moongate)item).Dispellable)
            {
                Caster.SendLocalizedMessage(1005047); // That magic is too chaotic
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, item);

                Effects.SendLocationParticles(EffectItem.Create(item.Location, item.Map, EffectItem.DefaultDuration), 0x376A, 9, 20, 5042);
                Effects.PlaySound(item.GetWorldLocation(), item.Map, 0x201);

                item.Delete();
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private DispelFieldSpell m_Owner;

            public InternalTarget(DispelFieldSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Item)
                {
                    m_Owner.Target((Item)o);
                }
                else
                {
                    m_Owner.Caster.SendLocalizedMessage(1005049); // That cannot be dispelled.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}