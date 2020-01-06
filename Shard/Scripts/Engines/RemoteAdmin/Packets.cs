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

/* Scripts/Engines/RemoteAdmin/Packets.cs
 * Changelog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Items;
using Server.Network;
using Server.Accounting;
using Server.Commands;

namespace Server.Admin
{
	public enum LoginResponse : byte
	{
		NoUser = 0,
		BadIP,
		BadPass,
		NoAccess,
		OK
	}

	public sealed class AdminCompressedPacket : Packet
	{
		public AdminCompressedPacket(byte[] CompData, int CDLen, int unCompSize)
			: base(0x01)
		{
			EnsureCapacity(1 + 2 + 2 + CDLen);
			m_Stream.Write((ushort)unCompSize);
			m_Stream.Write(CompData, 0, CDLen);
		}
	}

	public sealed class Login : Packet
	{
		public Login(LoginResponse resp)
			: base(0x02, 2)
		{
			m_Stream.Write((byte)resp);
		}
	}

	public sealed class ConsoleData : Packet
	{
		public ConsoleData(string str)
			: base(0x03)
		{
			EnsureCapacity(1 + 2 + 1 + str.Length + 1);
			m_Stream.Write((byte)2);

			m_Stream.WriteAsciiNull(str);
		}

		public ConsoleData(char ch)
			: base(0x03)
		{
			EnsureCapacity(1 + 2 + 1 + 1);
			m_Stream.Write((byte)3);

			m_Stream.Write((byte)ch);
		}
	}

	public sealed class ServerInfo : Packet
	{
		public ServerInfo()
			: base(0x04)
		{
			string netVer = Environment.Version.ToString();
			string os = Environment.OSVersion.ToString();

			EnsureCapacity(1 + 2 + (10 * 4) + netVer.Length + 1 + os.Length + 1);
			int banned = 0;
			int active = 0;

			foreach (Account acct in Accounts.Table.Values)
			{
				if (acct.Banned)
					++banned;
				else
					++active;
			}

			m_Stream.Write((int)active);
			m_Stream.Write((int)banned);
			m_Stream.Write((int)Firewall.List.Count);
			m_Stream.Write((int)NetState.Instances.Count);

			m_Stream.Write((int)World.Mobiles.Count);
			m_Stream.Write((int)Core.ScriptMobiles);
			m_Stream.Write((int)World.Items.Count);
			m_Stream.Write((int)Core.ScriptItems);

			m_Stream.Write((uint)(DateTime.Now - Clock.ServerStart).TotalSeconds);
			m_Stream.Write((uint)GC.GetTotalMemory(false));
			m_Stream.WriteAsciiNull(netVer);
			m_Stream.WriteAsciiNull(os);
		}
	}

	public sealed class AccountSearchResults : Packet
	{
		public AccountSearchResults(ArrayList results)
			: base(0x05)
		{
			EnsureCapacity(1 + 2 + 2);

			m_Stream.Write((byte)results.Count);

			foreach (Account a in results)
			{
				m_Stream.WriteAsciiNull(a.Username);

				string pwToSend = a.PlainPassword;

				if (pwToSend == null)
					pwToSend = "(hidden)";

				m_Stream.WriteAsciiNull(pwToSend);
				m_Stream.Write((byte)a.AccessLevel);
				m_Stream.Write(a.Banned);
				unchecked { m_Stream.Write((uint)a.LastLogin.Ticks); }

				m_Stream.Write((ushort)a.LoginIPs.Length);
				for (int i = 0; i < a.LoginIPs.Length; i++)
					m_Stream.WriteAsciiNull(a.LoginIPs[i].ToString());

				m_Stream.Write((ushort)a.IPRestrictions.Length);
				for (int i = 0; i < a.IPRestrictions.Length; i++)
					m_Stream.WriteAsciiNull(a.IPRestrictions[i]);
			}
		}
	}

	public sealed class UOGInfo : Packet
	{
		public UOGInfo(string str)
			: base(0x52, str.Length + 6) // 'R'
		{
			m_Stream.WriteAsciiFixed("unUO", 4);
			m_Stream.WriteAsciiNull(str);
		}
	}

	public sealed class MessageBoxMessage : Packet
	{
		public MessageBoxMessage(string msg, string caption)
			: base(0x08)
		{
			EnsureCapacity(1 + 2 + msg.Length + 1 + caption.Length + 1);

			m_Stream.WriteAsciiNull(msg);
			m_Stream.WriteAsciiNull(caption);
		}
	}
}
