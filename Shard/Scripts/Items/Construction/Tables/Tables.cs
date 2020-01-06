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

/* Items/Construction/Tables/Tables.cs
 * ChangeLog:
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;

namespace Server.Items
{
	[Furniture]
	[Flipable(0xB90, 0xB7D)]
	public class LargeTable : Item
	{
		[Constructable]
		public LargeTable()
			: base(0xB90)
		{
			Weight = 1.0;
		}

		public LargeTable(Serial serial)
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

			if (Weight == 4.0)
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB35, 0xB34)]
	public class Nightstand : Item
	{
		[Constructable]
		public Nightstand()
			: base(0xB35)
		{
			Weight = 1.0;
		}

		public Nightstand(Serial serial)
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

			if (Weight == 4.0)
				Weight = 1.0;
		}
	}

	[Furniture]
	[Flipable(0xB8F, 0xB7C)]
	public class YewWoodTable : Item
	{
		[Constructable]
		public YewWoodTable()
			: base(0xB8F)
		{
			Weight = 1.0;
		}

		public YewWoodTable(Serial serial)
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

			if (Weight == 4.0)
				Weight = 1.0;
		}
	}
}