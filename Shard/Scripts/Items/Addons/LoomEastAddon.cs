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
	public interface ILoom
	{
		int Phase { get; set; }
	}

	public class LoomEastAddon : BaseAddon, ILoom
	{
		public override BaseAddonDeed Deed { get { return new LoomEastDeed(); } }

		private int m_Phase;

		public int Phase { get { return m_Phase; } set { m_Phase = value; } }

		[Constructable]
		public LoomEastAddon()
		{
			AddComponent(new AddonComponent(0x1060), 0, 0, 0);
			AddComponent(new AddonComponent(0x105F), 0, 1, 0);
		}

		public LoomEastAddon(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			writer.Write((int)m_Phase);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_Phase = reader.ReadInt();
						break;
					}
			}
		}
	}

	public class LoomEastDeed : BaseAddonDeed
	{
		public override BaseAddon Addon { get { return new LoomEastAddon(); } }
		public override int LabelNumber { get { return 1044343; } } // loom (east)

		[Constructable]
		public LoomEastDeed()
		{
		}

		public LoomEastDeed(Serial serial)
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