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

/* Scripts/Items/Skill Items/Fishing/Misc/MessageInABottle.cs
 * CHANGELOG:
 *	9/3/10, Adam
 *		Add in a notion of level
 *	6/29/04 - Pix
 *		Temporary fix for Mibs/SOSs set for Trammel
 */

using System;
using Server.Network;
using Server.Items;

namespace Server.Items
{
	public class MessageInABottle : Item
	{
		public override int LabelNumber { get { return 1041080; } } // a message in a bottle

		private Map m_TargetMap;
		private int m_level;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Level
		{
			get { return m_level; }
			set { m_level = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Map TargetMap
		{
			get { return m_TargetMap; }
			set { m_TargetMap = value; }
		}

		[Constructable]
		public MessageInABottle()
			: this(Map.Felucca)
		{
		}

		[Constructable]
		public MessageInABottle(Map map)
			: this(map, 0)
		{
			Weight = 1.0;
			m_TargetMap = map;
		}

		[Constructable]
		public MessageInABottle(Map map, int level)
			: base(0x099F)
		{
			Weight = 1.0;
			m_level = level;
			m_TargetMap = map;
		}

		public MessageInABottle(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)2); // version

			// version 2
			writer.Write(m_level);

			// version 1
			writer.Write(m_TargetMap);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
					{
						m_level = reader.ReadInt();
						goto case 1;
					}
				case 1:
					{
						m_TargetMap = reader.ReadMap();
						break;
					}
				case 0:
					{
						m_TargetMap = Map.Trammel;
						break;
					}
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (IsChildOf(from.Backpack))
			{
				Consume();
				from.AddToBackpack(new SOS(m_TargetMap, m_level));
				from.LocalOverheadMessage(Network.MessageType.Regular, 0x3B2, 501891); // You extract the message from the bottle.
			}
			else
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
		}
	}
}