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

/*	/Scripts/Items/Construction/Ruined/RuinedItemSingle.cs
 *	ChangeLog :
 *		7/30/05, erlein
 *			Fixed flipping problem.
 *		7/29/05, erlein
 *			Changed RuinedBookcase to weigh 25.0 stones.
 *			Changed RuinedBookcase to weigh 12.0 stones, haha.
 *
 */

using System;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0xC10, 0xC11)]
	public class RuinedFallenChairA : Item
	{
		[Constructable]
		public RuinedFallenChairA()
			: base(0xC10)
		{
			Movable = false;
		}

		public RuinedFallenChairA(Serial serial)
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

	[FlipableAttribute(0xC13, 0xC12)]
	public class RuinedArmoire : Item
	{
		[Constructable]
		public RuinedArmoire()
			: base(0xC13)
		{
			Movable = false;
		}

		public RuinedArmoire(Serial serial)
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

	[Flipable(0xC14, 0xC15)]
	public class RuinedBookcase : Item
	{

		[Constructable]
		public RuinedBookcase()
			: base(0xC14)
		{
			Movable = false;
			Weight = 12.0;
		}

		public RuinedBookcase(Serial serial)
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

	public class RuinedBooks : Item
	{
		[Constructable]
		public RuinedBooks()
			: base(0xC16)
		{
			Movable = false;
		}

		public RuinedBooks(Serial serial)
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

	[FlipableAttribute(0xC17, 0xC18)]
	public class CoveredChair : Item
	{
		[Constructable]
		public CoveredChair()
			: base(0xC17)
		{
			Movable = false;
		}

		public CoveredChair(Serial serial)
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

	[FlipableAttribute(0xC19, 0xC1A)]
	public class RuinedFallenChairB : Item
	{
		[Constructable]
		public RuinedFallenChairB()
			: base(0xC19)
		{
			Movable = false;
		}

		public RuinedFallenChairB(Serial serial)
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

	[FlipableAttribute(0xC1B, 0xC1C, 0xC1E, 0xC1D)]
	public class RuinedChair : Item
	{
		[Constructable]
		public RuinedChair()
			: base(0xC1B)
		{
			Movable = false;
		}

		public RuinedChair(Serial serial)
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

	public class RuinedClock : Item
	{
		[Constructable]
		public RuinedClock()
			: base(0xC1F)
		{
			Movable = false;
		}

		public RuinedClock(Serial serial)
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

	[FlipableAttribute(0xC24, 0xC25)]
	public class RuinedDrawers : Item
	{
		[Constructable]
		public RuinedDrawers()
			: base(0xC24)
		{
			Movable = false;
		}

		public RuinedDrawers(Serial serial)
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

	public class RuinedPainting : Item
	{
		[Constructable]
		public RuinedPainting()
			: base(0xC2C)
		{
			Movable = false;
		}

		public RuinedPainting(Serial serial)
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

	[FlipableAttribute(0xC2D, 0xC2F, 0xC2E, 0xC30)]
	public class WoodDebris : Item
	{
		[Constructable]
		public WoodDebris()
			: base(0xC2D)
		{
			Movable = false;
		}

		public WoodDebris(Serial serial)
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