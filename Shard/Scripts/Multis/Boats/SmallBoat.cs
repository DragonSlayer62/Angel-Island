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
using Server;
using Server.Items;

namespace Server.Multis
{
	public class SmallBoat : BaseBoat
	{
		public override int NorthID { get { return 0x4000; } }
		public override int EastID { get { return 0x4001; } }
		public override int SouthID { get { return 0x4002; } }
		public override int WestID { get { return 0x4003; } }

		public override int HoldDistance { get { return 4; } }
		public override int TillerManDistance { get { return -4; } }

		public override Point2D StarboardOffset { get { return new Point2D(2, 0); } }
		public override Point2D PortOffset { get { return new Point2D(-2, 0); } }

		public override Point3D MarkOffset { get { return new Point3D(0, 1, 3); } }

		public override BaseDockedBoat DockedBoat { get { return new SmallDockedBoat(this); } }

		[Constructable]
		public SmallBoat()
		{
		}

		public SmallBoat(Serial serial)
			: base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}
	}

	public class SmallBoatDeed : BaseBoatDeed
	{
		public override int LabelNumber { get { return 1041205; } } // small ship deed
		public override BaseBoat Boat { get { return new SmallBoat(); } }

		[Constructable]
		public SmallBoatDeed()
			: base(0x4000, Point3D.Zero)
		{
		}

		public SmallBoatDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}
	}

	public class SmallDockedBoat : BaseDockedBoat
	{
		public override BaseBoat Boat { get { return new SmallBoat(); } }

		public SmallDockedBoat(BaseBoat boat)
			: base(0x4000, Point3D.Zero, boat)
		{
		}

		public SmallDockedBoat(Serial serial)
			: base(serial)
		{
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);
		}
	}
}