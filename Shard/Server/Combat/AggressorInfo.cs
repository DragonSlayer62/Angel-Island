/***************************************************************************
 *                              AggressorInfo.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: AggressorInfo.cs,v 1.10 2011/02/24 18:32:34 luket Exp $
 *   $Author: luket $
 *   $Date: 2011/02/24 18:32:34 $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

/* Server/Combat/AggressorInfo.cs
 * CHANGELOG:
 *	10/10/2006, Pix
 *		Changed to use IsInNoMurderZone static calls for nested region handling
 *	6/15/06, Pix
 *		Added InitialAggressionInNoCountZone.
 */

using System;
using System.IO;
using System.Collections;

namespace Server
{
	public class AggressorInfo
	{
		private Mobile m_Attacker, m_Defender;
		private DateTime m_LastCombatTime;
		private bool m_CanReportMurder;
		private bool m_Reported;
		private bool m_CriminalAggression;

		private bool m_bInitialAggressionNotCountable;
		public bool InitialAggressionNotCountable { get { return m_bInitialAggressionNotCountable; } }
		public bool InitialAggressionInNoCountZone { get { return m_bInitialAggressionNotCountable; } }

		private bool m_Queued;

		private static Queue m_Pool = new Queue();

		public static AggressorInfo Create(Mobile attacker, Mobile defender, bool criminal)
		{
			AggressorInfo info;

			if (m_Pool.Count > 0)
			{
				info = (AggressorInfo)m_Pool.Dequeue();

				info.m_Attacker = attacker;
				info.m_Defender = defender;

				info.m_CanReportMurder = criminal;
				info.m_CriminalAggression = criminal;

				info.m_Queued = false;

				info.m_bInitialAggressionNotCountable = Region.IsInitialAggressionNotCountable(attacker, defender);

				info.Refresh();
			}
			else
			{
				info = new AggressorInfo(attacker, defender, criminal);

				info.m_bInitialAggressionNotCountable = Region.IsInitialAggressionNotCountable(attacker, defender);
			}

			return info;
		}

		public void Free()
		{
			if (m_Queued)
				return;

			m_Queued = true;
			m_Pool.Enqueue(this);
		}

		private AggressorInfo(Mobile attacker, Mobile defender, bool criminal)
		{
			m_Attacker = attacker;
			m_Defender = defender;

			m_CanReportMurder = criminal;
			m_CriminalAggression = criminal;

			Refresh();
		}

		private static TimeSpan m_ExpireDelay = TimeSpan.FromMinutes(2.0);

		public static TimeSpan ExpireDelay
		{
			get { return m_ExpireDelay; }
			set { m_ExpireDelay = value; }
		}

		public static void DumpAccess()
		{
			using (StreamWriter op = new StreamWriter("warnings.log", true))
			{
				op.WriteLine("Warning: Access to queued AggressorInfo:");
				op.WriteLine(new System.Diagnostics.StackTrace());
				op.WriteLine();
				op.WriteLine();
			}
		}

		public bool Expired
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return (m_Attacker.Deleted || m_Defender.Deleted || DateTime.Now >= (m_LastCombatTime + m_ExpireDelay));
			}
		}

		public bool CriminalAggression
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_CriminalAggression;
			}
			set
			{
				if (m_Queued)
					DumpAccess();

				m_CriminalAggression = value;
			}
		}

		public Mobile Attacker
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_Attacker;
			}
		}

		public Mobile Defender
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_Defender;
			}
		}

		public DateTime LastCombatTime
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_LastCombatTime;
			}
		}

		public bool Reported
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_Reported;
			}
			set
			{
				if (m_Queued)
					DumpAccess();

				m_Reported = value;
			}
		}

		public bool CanReportMurder
		{
			get
			{
				if (m_Queued)
					DumpAccess();

				return m_CanReportMurder;
			}
			set
			{
				if (m_Queued)
					DumpAccess();

				m_CanReportMurder = value;
			}
		}

		public void Refresh()
		{
			if (m_Queued)
				DumpAccess();

			m_LastCombatTime = DateTime.Now;
			m_Reported = false;
		}
	}
}