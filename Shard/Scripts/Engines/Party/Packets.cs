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
using Server.Network;

namespace Server.Engines.PartySystem
{
	public sealed class PartyEmptyList : Packet
	{
		public PartyEmptyList(Mobile m)
			: base(0xBF)
		{
			EnsureCapacity(7);

			m_Stream.Write((short)0x0006);
			m_Stream.Write((byte)0x02);
			m_Stream.Write((byte)0);
			m_Stream.Write((int)m.Serial);
		}
	}

	public sealed class PartyMemberList : Packet
	{
		public PartyMemberList(Party p)
			: base(0xBF)
		{
			EnsureCapacity(7 + p.Count * 4);

			m_Stream.Write((short)0x0006);
			m_Stream.Write((byte)0x01);
			m_Stream.Write((byte)p.Count);

			for (int i = 0; i < p.Count; ++i)
				m_Stream.Write((int)p[i].Mobile.Serial);
		}
	}

	public sealed class PartyRemoveMember : Packet
	{
		public PartyRemoveMember(Mobile removed, Party p)
			: base(0xBF)
		{
			EnsureCapacity(11 + p.Count * 4);

			m_Stream.Write((short)0x0006);
			m_Stream.Write((byte)0x02);
			m_Stream.Write((byte)p.Count);

			m_Stream.Write((int)removed.Serial);

			for (int i = 0; i < p.Count; ++i)
				m_Stream.Write((int)p[i].Mobile.Serial);
		}
	}

	public sealed class PartyTextMessage : Packet
	{
		public PartyTextMessage(bool toAll, Mobile from, string text)
			: base(0xBF)
		{
			if (text == null)
				text = "";

			EnsureCapacity(12 + text.Length * 2);

			m_Stream.Write((short)0x0006);
			m_Stream.Write((byte)(toAll ? 0x04 : 0x03));
			m_Stream.Write((int)from.Serial);
			m_Stream.WriteBigUniNull(text);
		}
	}

	public sealed class PartyInvitation : Packet
	{
		public PartyInvitation(Mobile leader)
			: base(0xBF)
		{
			EnsureCapacity(10);

			m_Stream.Write((short)0x0006);
			m_Stream.Write((byte)0x07);
			m_Stream.Write((int)leader.Serial);
		}
	}
}