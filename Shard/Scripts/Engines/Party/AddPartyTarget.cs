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
	public class AddPartyTarget : Target
	{
		public AddPartyTarget(Mobile from)
			: base(8, false, TargetFlags.None)
		{
			from.SendLocalizedMessage(1005454); // Who would you like to add to your party?
		}

		protected override void OnTarget(Mobile from, object o)
		{
			if (o is Mobile)
			{
				Mobile m = (Mobile)o;
				Party p = Party.Get(from);
				Party mp = Party.Get(m);

				if (from == m)
					from.SendLocalizedMessage(1005439); // You cannot add yourself to a party.
				else if (p != null && p.Leader != from)
					from.SendLocalizedMessage(1005453); // You may only add members to the party if you are the leader.
				else if (m.Party is Mobile)
					return;
				else if (p != null && (p.Members.Count + p.Candidates.Count) >= Party.Capacity)
					from.SendLocalizedMessage(1008095); // You may only have 10 in your party (this includes candidates).
				else if (!m.Player && m.Body.IsHuman)
					m.SayTo(from, 1005443); // Nay, I would rather stay here and watch a nail rust.
				else if (!m.Player)
					from.SendLocalizedMessage(1005444); // The creature ignores your offer.
				else if (mp != null && mp == p)
					from.SendLocalizedMessage(1005440); // This person is already in your party!
				else if (mp != null)
					from.SendLocalizedMessage(1005441); // This person is already in a party!
				else
					Party.Invite(from, m);
			}
			else
			{
				from.SendLocalizedMessage(1005442); // You may only add living things to your party!
			}
		}
	}
}