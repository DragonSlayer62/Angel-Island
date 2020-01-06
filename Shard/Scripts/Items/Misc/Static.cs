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

namespace Server.Items
{
	public class Static : Item
	{
		[Constructable]
		public Static(int itemID)
			: base(itemID)
		{
			Movable = false;
		}

		[Constructable]
		public Static(int itemID, int count)
			: this(Utility.Random(itemID, count))
		{
		}

		public Static(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class LocalizedStatic : Static
	{
		private int m_LabelNumber;

		[CommandProperty(AccessLevel.GameMaster)]
		public int Number
		{
			get { return m_LabelNumber; }
			set { m_LabelNumber = value; InvalidateProperties(); }
		}

		public override int LabelNumber { get { return m_LabelNumber; } }

		[Constructable]
		public LocalizedStatic(int itemID)
			: this(itemID, 1020000 + itemID)
		{
		}

		[Constructable]
		public LocalizedStatic(int itemID, int labelNumber)
			: base(itemID)
		{
			m_LabelNumber = labelNumber;
		}

		public LocalizedStatic(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((byte)0); // version
			writer.WriteEncodedInt((int)m_LabelNumber);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadByte();

			switch (version)
			{
				case 0:
					{
						m_LabelNumber = reader.ReadEncodedInt();
						break;
					}
			}
		}
	}
}