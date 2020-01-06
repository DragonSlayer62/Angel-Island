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

namespace Server.Engines.Chat
{
	public sealed class ChatMessagePacket : Packet
	{
		public ChatMessagePacket(Mobile who, int number, string param1, string param2)
			: base(0xB2)
		{
			if (param1 == null)
				param1 = String.Empty;

			if (param2 == null)
				param2 = String.Empty;

			EnsureCapacity(13 + ((param1.Length + param2.Length) * 2));

			m_Stream.Write((ushort)(number - 20));

			if (who != null)
				m_Stream.WriteAsciiFixed(who.Language, 4);
			else
				m_Stream.Write((int)0);

			m_Stream.WriteBigUniNull(param1);
			m_Stream.WriteBigUniNull(param2);
		}
	}
}