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

/* Scripts\Multis\Patches\patch_1293Addon.cs
 * ChangeLog
 *	8/12/07, Adam
 *		single tile patch for houses with marble floors. Static ID 1293
 */

using System;
using Server;
using Server.Items;

namespace Server.Multis
{
	public class patch_1293Addon : BaseAddon
	{
		public override BaseAddonDeed Deed
		{
			get
			{
				return new patch_1293AddonDeed();
			}
		}

		[Constructable]
		public patch_1293Addon()
		{
			AddonComponent ac = null;
			ac = new AddonComponent(1293);
			AddComponent(ac, 0, 0, 0);

		}

		public patch_1293Addon(Serial serial)
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

	public class patch_1293AddonDeed : BaseAddonDeed
	{
		public override BaseAddon Addon
		{
			get
			{
				return new patch_1293Addon();
			}
		}

		[Constructable]
		public patch_1293AddonDeed()
		{
			Name = "patch_1293";
		}

		public patch_1293AddonDeed(Serial serial)
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
}