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
using Server.Targeting;
using Server.Network;

/* Scripts/Spells/First/ReactivateArmor.cs
 * CHANGELOG:
 * 12/26/06, Pix
 *		Added specific checks for Reactive Armor, Protection, and Magic Reflect
 *		so two can't be active at the same time
 *	7/26/05, Adam
 *		Massive AOS cleanout
 */

namespace Server.Spells.First
{
    public class ReactiveArmorSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Reactive Armor", "Flam Sanct",
                SpellCircle.First,
                236,
                9011,
                Reagent.Garlic,
                Reagent.SpidersSilk,
                Reagent.SulfurousAsh
            );

        public ReactiveArmorSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            bool protectionActive = Server.Spells.Second.ProtectionSpell.Registry.Contains(Caster);

            if (Caster.MeleeDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }
            //Pix: 12/26/06 - add explicit check for Magic Reflect
            else if (Caster.MagicDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                return false;
            }
            //Pix: 12/26/06 - add explicit check for Protection
            else if (protectionActive)
            {
                Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                return false;
            }
            else if (!Caster.CanBeginAction(typeof(DefensiveSpell)))
            {
                Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                return false;
            }

            return true;
        }

        private static Hashtable m_Table = new Hashtable();

        public override void OnCast()
        {
            /*if ( Core.AOS )
            {
                 // The reactive armor spell increases the caster's physical resistance, while lowering the caster's elemental resistances.
                 //* 15 + (Inscription/20) Physcial bonus
                 //* -5 Elemental
                 //* The reactive armor spell has an indefinite duration, becoming active when cast, and deactivated when re-cast. 
                 //* Reactive Armor, Protection, and Magic Reflection will stay on�even after logging out, even after dying�until you �turn them off� by casting them again. 
                 //* (+20 physical -5 elemental at 100 Inscription)
                 //

                if ( CheckSequence() )
                {
                    Mobile targ = Caster;

                    ResistanceMod[] mods = (ResistanceMod[])m_Table[targ];

                    if ( mods == null )
                    {
                        targ.PlaySound( 0x1E9 );
                        targ.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

                        mods = new ResistanceMod[5]
                            {
                                new ResistanceMod( ResistanceType.Physical, 15 + (int)(targ.Skills[SkillName.Inscribe].Value / 20) ),
                                new ResistanceMod( ResistanceType.Fire, -5 ),
                                new ResistanceMod( ResistanceType.Cold, -5 ),
                                new ResistanceMod( ResistanceType.Poison, -5 ),
                                new ResistanceMod( ResistanceType.Energy, -5 )
                            };

                        m_Table[targ] = mods;

                        for ( int i = 0; i < mods.Length; ++i )
                            targ.AddResistanceMod( mods[i] );
                    }
                    else
                    {
                        targ.PlaySound( 0x1ED );
                        targ.FixedParticles( 0x376A, 9, 32, 5008, EffectLayer.Waist );

                        m_Table.Remove( targ );

                        for ( int i = 0; i < mods.Length; ++i )
                            targ.RemoveResistanceMod( mods[i] );
                    }
                }

                FinishSequence();
            }
            else*/
            {
                if (Caster.MeleeDamageAbsorb > 0)
                {
                    Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                }
                else if (!Caster.CanBeginAction(typeof(DefensiveSpell)))
                {
                    Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                }
                else if (CheckSequence())
                {
                    if (Caster.BeginAction(typeof(DefensiveSpell)))
                    {
                        int value = (int)(Caster.Skills[SkillName.Magery].Value + Caster.Skills[SkillName.Meditation].Value + Caster.Skills[SkillName.Inscribe].Value);
                        value /= 3;

                        if (value < 0)
                            value = 1;
                        else if (value > 75)
                            value = 75;

                        Caster.MeleeDamageAbsorb = value;

                        Caster.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        Caster.PlaySound(0x1F2);
                    }
                    else
                    {
                        Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
                    }
                }

                FinishSequence();
            }
        }
    }
}