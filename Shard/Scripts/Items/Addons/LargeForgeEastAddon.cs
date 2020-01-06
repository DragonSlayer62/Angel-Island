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
/* Scripts/Items/Addons/LargeEastForgeAddon.cs
* ChangeLog
*  7/30/06, Kit
*		Add Forge Bellows, Rise of the animated forges!
*/

using System;
using Server;
using Server.Commands;

namespace Server.Items
{

	public class LargeForgeEastAddon : BaseAddon
	{
		public override BaseAddonDeed Deed { get { return new LargeForgeEastDeed(); } }

		[Constructable]
		public LargeForgeEastAddon()
		{
			AddComponent(new ForgeBellows(6534), 0, 0, 0);
			AddComponent(new ForgeComponent(0x198A), 0, 1, 0);
			AddComponent(new ForgeComponent(0x1996), 0, 2, 0);
			AddComponent(new ForgeBellows(6546), 0, 3, 0);
		}

		public LargeForgeEastAddon(Serial serial)
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
			//reset all graphics back to base animation, incase we saved and crashed, or went down
			//before bellow timer could reset graphics.
			try
			{
				((AddonComponent)this.Components[0]).ItemID = 6534;
				((AddonComponent)this.Components[1]).ItemID = 0x198A;
				((AddonComponent)this.Components[2]).ItemID = 0x1996;
				((AddonComponent)this.Components[3]).ItemID = 6546;
			}
			catch (Exception exc)
			{
				LogHelper.LogException(exc);
				Console.WriteLine("Exception caught in Large East forge addon Deserialization");
				System.Console.WriteLine(exc.Message);
				System.Console.WriteLine(exc.StackTrace);
			}
		}

	}

	public class LargeForgeEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LargeForgeEastAddon(); } }
		public override int LabelNumber { get { return 1044331; } } // large forge (east)

		[Constructable]
		public LargeForgeEastDeed()
		{
		}

		public LargeForgeEastDeed(Serial serial)
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