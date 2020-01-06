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

/* Scripts/Items/Addons/ShieldAddons.cs
 * CHANGELOG:
 *	15/12/07, plasma
 *		Initial creation
 */

using System;
using Server;
using System.Collections;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{

	#region silver sepent shield

	#region deeds

	public class SilverSerpentShieldSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SilverSerpentShieldSouthAddon();
			}
		}

		[Constructable]
		public SilverSerpentShieldSouthAddonDeed()
		{
			Name = "a silver serpent shield (south)";
		}

		public SilverSerpentShieldSouthAddonDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class SilverSerpentShieldEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new SilverSerpentShieldEastAddon();
			}
		}

		[Constructable]
		public SilverSerpentShieldEastAddonDeed()
		{
			Name = "a silver serpent shield (east)";
		}

		public SilverSerpentShieldEastAddonDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	#endregion

	#region addons

	public class SilverSerpentShieldSouthAddon : BaseAddon
	{
		[Constructable]
		public SilverSerpentShieldSouthAddon()
		{
			AddComponent(new SilverSerpentShieldSouth(), 0, 0, 0);
		}

		public SilverSerpentShieldSouthAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed
		{
			get
			{
				return new SilverSerpentShieldSouthAddonDeed();
			}
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

	public class SilverSerpentShieldEastAddon : BaseAddon
	{
		[Constructable]
		public SilverSerpentShieldEastAddon()
		{
			AddComponent(new SilverSerpentShieldEast(), 0, 1, 0);
		}

		public SilverSerpentShieldEastAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed
		{
			get
			{
				return new SilverSerpentShieldEastAddonDeed();
			}
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

	#endregion

	#region addon components

	public class SilverSerpentShieldEast : AddonComponent
	{
		[Constructable]
		public SilverSerpentShieldEast()
			: this(0x1577)
		{
		}

		public SilverSerpentShieldEast(int itemID)
			: base(itemID)
		{
		}

		public SilverSerpentShieldEast(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class SilverSerpentShieldSouth : AddonComponent
	{
		[Constructable]
		public SilverSerpentShieldSouth()
			: this(0x1576)
		{
		}

		public SilverSerpentShieldSouth(int itemID)
			: base(itemID)
		{
		}

		public SilverSerpentShieldSouth(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	#endregion

	#endregion

	#region Golden serpent shield

	#region deeds

	public class GoldenSerpentShieldSouthAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new GoldenSerpentShieldSouthAddon();
			}
		}

		[Constructable]
		public GoldenSerpentShieldSouthAddonDeed()
		{
			Name = "a golden serpent shield (south)";
		}

		public GoldenSerpentShieldSouthAddonDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	public class GoldenSerpentShieldEastAddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new GoldenSerpentShieldEastAddon();
			}
		}

		[Constructable]
		public GoldenSerpentShieldEastAddonDeed()
		{
			Name = "a golden serpent shield (east)";
		}

		public GoldenSerpentShieldEastAddonDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // Version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}

	#endregion

	#region addons

	public class GoldenSerpentShieldSouthAddon : BaseAddon
	{
		[Constructable]
		public GoldenSerpentShieldSouthAddon()
		{
			AddComponent(new GoldenSerpentShieldSouth(), 0, 0, 0);
		}

		public GoldenSerpentShieldSouthAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed
		{
			get
			{
				return new GoldenSerpentShieldSouthAddonDeed();
			}
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

	public class GoldenSerpentShieldEastAddon : BaseAddon
	{
		[Constructable]
		public GoldenSerpentShieldEastAddon()
		{
			AddComponent(new GoldenSerpentShieldEast(), 0, 1, 0);
		}

		public GoldenSerpentShieldEastAddon(Serial serial)
			: base(serial)
		{
		}

		public override BaseAddonDeed Deed
		{
			get
			{
				return new GoldenSerpentShieldEastAddonDeed();
			}
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

	#endregion

	#region addon components

	public class GoldenSerpentShieldEast : AddonComponent
	{
		[Constructable]
		public GoldenSerpentShieldEast()
			: this(0x1579)
		{
		}

		public GoldenSerpentShieldEast(int itemID)
			: base(itemID)
		{
		}

		public GoldenSerpentShieldEast(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	public class GoldenSerpentShieldSouth : AddonComponent
	{
		[Constructable]
		public GoldenSerpentShieldSouth()
			: this(0x1578)
		{
		}

		public GoldenSerpentShieldSouth(int itemID)
			: base(itemID)
		{
		}

		public GoldenSerpentShieldSouth(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0);

		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}

	#endregion

	#endregion

}