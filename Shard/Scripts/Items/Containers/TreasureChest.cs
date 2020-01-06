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

/* Items/Containers/TreasureChest.cs
 * ChangeLog:
 *	11/20/04, Adam
 *		Add L0TreasureChest trainers (for building initial lockpick skill)
 *	6/27/04, adam
 *		Add L2TreasureChest, L3TreasureChest, L4TreasureChest, and L5TreasureChest
 *	6/25/04, adam
 *		Add L1TreasureChest
 */

using Server;
using Server.Items;
using Server.Multis;
using Server.Network;
using System;

namespace Server.Items
{
	[FlipableAttribute(0xe43, 0xe42)]
	public class L0TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L0TreasureChest()
			: base(0)
		{
		}

		public L0TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class L1TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L1TreasureChest()
			: base(1)
		{
		}

		public L1TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class L2TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L2TreasureChest()
			: base(2)
		{
		}

		public L2TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class L3TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L3TreasureChest()
			: base(3)
		{
		}

		public L3TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class L4TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L4TreasureChest()
			: base(4)
		{
		}

		public L4TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class L5TreasureChest : DungeonTreasureChest
	{
		//public override int DefaultGumpID{ get{ return 0x49; } }
		//public override int DefaultDropSound{ get{ return 0x42; } }

		//public override Rectangle2D Bounds
		//{
		//	get{ return new Rectangle2D( 20, 105, 150, 180 ); }
		//}

		[Constructable]
		public L5TreasureChest()
			: base(5)
		{
		}

		public L5TreasureChest(Serial serial)
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

	[FlipableAttribute(0xe43, 0xe42)]
	public class WoodenTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID { get { return 0x49; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(20, 105, 150, 180); }
		}

		[Constructable]
		public WoodenTreasureChest()
			: base(0xE43)
		{
		}

		public WoodenTreasureChest(Serial serial)
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

	[FlipableAttribute(0xe41, 0xe40)]
	public class MetalGoldenTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID { get { return 0x42; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(20, 105, 150, 180); }
		}

		[Constructable]
		public MetalGoldenTreasureChest()
			: base(0xE41)
		{
		}

		public MetalGoldenTreasureChest(Serial serial)
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

	[FlipableAttribute(0x9ab, 0xe7c)]
	public class MetalTreasureChest : BaseTreasureChest
	{
		public override int DefaultGumpID { get { return 0x4A; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(20, 105, 150, 180); }
		}

		[Constructable]
		public MetalTreasureChest()
			: base(0x9AB)
		{
		}

		public MetalTreasureChest(Serial serial)
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