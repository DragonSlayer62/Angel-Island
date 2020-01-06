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

/* Accounting/Firewall.cs
 * CHANGELOG:
 *	2/15/05 - Pix
 *		Initial version for 1.0.0
 */

using System;
using System.IO;
using System.Collections;
using System.Net;

namespace Server
{
	public class Firewall
	{
		private static ArrayList m_Blocked;

		static Firewall()
		{
			m_Blocked = new ArrayList();

			string path = "firewall.cfg";

			if (File.Exists(path))
			{
				using (StreamReader ip = new StreamReader(path))
				{
					string line;

					while ((line = ip.ReadLine()) != null)
					{
						line = line.Trim();

						if (line.Length == 0)
							continue;

						object toAdd;

						try { toAdd = IPAddress.Parse(line); }
						catch { toAdd = line; }

						m_Blocked.Add(toAdd.ToString());
					}
				}
			}
		}

		public static ArrayList List
		{
			get
			{
				return m_Blocked;
			}
		}

		public static void RemoveAt(int index)
		{
			m_Blocked.RemoveAt(index);
			Save();
		}

		public static void Remove(string pattern)
		{
			m_Blocked.Remove(pattern);
			Save();
		}

		public static void Remove(IPAddress ip)
		{
			m_Blocked.Remove(ip);
			Save();
		}

		public static void Add(object obj)
		{
			if (!(obj is IPAddress) && !(obj is String))
				return;

			if (!m_Blocked.Contains(obj))
				m_Blocked.Add(obj);

			Save();
		}

		public static void Add(string pattern)
		{
			if (!m_Blocked.Contains(pattern))
				m_Blocked.Add(pattern);

			Save();
		}

		public static void Add(IPAddress ip)
		{
			if (!m_Blocked.Contains(ip))
				m_Blocked.Add(ip);

			Save();
		}

		public static void Save()
		{
			string path = "firewall.cfg";

			using (StreamWriter op = new StreamWriter(path))
			{
				for (int i = 0; i < m_Blocked.Count; ++i)
					op.WriteLine(m_Blocked[i]);
			}
		}

		public static bool IsBlocked(IPAddress ip)
		{
			bool contains = false;

			for (int i = 0; !contains && i < m_Blocked.Count; ++i)
			{
				if (m_Blocked[i] is IPAddress)
					contains = ip.Equals(m_Blocked[i]);
				else if (m_Blocked[i] is String)
					contains = Utility.IPMatch((string)m_Blocked[i], ip);
			}

			return contains;
		}
	}
}