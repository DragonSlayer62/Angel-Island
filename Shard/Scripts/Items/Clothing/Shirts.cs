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

namespace Server.Items
{

	public abstract class BaseShirt : BaseClothing
	{
		public BaseShirt(int itemID)
			: this(itemID, 0)
		{
		}

		public BaseShirt(int itemID, int hue)
			: base(itemID, Layer.Shirt, hue)
		{
		}

		public BaseShirt(Serial serial)
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

	[FlipableAttribute(0x1efd, 0x1efe)]
	public class FancyShirt : BaseShirt
	{
		[Constructable]
		public FancyShirt()
			: this(0)
		{
		}

		[Constructable]
		public FancyShirt(int hue)
			: base(0x1EFD, hue)
		{
			Weight = 2.0;
		}

		public FancyShirt(Serial serial)
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

	[FlipableAttribute(0x1517, 0x1518)]
	public class Shirt : BaseShirt
	{
		[Constructable]
		public Shirt()
			: this(0)
		{
		}

		[Constructable]
		public Shirt(int hue)
			: base(0x1517, hue)
		{
			Weight = 1.0;
		}

		public Shirt(Serial serial)
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

			if (Weight == 2.0)
				Weight = 1.0;
		}
	}
}