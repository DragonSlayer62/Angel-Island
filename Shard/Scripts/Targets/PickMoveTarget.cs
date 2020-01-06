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
    public class PickMoveTarget : Target
    {
        public PickMoveTarget()
            : base(-1, false, TargetFlags.None)
        {
        }

        protected override void OnTarget(Mobile from, object o)
        {
            if (!BaseCommand.IsAccessible(from, o))
            {
                from.SendMessage("That is not accessible.");
                return;
            }

            if (o is Item || o is Mobile)
                from.Target = new MoveTarget(o);
        }
    }
}