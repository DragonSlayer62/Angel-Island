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
	public class SandstoneFireplaceEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new SandstoneFireplaceEastDeed(); } }

		[Constructable]
		public SandstoneFireplaceEastAddon()
		{
			AddComponent(new AddonComponent(0x489), 0, 0, 0);
			AddComponent(new AddonComponent(0x475), 0, 1, 0);
		}

		public SandstoneFireplaceEastAddon(Serial serial)
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

	public class SandstoneFireplaceEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new SandstoneFireplaceEastAddon(); } }
		public override int LabelNumber { get { return 1061844; } } // sandstone fireplace (east)

		[Constructable]
		public SandstoneFireplaceEastDeed()
		{
		}

		public SandstoneFireplaceEastDeed(Serial serial)
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