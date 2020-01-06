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

/* Scripts/Items/Construction/Chairs/Chairs.cs
 * ChangeLog
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0xB4F, 0xB4E, 0xB50, 0xB51)]
	public class FancyWoodenChairCushion : Item
	{
		[Constructable]
		public FancyWoodenChairCushion()
			: base(0xB4F)
		{
			Weight = 20.0;
		}

		public FancyWoodenChairCushion(Serial serial)
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

			if (Weight == 6.0)
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable(0xB53, 0xB52, 0xB54, 0xB55)]
	public class WoodenChairCushion : Item
	{
		[Constructable]
		public WoodenChairCushion()
			: base(0xB53)
		{
			Weight = 20.0;
		}

		public WoodenChairCushion(Serial serial)
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

			if (Weight == 6.0)
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable(0xB57, 0xB56, 0xB59, 0xB58)]
	public class WoodenChair : Item
	{
		[Constructable]
		public WoodenChair()
			: base(0xB57)
		{
			Weight = 20.0;
		}

		public WoodenChair(Serial serial)
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

			if (Weight == 6.0)
				Weight = 20.0;
		}
	}

	[Furniture]
	[Flipable(0xB5B, 0xB5A, 0xB5C, 0xB5D)]
	public class BambooChair : Item
	{
		[Constructable]
		public BambooChair()
			: base(0xB5B)
		{
			Weight = 20.0;
		}

		public BambooChair(Serial serial)
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

			if (Weight == 6.0)
				Weight = 20.0;
		}
	}

	[DynamicFliping]
	[Flipable(0x1218, 0x1219, 0x121A, 0x121B)]
	public class StoneChair : Item
	{
		[Constructable]
		public StoneChair()
			: base(0x1218)
		{
			Weight = 20;
		}

		public StoneChair(Serial serial)
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