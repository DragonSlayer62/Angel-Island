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
using Server;
using Server.Targeting;

namespace Server.Engines.PartySystem
{
	public class RemovePartyTarget : Target
	{
		public RemovePartyTarget()
			: base(8, false, TargetFlags.None)
		{
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o is Mobile)
			{
				Mobile m = (Mobile)o;
				Party p = Party.Get(from);

				if (p == null || p.Leader != from || !p.Contains(m))
					return;

				if (from == m)
					from.SendLocalizedMessage(1005446); // You may only remove yourself from a party if you are not the leader.
				else
					p.Remove(m);
			}
		}
	}
}