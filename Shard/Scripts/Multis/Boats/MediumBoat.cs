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
	public class MediumBoat : BaseBoat
	{
		public override int NorthID { get { return 0x4008; } }
		public override int EastID { get { return 0x4009; } }
		public override int SouthID { get { return 0x400A; } }
		public override int WestID { get { return 0x400B; } }

		public override int HoldDistance { get { return 4; } }
		public override int TillerManDistance { get { return -5; } }

		public override Point2D StarboardOffset { get { return new Point2D(2, 0); } }
		public override Point2D PortOffset { get { return new Point2D(-2, 0); } }

		public override Point3D MarkOffset { get { return new Point3D(0, 1, 3); } }

		public override BaseDockedBoat DockedBoat { get { return new MediumDockedBoat(this); } }

		[Constructable]
		public MediumBoat()
		{
		}

		public MediumBoat(Serial serial)
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

	public class MediumBoatDeed : BaseBoatDeed
	{
		public override int LabelNumber { get { return 1041207; } } // medium ship deed
		public override BaseBoat Boat { get { return new MediumBoat(); } }

		[Constructable]
		public MediumBoatDeed()
			: base(0x4008, Point3D.Zero)
		{
		}

		public MediumBoatDeed(Serial serial)
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

	public class MediumDockedBoat : BaseDockedBoat
	{
		public override BaseBoat Boat { get { return new MediumBoat(); } }

		public MediumDockedBoat(BaseBoat boat)
			: base(0x4008, Point3D.Zero, boat)
		{
		}

		public MediumDockedBoat(Serial serial)
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