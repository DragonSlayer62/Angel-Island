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
	3/30/05, Kit
		Initial Creation.
*/

using System;
using Server.Targeting;
using Server.Network;
using Server.Regions;
using Server.Items;

namespace Server.Spells.Third
{
    public class GenieTeleport : Spell
    {
        private static SpellInfo m_Info = new SpellInfo(
                "GenieTeleport", "Rel Por",
                SpellCircle.Third,
                215,
                9031,
                Reagent.Bloodmoss,
                Reagent.MandrakeRoot
            );

        public GenieTeleport(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override bool CheckCast()
        {
            if (Server.Misc.WeightOverloading.IsOverloaded(Caster))
            {
                Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
                return false;
            }

            return SpellHelper.CheckTravel(Caster, TravelCheckType.TeleportFrom);
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            IPoint3D orig = p;
            Map map = Caster.Map;

            SpellHelper.GetSurfaceTop(ref p);

            if (Server.Misc.WeightOverloading.IsOverloaded(Caster))
            {
                Caster.SendLocalizedMessage(502359, "", 0x22); // Thou art too encumbered to move.
            }
            else if (!SpellHelper.CheckTravel(Caster, TravelCheckType.TeleportFrom))
            {
            }
            else if (!SpellHelper.CheckTravel(Caster, map, new Point3D(p), TravelCheckType.TeleportTo))
            {
            }
            else if (map == null || !map.CanSpawnMobile(p.X, p.Y, p.Z))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (SpellHelper.CheckMulti(new Point3D(p), map))
            {
                Caster.SendLocalizedMessage(501942); // That location is blocked.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, orig);

                Mobile m = Caster;

                Point3D from = m.Location;
                Point3D to = new Point3D(p);
                m.Location = to;
                m.ProcessDelta();

                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y, from.Z + 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y, from.Z), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y, from.Z - 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X, from.Y + 1, from.Z + 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X, from.Y + 1, from.Z), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X, from.Y + 1, from.Z - 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y + 1, from.Z + 11), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y + 1, from.Z + 7), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y + 1, from.Z + 3), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y + 1, from.Z - 1), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(from.X + 1, from.Y, from.Z + 4), m.Map, 0x3728, 13);



                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y, to.Z), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y, to.Z - 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X, to.Y + 1, to.Z + 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X, to.Y + 1, to.Z), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X, to.Y + 1, to.Z - 4), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y + 1, to.Z + 11), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y + 1, to.Z + 7), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y + 1, to.Z + 3), m.Map, 0x3728, 13);
                Effects.SendLocationEffect(new Point3D(to.X + 1, to.Y + 1, to.Z - 1), m.Map, 0x3728, 13);

                m.PlaySound(0x1FE);
            }

            FinishSequence();
        }

        public class InternalTarget : Target
        {
            private GenieTeleport m_Owner;

            public InternalTarget(GenieTeleport owner)
                : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                IPoint3D p = o as IPoint3D;

                if (p != null)
                    m_Owner.Target(p);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}