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
	public class LargePainting : Item
	{
		[Constructable]
		public LargePainting()
			: base(0x0EA0)
		{
			Movable = false;
		}

		public LargePainting(Serial serial)
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

	[FlipableAttribute(0x0E9F, 0x0EC8)]
	public class WomanPortrait1 : Item
	{
		[Constructable]
		public WomanPortrait1()
			: base(0x0E9F)
		{
			Movable = false;
		}

		public WomanPortrait1(Serial serial)
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

	[FlipableAttribute(0x0EE7, 0x0EC9)]
	public class WomanPortrait2 : Item
	{
		[Constructable]
		public WomanPortrait2()
			: base(0x0EE7)
		{
			Movable = false;
		}

		public WomanPortrait2(Serial serial)
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

	[FlipableAttribute(0x0EA2, 0x0EA1)]
	public class ManPortrait1 : Item
	{
		[Constructable]
		public ManPortrait1()
			: base(0x0EA2)
		{
			Movable = false;
		}

		public ManPortrait1(Serial serial)
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

	[FlipableAttribute(0x0EA3, 0x0EA4)]
	public class ManPortrait2 : Item
	{
		[Constructable]
		public ManPortrait2()
			: base(0x0EA3)
		{
			Movable = false;
		}

		public ManPortrait2(Serial serial)
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

	[FlipableAttribute(0x0EA6, 0x0EA5)]
	public class LadyPortrait1 : Item
	{
		[Constructable]
		public LadyPortrait1()
			: base(0x0EA6)
		{
			Movable = false;
		}

		public LadyPortrait1(Serial serial)
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

	[FlipableAttribute(0x0EA7, 0x0EA8)]
	public class LadyPortrait2 : Item
	{
		[Constructable]
		public LadyPortrait2()
			: base(0x0EA7)
		{
			Movable = false;
		}

		public LadyPortrait2(Serial serial)
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