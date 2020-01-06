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
	6/5/04, Pix
		Merged in 1.0RC0 code.
*/

using System;
using Server;
using Server.Targeting;
using Server.Commands;

namespace Server.Targets
{
    public class MoveTarget : Target
    {
        private object m_Object;

        public MoveTarget(object o)
            : base(-1, true, TargetFlags.None)
        {
            m_Object = o;
        }

        protected override void OnTarget(Mobile from, object o)
        {
            IPoint3D p = o as IPoint3D;

            if (p != null)
            {
                if (!BaseCommand.IsAccessible(from, m_Object))
                {
                    from.SendMessage("That is not accessible.");
                    return;
                }

                if (p is Item)
                    p = ((Item)p).GetWorldTop();

                Server.Commands.CommandLogging.WriteLine(from, "{0} {1} moving {2} to {3}", from.AccessLevel, Server.Commands.CommandLogging.Format(from), Server.Commands.CommandLogging.Format(m_Object), new Point3D(p));

                if (m_Object is Item)
                {
                    Item item = (Item)m_Object;

                    if (!item.Deleted)
                        item.MoveToWorld(new Point3D(p), from.Map);
                }
                else if (m_Object is Mobile)
                {
                    Mobile m = (Mobile)m_Object;

                    if (!m.Deleted)
                        m.MoveToWorld(new Point3D(p), from.Map);
                }
            }
        }
    }
}