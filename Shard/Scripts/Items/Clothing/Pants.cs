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
	public abstract class BasePants : BaseClothing
	{
		public BasePants(int itemID)
			: this(itemID, 0)
		{
		}

		public BasePants(int itemID, int hue)
			: base(itemID, Layer.Pants, hue)
		{
		}

		public BasePants(Serial serial)
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

	[FlipableAttribute(0x152e, 0x152f)]
	public class ShortPants : BasePants
	{
		[Constructable]
		public ShortPants()
			: this(0)
		{
		}

		[Constructable]
		public ShortPants(int hue)
			: base(0x152E, hue)
		{
			Weight = 2.0;
		}

		public ShortPants(Serial serial)
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

	[FlipableAttribute(0x1539, 0x153a)]
	public class LongPants : BasePants
	{
		[Constructable]
		public LongPants()
			: this(0)
		{
		}

		[Constructable]
		public LongPants(int hue)
			: base(0x1539, hue)
		{
			Weight = 2.0;
		}

		public LongPants(Serial serial)
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


}