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
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0x155E, 0x155F, 0x155C, 0x155D)]
	public class DecorativeBowWest : Item
	{
		[Constructable]
		public DecorativeBowWest()
			: base(Utility.Random(0x155E, 2))
		{
			Movable = false;
		}

		public DecorativeBowWest(Serial serial)
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

	[FlipableAttribute(0x155C, 0x155D, 0x155E, 0x155F)]
	public class DecorativeBowNorth : Item
	{
		[Constructable]
		public DecorativeBowNorth()
			: base(Utility.Random(0x155C, 2))
		{
			Movable = false;
		}

		public DecorativeBowNorth(Serial serial)
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

	[FlipableAttribute(0x1560, 0x1561, 0x1562, 0x1563)]
	public class DecorativeAxeNorth : Item
	{
		[Constructable]
		public DecorativeAxeNorth()
			: base(Utility.Random(0x1560, 2))
		{
			Movable = false;
		}

		public DecorativeAxeNorth(Serial serial)
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

	[FlipableAttribute(0x1562, 0x1563, 0x1560, 0x1561)]
	public class DecorativeAxeWest : Item
	{
		[Constructable]
		public DecorativeAxeWest()
			: base(Utility.Random(0x1562, 2))
		{
			Movable = false;
		}

		public DecorativeAxeWest(Serial serial)
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

	public class DecorativeSwordNorth : Item
	{
		private InternalItem m_Item;

		[Constructable]
		public DecorativeSwordNorth()
			: base(0x1565)
		{
			Movable = false;

			m_Item = new InternalItem(this);
		}

		public DecorativeSwordNorth(Serial serial)
			: base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X - 1, Y, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}
		private class InternalItem : Item
		{
			private DecorativeSwordNorth m_Item;

			public InternalItem(DecorativeSwordNorth item)
				: base(0x1564)
			{
				Movable = true;

				m_Item = item;
			}

			public InternalItem(Serial serial)
				: base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X + 1, Y, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as DecorativeSwordNorth;
			}
		}
	}
	public class DecorativeSwordWest : Item
	{
		private InternalItem m_Item;

		[Constructable]
		public DecorativeSwordWest()
			: base(0x1566)
		{
			Movable = false;

			m_Item = new InternalItem(this);
		}

		public DecorativeSwordWest(Serial serial)
			: base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X, Y - 1, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}
		private class InternalItem : Item
		{
			private DecorativeSwordWest m_Item;

			public InternalItem(DecorativeSwordWest item)
				: base(0x1567)
			{
				Movable = true;

				m_Item = item;
			}

			public InternalItem(Serial serial)
				: base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X, Y + 1, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as DecorativeSwordWest;
			}
		}
	}
	public class DecorativeDAxeNorth : Item
	{
		private InternalItem m_Item;

		[Constructable]
		public DecorativeDAxeNorth()
			: base(0x1569)
		{
			Movable = false;

			m_Item = new InternalItem(this);
		}

		public DecorativeDAxeNorth(Serial serial)
			: base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X - 1, Y, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}
		private class InternalItem : Item
		{
			private DecorativeDAxeNorth m_Item;

			public InternalItem(DecorativeDAxeNorth item)
				: base(0x1568)
			{
				Movable = true;

				m_Item = item;
			}

			public InternalItem(Serial serial)
				: base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X + 1, Y, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as DecorativeDAxeNorth;
			}
		}
	}
	public class DecorativeDAxeWest : Item
	{
		private InternalItem m_Item;

		[Constructable]
		public DecorativeDAxeWest()
			: base(0x156A)
		{
			Movable = false;

			m_Item = new InternalItem(this);
		}

		public DecorativeDAxeWest(Serial serial)
			: base(serial)
		{
		}

		public override void OnLocationChange(Point3D oldLocation)
		{
			if (m_Item != null)
				m_Item.Location = new Point3D(X, Y - 1, Z);
		}

		public override void OnMapChange()
		{
			if (m_Item != null)
				m_Item.Map = Map;
		}

		public override void OnAfterDelete()
		{
			base.OnAfterDelete();

			if (m_Item != null)
				m_Item.Delete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Item);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			m_Item = reader.ReadItem() as InternalItem;
		}
		private class InternalItem : Item
		{
			private DecorativeDAxeWest m_Item;

			public InternalItem(DecorativeDAxeWest item)
				: base(0x156B)
			{
				Movable = true;

				m_Item = item;
			}

			public InternalItem(Serial serial)
				: base(serial)
			{
			}

			public override void OnLocationChange(Point3D oldLocation)
			{
				if (m_Item != null)
					m_Item.Location = new Point3D(X, Y + 1, Z);
			}

			public override void OnMapChange()
			{
				if (m_Item != null)
					m_Item.Map = Map;
			}

			public override void OnAfterDelete()
			{
				base.OnAfterDelete();

				if (m_Item != null)
					m_Item.Delete();
			}

			public override void Serialize(GenericWriter writer)
			{
				base.Serialize(writer);

				writer.Write((int)0); // version

				writer.Write(m_Item);
			}

			public override void Deserialize(GenericReader reader)
			{
				base.Deserialize(reader);

				int version = reader.ReadInt();

				m_Item = reader.ReadItem() as DecorativeDAxeWest;
			}
		}
	}
}