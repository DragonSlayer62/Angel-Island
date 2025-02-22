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

/* Scripts/Spells/First/Protection.cs
 * CHANGELOG:
 * 12/26/06, Pix
 *		Added specific checks for Reactive Armor, Protection, and Magic Reflect
 *		so two can't be active at the same time
 *	7/26/05, Adam
 *		Massive AOS cleanout
 */

using System;
using System.Collections;
using Server.Targeting;
using Server.Network;

namespace Server.Spells.Second
{
    public class ProtectionSpell : Spell
    {
        private static Hashtable m_Registry = new Hashtable();
        public static Hashtable Registry { get { return m_Registry; } }

        private static SpellInfo m_Info = new SpellInfo(
                "Protection", "Uus Sanct",
                SpellCircle.Second,
                236,
                9011,
                Reagent.Garlic,
                Reagent.Ginseng,
                Reagent.SulfurousAsh
            );

        public ProtectionSpell(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

		/*
		 * The protection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast. 
		 * This spell, along with Reactive Armor and Magic Reflection, will stay on�even after logging out �- until you �turn it off� by casting it again. 
		 * The spell effect will be purged when dying, and Protection has to be re-cast after death.
		 * http://www.uoguide.com/Protection
		 */
		public override bool CheckCast()
        {
			if (Core.AOS || true) // this is the behavior we want for all of our shards
                return true;

            if (m_Registry.ContainsKey(Caster))
            {
                Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
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

        public static void Toggle(Mobile caster, Mobile target)
        {
            /* Players under the protection spell effect can no longer have their spells "disrupted" when hit.
             * Players under the protection spell have decreased physical resistance stat value,
             * a decreased "resisting spells" skill value by -35,
             * and a slower casting speed modifier (technically, a negative "faster cast speed") of 2 points.
             * The protection spell has an indefinite duration, becoming active when cast, and deactivated when re-cast.
             * Reactive Armor, Protection, and Magic Reflection will stay on�even after logging out,
             * even after dying�until you �turn them off� by casting them again.
             */

            object[] mods = (object[])m_Table[target];

            if (mods == null)
            {
                target.PlaySound(0x1E9);
                target.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);

                mods = new object[1]
					{
					//	new ResistanceMod( ResistanceType.Physical, -15 + Math.Min( (int)(caster.Skills[SkillName.Inscribe].Value / 20), 15 ) ),
						new DefaultSkillMod( SkillName.MagicResist, true, -35 + (int)(caster.Skills[SkillName.Inscribe].Value / 20) )
					};

                m_Table[target] = mods;
                Registry[target] = 100.0;

			//	target.AddResistanceMod( (ResistanceMod)mods[0] );
                target.AddSkillMod((SkillMod)mods[0]);

			//	int physloss = -15 + (int)(caster.Skills[SkillName.Inscribe].Value / 20);
			//	int resistloss = -35 + (int)(caster.Skills[SkillName.Inscribe].Value / 20);
			//	string args = String.Format("{0}\t{1}", physloss, resistloss);
			//	BuffInfo.AddBuff(target, new BuffInfo(BuffIcon.Protection, 1075814, 1075815, args.ToString()));
            }
            else
            {
                target.PlaySound(0x1ED);
                target.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);

                m_Table.Remove(target);
                Registry.Remove(target);

			//	target.RemoveResistanceMod((ResistanceMod)mods[0]);
				target.RemoveSkillMod((SkillMod)mods[0]);

			//	BuffInfo.RemoveBuff(target, BuffIcon.Protection);
            }
        }

        public override void OnCast()
        {
			if (Core.AOS || true)	// this is the behavior we want for all of our shards
			{
				if ( CheckSequence() )
					Toggle( Caster, Caster );

				FinishSequence();
			}
			else
			{
				if (m_Registry.ContainsKey(Caster))
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
						double value = (int)(Caster.Skills[SkillName.EvalInt].Value + Caster.Skills[SkillName.Meditation].Value + Caster.Skills[SkillName.Inscribe].Value);
						value /= 4;

						if (value < 0)
							value = 0;
						else if (value > 75)
							value = 75.0;

						Registry.Add(Caster, value);
						new InternalTimer(Caster).Start();

						Caster.FixedParticles(0x375A, 9, 20, 5016, EffectLayer.Waist);
						Caster.PlaySound(0x1ED);
					}
					else
					{
						Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
					}
				}

				FinishSequence();
            }
        }

        private class InternalTimer : Timer
        {
            private Mobile m_Caster;

            public InternalTimer(Mobile caster)
                : base(TimeSpan.FromSeconds(0))
            {
                double val = caster.Skills[SkillName.Magery].Value * 2.0;
                if (val < 15)
                    val = 15;
                else if (val > 240)
                    val = 240;

                m_Caster = caster;
                Delay = TimeSpan.FromSeconds(val);
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                ProtectionSpell.Registry.Remove(m_Caster);
                DefensiveSpell.Nullify(m_Caster);
            }
        }
    }
}


/*
//Pix: 12/26/06 - add explicit check for Reactive Armor
				else if (Caster.MeleeDamageAbsorb > 0)
				{
					Caster.SendLocalizedMessage(1005559); // This spell is already in effect.
				}
				//Pix: 12/26/06 - add explicit check for Magic Reflect
				else if (Caster.MagicDamageAbsorb > 0)
				{
					Caster.SendLocalizedMessage(1005385); // The spell will not adhere to you at this time.
				}
*/