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
	2/2/05, Darva
		Removing ability to mark Moonstones with the mark spell.
	1/25/05, Darva
		Allowed mark to target moonstones.
	6/5/04, Pix
		Merged in 1.0RC0 code.

*/

using System;
using Server.Items;
using Server.Targeting;
using Server.Network;
using Server.Regions;

namespace Server.Spells.Sixth
{
    public class MarkSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Mark", "Kal Por Ylem",
                SpellCircle.Sixth,
                218,
                9002,
                Reagent.BlackPearl,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot
            );

        public MarkSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public override bool CheckCast()
        {
            if (!base.CheckCast())
                return false;

            return SpellHelper.CheckTravel(Caster, TravelCheckType.Mark);
        }

        public void Target(RecallRune rune)
        {
            if (!Caster.CanSee(rune))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.Mark))
            {
            }
            else if (SpellHelper.CheckMulti(Caster.Location, Caster.Map, !Core.AOS))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (!rune.IsChildOf(Caster.Backpack))
            {
                Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1062422); // You must have this rune in your backpack in order to mark it.
            }
            else if (CheckSequence())
            {
                rune.Mark(Caster);

                Caster.PlaySound(0x1FA);
                Effects.SendLocationEffect(Caster, Caster.Map, 14201, 16);
            }

            FinishSequence();
        }
        /* 	Removing ability to mark runestones with the mark spell.
                public void Target( Moonstone stone )
                {
                    if ( !Caster.CanSee( stone ) )
                    {
                        Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
                    }
                    else if ( !SpellHelper.CheckTravel( Caster, TravelCheckType.Mark ) )
                    {
                    }
                    else if ( SpellHelper.CheckMulti( Caster.Location, Caster.Map, !Core.AOS ) )
                    {
                        Caster.SendLocalizedMessage( 501942 ); // That location is blocked.
                    }
                    else if ( !stone.IsChildOf( Caster.Backpack ) )
                    {
                        Caster.LocalOverheadMessage( MessageType.Regular, 0x3B2, 1062422 ); // You must have this rune in your backpack in order to mark it.
                    }
                    else if ( CheckSequence() )
                    {
                        stone.Mark( Caster );

                        Caster.PlaySound( 0x1FA );
                        Effects.SendLocationEffect( Caster, Caster.Map, 14201, 16 );
                    }

                    FinishSequence();
                }
        */

        private class InternalTarget : Target
        {
            private MarkSpell m_Owner;

            public InternalTarget(MarkSpell owner)
                : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is RecallRune)
                {
                    m_Owner.Target((RecallRune)o);
                }
                /*
                                else if ( o is Moonstone)
                                {
                                    m_Owner.Target( (Moonstone) o );
                                }
                */
                else
                {
                    from.Send(new MessageLocalized(from.Serial, from.Body, MessageType.Regular, 0x3B2, 3, 501797, from.Name, "")); // I cannot mark that object.
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}