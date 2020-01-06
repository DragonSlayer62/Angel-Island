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

/* Scripts/Construction/Doors/Portcullis.cs
 * CHANGELOG
 * 
 *	9/01/06 Taran Kain
 *		Modified constructors to fit new BaseDoor constructor
 */

using System;

namespace Server.Items
{
	public class PortcullisNS : BaseDoor
	{
		public override bool UseChainedFunctionality { get { return true; } }

		[Constructable]
		public PortcullisNS()
			: base(0, 0, 0x6F5, 0x6F5, 0xF0, 0xEF, DoorFacing.EastCCW, new Point3D(0, 0, 20))
		{
		}

		public PortcullisNS(Serial serial)
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

	public class PortcullisEW : BaseDoor
	{
		public override bool UseChainedFunctionality { get { return true; } }

		[Constructable]
		public PortcullisEW()
			: base(0, 0, 0x6F6, 0x6F6, 0xF0, 0xEF, DoorFacing.EastCCW, new Point3D(0, 0, 20))
		{
		}

		public PortcullisEW(Serial serial)
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