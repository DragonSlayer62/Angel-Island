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

/* Scripts/Spells/Fifth/MagicReflect.cs
 * CHANGELOG:
 * 12/26/06, Pix
 *		Added specific checks for Reactive Armor, Protection, and Magic Reflect
 *		so two can't be active at the same time
 *	2/06/06, Pix
 *		Fixed inscribe bonus.
 *	1/29/06, Pix
 *		Fixed integer math problem with Inscribe bonus.
 *	7/26/05, Adam
 *		Massive AOS cleanout
 */

using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Fifth
{
    public class MagicReflectSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Magic Reflection", "In Jux Sanct",
                SpellCircle.Fifth,
                242,
                9012,
                Reagent.Garlic,
                Reagent.MandrakeRoot,
                Reagent.SpidersSilk
            );

        public MagicReflectSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            bool protectionActive = Server.Spells.Second.ProtectionSpell.Registry.Contains(Caster);

            if (Caster.MagicDamageAbsorb > 0)
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
                return false;
            }
            //Pix: 12/26/06 - add explicit check for Reactive Armor
            else if (Caster.MeleeDamageAbsorb > 0)
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
                 //* The magic reflection spell decreases the caster's physical resistance, while increasing the caster's elemental resistances.
                 //* Physical decrease = 25 - (Inscription/20).
                 //* Elemental resistance = +10 (-20 physical, +10 elemental at GM Inscription)
                 //* The magic reflection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast.
                 //* Reactive Armor, Protection, and Magic Reflection will stay on—even after logging out, even after dying—until you “turn them off” by casting them again. 
                 //*

                if ( CheckSequence() )
                {
                    Mobile targ = Caster;

                    ResistanceMod[] mods = (ResistanceMod[])m_Table[targ];

                    if ( mods == null )
                    {
                        targ.PlaySound( 0x1E9 );
                        targ.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );

                        mods = new ResistanceMod[5]
                            {
                                new ResistanceMod( ResistanceType.Physical, -25 + (int)(targ.Skills[SkillName.Inscribe].Value / 20) ),
                                new ResistanceMod( ResistanceType.Fire, 10 ),
                                new ResistanceMod( ResistanceType.Cold, 10 ),
                                new ResistanceMod( ResistanceType.Poison, 10 ),
                                new ResistanceMod( ResistanceType.Energy, 10 )
                            };

                        m_Table[targ] = mods;

                        for ( int i = 0; i < mods.Length; ++i )
                            targ.AddResistanceMod( mods[i] );
                    }
                    else
                    {
                        targ.PlaySound( 0x1ED );
                        targ.FixedParticles( 0x375A, 10, 15, 5037, EffectLayer.Waist );

                        m_Table.Remove( targ );

                        for ( int i = 0; i < mods.Length; ++i )
                            targ.RemoveResistanceMod( mods[i] );
                    }
                }

                FinishSequence();
            }
            else*/
            {
                if (Caster.MagicDamageAbsorb > 0)
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
                        double dValue = 8.0; //8.0 is the base you'll get, whatever your skill levels
                        dValue += (Caster.Skills[SkillName.Inscribe].Value / 99.9) * 6.0; //add +1 per ~16.7 skillpoints (will be +6 at 99.9
                        if (Caster.Skills[SkillName.Inscribe].Value >= 100.0) dValue += 1.0; //Add +1 if AT GM (or above)

                        Caster.MagicDamageAbsorb = (int)dValue;

                        Caster.FixedParticles(0x375A, 10, 15, 5037, EffectLayer.Waist);
                        Caster.PlaySound(0x1E9);
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