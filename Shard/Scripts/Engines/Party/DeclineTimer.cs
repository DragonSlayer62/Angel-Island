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

/* Scripts/Engines/Party/DeclineTimer.cs
 * Changelog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;

namespace Server.Engines.PartySystem
{
	public class DeclineTimer : Timer
	{
		private Mobile m_Mobile, m_Leader;

		private static Hashtable m_Table = new Hashtable();

		public static void Start(Mobile m, Mobile leader)
		{
			DeclineTimer t = (DeclineTimer)m_Table[m];

			if (t != null)
				t.Stop();

			m_Table[m] = t = new DeclineTimer(m, leader);
			t.Start();
		}

		private DeclineTimer(Mobile m, Mobile leader)
			: base(TimeSpan.FromSeconds(10.0))
		{
			m_Mobile = m;
			m_Leader = leader;
		}

		protected override void OnTick()
		{
			m_Table.Remove(m_Mobile);

			if (m_Mobile.Party == m_Leader && PartyCommands.Handler != null)
				PartyCommands.Handler.OnDecline(m_Mobile, m_Leader);
		}
	}
}