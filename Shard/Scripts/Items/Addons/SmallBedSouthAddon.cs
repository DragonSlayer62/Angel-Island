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

namespace Server.Items
{
	public class SmallBedSouthAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SmallBedSouthDeed(); } }

		[Constructable]
		public SmallBedSouthAddon()
		{
			AddComponent(new AddonComponent(0xA63), 0, 0, 0);
			AddComponent(new AddonComponent(0xA5C), 0, 1, 0);
		}

		public SmallBedSouthAddon(Serial serial)
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

	public class SmallBedSouthDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SmallBedSouthAddon(); } }
		public override int LabelNumber { get { return 1044321; } } // small bed (south)

		[Constructable]
		public SmallBedSouthDeed()
		{
		}

		public SmallBedSouthDeed(Serial serial)
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