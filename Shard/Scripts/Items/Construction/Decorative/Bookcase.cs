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

/* Scripts/Items/Construction/Decorative/Bookcase.cs
 * CHANGELOG:
 *	06/04/05 : Pixie
 *		Initial Version
 */

using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0xA97, 0xA99)]
	public class Bookcase : Item
	{
		[Constructable]
		public Bookcase()
			: base(0xA97)
		{
			Weight = 15.0;
		}
		public Bookcase(Serial serial)
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

	[Furniture]
	[Flipable(0xA98, 0xA9A)]
	public class Bookcase2 : Item
	{
		[Constructable]
		public Bookcase2()
			: base(0xA98)
		{
			Weight = 15.0;
		}
		public Bookcase2(Serial serial)
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

	[Furniture]
	[Flipable(0xA9B, 0xA9C)]
	public class Bookcase3 : Item
	{
		[Constructable]
		public Bookcase3()
			: base(0xA9C)
		{
			Weight = 15.0;
		}
		public Bookcase3(Serial serial)
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
