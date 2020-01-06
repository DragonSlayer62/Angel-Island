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

/* Scripts\Spells\Npc\NpcRecall.cs
 * ChangeLog:
 *	6/18/10, Adam
 *		Update region logic to reflect shift from static to new dynamic regions
 *	7/23/05, Adam
 *		Remove all Necromancy, and Chivalry nonsense
 *	5/10/05, Kit
 *		Initial creation
 */

using System;
using Server.Items;
using Server.Multis;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Regions;

namespace Server.Spells.Fourth
{
    public class NpcRecallSpell : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "Recall", "Kal Ort Por",
                SpellCircle.Fourth,
                239,
                9031,
                Reagent.BlackPearl,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot
            );

        private Point3D RecallLocation = new Point3D(0, 0, 0);
        private Map map;
        public NpcRecallSpell(Mobile caster, Item scroll)
            : this(caster, scroll, new Point3D(0, 0, 0))
        {
        }

        public NpcRecallSpell(Mobile caster, Item scroll, Point3D p)
            : base(caster, scroll, m_Info)
        {
            RecallLocation = p;
            map = caster.Map;
        }


        public override void OnCast()
        {
            Effect(RecallLocation, map, true);
        }

        public override bool CheckCast()
        {
            if (Caster.Criminal)
            {
                Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
                return false;
            }
            else if (SpellHelper.CheckCombat(Caster))
            {
                Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
                return false;
            }

            return SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom);
        }

        public void Effect(Point3D loc, Map map, bool checkMulti)
        {
            if (map == null || (!Core.AOS && Caster.Map != map))
            {
                Caster.SendLocalizedMessage(1005569); // You can not recall to another facet.
            }
            else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.RecallFrom))
            {
            }
            else if (!SpellHelper.CheckTravel(Caster, map, loc, TravelCheckType.RecallTo))
            {
            }
            else if (Caster.Murderer && map != Map.Felucca)
            {
                Caster.SendLocalizedMessage(1019004); // You are not allowed to travel there.
            }
            else if (Caster.Criminal)
            {
                Caster.SendLocalizedMessage(1005561, "", 0x22); // Thou'rt a criminal and cannot escape so easily.
            }
            else if (SpellHelper.CheckCombat(Caster))
            {
                Caster.SendLocalizedMessage(1005564, "", 0x22); // Wouldst thou flee during the heat of battle??
            }
            else if (Server.Misc.WeightOverloading.IsOverloaded(Caster))
            {
                Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
            }
            else if (!map.CanSpawnMobile(loc.X, loc.Y, loc.Z))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if ((checkMulti && SpellHelper.CheckMulti(loc, map)))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (CheckSequence())
            {
                if (SpellHelper.IsSpecialRegion(loc) || SpellHelper.IsSpecialRegion(Caster.Location))
                {
                    loc = new Point3D(5295, 1174, 0);
                }

                InternalTimer t = new InternalTimer(this, Caster, loc);
                t.Start();
            }

            FinishSequence();
        }

        private class InternalTimer : Timer
        {
            private Spell m_Spell;
            private Mobile m_Caster;
            private Point3D m_Location;

            public InternalTimer(Spell spell, Mobile caster, Point3D location)
                : base(TimeSpan.FromSeconds(0.75))
            {
                m_Spell = spell;
                m_Caster = caster;
                m_Location = location;

                Priority = TimerPriority.FiftyMS;
            }

            protected override void OnTick()
            {
                // Since all we have is Felucca, we can assume Map.Felucca here
                m_Caster.PlaySound(0x1FC);
                m_Caster.MoveToWorld(m_Location, Map.Felucca);
                m_Caster.PlaySound(0x1FC);
            }
        }
    }
}