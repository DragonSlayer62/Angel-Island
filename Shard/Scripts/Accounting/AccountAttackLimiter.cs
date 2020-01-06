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
using System.Net;
using System.Net.Sockets;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Accounting
{
	public class AccountAttackLimiter
	{
		public static bool Enabled = true;

		public static void Initialize()
		{
			if (!Enabled)
				return;

			RegisterThrottler(0x80);
			RegisterThrottler(0x91);
			RegisterThrottler(0xCF);
		}

		public static void RegisterThrottler(int packetID)
		{
			PacketHandler ph = PacketHandlers.GetHandler(packetID);

			if (ph == null)
				return;

			ph.ThrottleCallback = new ThrottlePacketCallback(Throttle_Callback);
		}

		public static bool Throttle_Callback(NetState ns)
		{
			InvalidAccountAccessLog accessLog = FindAccessLog(ns);

			if (accessLog == null)
				return true;

			return (DateTime.Now >= (accessLog.LastAccessTime + ComputeThrottle(accessLog.Counts)));
		}

		private static ArrayList m_List = new ArrayList();

		public static InvalidAccountAccessLog FindAccessLog(NetState ns)
		{
			if (ns == null)
				return null;

			IPAddress ipAddress = ns.Address;

			for (int i = 0; i < m_List.Count; ++i)
			{
				InvalidAccountAccessLog accessLog = (InvalidAccountAccessLog)m_List[i];

				if (accessLog.HasExpired)
					m_List.RemoveAt(i--);
				else if (accessLog.Address.Equals(ipAddress))
					return accessLog;
			}

			return null;
		}

		public static void RegisterInvalidAccess(NetState ns)
		{
			if (ns == null || !Enabled)
				return;

			InvalidAccountAccessLog accessLog = FindAccessLog(ns);

			if (accessLog == null)
				m_List.Add(accessLog = new InvalidAccountAccessLog(ns.Address));

			accessLog.Counts += 1;
			accessLog.RefreshAccessTime();
		}

		public static TimeSpan ComputeThrottle(int counts)
		{
			if (counts >= 15)
				return TimeSpan.FromMinutes(5.0);

			if (counts >= 10)
				return TimeSpan.FromMinutes(1.0);

			if (counts >= 5)
				return TimeSpan.FromSeconds(20.0);

			if (counts >= 3)
				return TimeSpan.FromSeconds(10.0);

			if (counts >= 1)
				return TimeSpan.FromSeconds(2.0);

			return TimeSpan.Zero;
		}
	}

	public class InvalidAccountAccessLog
	{
		private IPAddress m_Address;
		private DateTime m_LastAccessTime;
		private int m_Counts;

		public IPAddress Address
		{
			get { return m_Address; }
			set { m_Address = value; }
		}

		public DateTime LastAccessTime
		{
			get { return m_LastAccessTime; }
			set { m_LastAccessTime = value; }
		}

		public bool HasExpired
		{
			get { return (DateTime.Now >= (m_LastAccessTime + TimeSpan.FromHours(1.0))); }
		}

		public int Counts
		{
			get { return m_Counts; }
			set { m_Counts = value; }
		}

		public void RefreshAccessTime()
		{
			m_LastAccessTime = DateTime.Now;
		}

		public InvalidAccountAccessLog(IPAddress address)
		{
			m_Address = address;
			RefreshAccessTime();
		}
	}
}