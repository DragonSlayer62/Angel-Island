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

/* Items/Construction/Misc/Stautes.cs
 * ChangeLog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	public class StatueSouth : Item
	{
		[Constructable]
		public StatueSouth()
			: base(0x139A)
		{
			Weight = 10;
		}

		public StatueSouth(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueSouth2 : Item
	{
		[Constructable]
		public StatueSouth2()
			: base(0x1227)
		{
			Weight = 10;
		}

		public StatueSouth2(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueNorth : Item
	{
		[Constructable]
		public StatueNorth()
			: base(0x139B)
		{
			Weight = 10;
		}

		public StatueNorth(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueWest : Item
	{
		[Constructable]
		public StatueWest()
			: base(0x1226)
		{
			Weight = 10;
		}

		public StatueWest(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueEast : Item
	{
		[Constructable]
		public StatueEast()
			: base(0x139C)
		{
			Weight = 10;
		}

		public StatueEast(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueEast2 : Item
	{
		[Constructable]
		public StatueEast2()
			: base(0x1224)
		{
			Weight = 10;
		}

		public StatueEast2(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatueSouthEast : Item
	{
		[Constructable]
		public StatueSouthEast()
			: base(0x1225)
		{
			Weight = 10;
		}

		public StatueSouthEast(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class BustSouth : Item
	{
		[Constructable]
		public BustSouth()
			: base(0x12CB)
		{
			Weight = 10;
		}

		public BustSouth(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class BustEast : Item
	{
		[Constructable]
		public BustEast()
			: base(0x12CA)
		{
			Weight = 10;
		}

		public BustEast(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatuePegasus : Item
	{
		[Constructable]
		public StatuePegasus()
			: base(0x139D)
		{
			Weight = 10;
		}

		public StatuePegasus(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class StatuePegasus2 : Item
	{
		[Constructable]
		public StatuePegasus2()
			: base(0x1228)
		{
			Weight = 10;
		}

		public StatuePegasus2(Serial serial)
			: base(serial)
		{
		}

		public override bool ForceShowProperties { get { return ObjectPropertyList.Enabled; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}