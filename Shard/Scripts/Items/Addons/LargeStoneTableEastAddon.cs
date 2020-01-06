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
	public class LargeStoneTableEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeStoneTableEastDeed(); } }

		public override bool RetainDeedHue { get { return true; } }

		[Constructable]
		public LargeStoneTableEastAddon()
			: this(0)
		{
		}

		[Constructable]
		public LargeStoneTableEastAddon(int hue)
		{
			AddComponent(new AddonComponent(0x1202), 0, 0, 0);
			AddComponent(new AddonComponent(0x1203), 0, 1, 0);
			AddComponent(new AddonComponent(0x1201), 0, 2, 0);
			Hue = hue;
		}

		public LargeStoneTableEastAddon(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class LargeStoneTableEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeStoneTableEastAddon(this.Hue); } }
		public override int LabelNumber { get { return 1044511; } } // large stone table (east)

		[Constructable]
		public LargeStoneTableEastDeed()
		{
		}

		public LargeStoneTableEastDeed(Serial serial)
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