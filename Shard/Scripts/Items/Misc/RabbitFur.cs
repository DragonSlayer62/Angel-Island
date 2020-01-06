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

/* Items/RabbitFur.cs
 * ChangeLog:
 *	3/26/05, Adam
 *		First time checkin
 */

using System;

namespace Server.Items
{
	public class RabbitFur1 : Item
	{
		[Constructable]
		public RabbitFur1()
			: base(0x11F4)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur1(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur2 : Item
	{
		[Constructable]
		public RabbitFur2()
			: base(0x11F5)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur2(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur3 : Item
	{
		[Constructable]
		public RabbitFur3()
			: base(0x11F6)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur3(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur4 : Item
	{
		[Constructable]
		public RabbitFur4()
			: base(0x11F7)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur4(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur5 : Item
	{
		[Constructable]
		public RabbitFur5()
			: base(0x11F8)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur5(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur6 : Item
	{
		[Constructable]
		public RabbitFur6()
			: base(0x11F9)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur6(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur7 : Item
	{
		[Constructable]
		public RabbitFur7()
			: base(0x11FA)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur7(Serial serial)
			: base(serial)
		{
		}

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

	public class RabbitFur8 : Item
	{
		[Constructable]
		public RabbitFur8()
			: base(0x11FB)
		{
			Name = "rabbit fur";
			Weight = 1.0;
		}

		public RabbitFur8(Serial serial)
			: base(serial)
		{
		}

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