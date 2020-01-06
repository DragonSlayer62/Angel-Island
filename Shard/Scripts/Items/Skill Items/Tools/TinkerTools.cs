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

/*
 * CHGANGELOG
 * 7/8/11, Adam
 *		Add old school tinker tools
 */

using System;
using Server;
using Server.Engines.Craft;

namespace Server.Items
{
	[Flipable(0x1EB8, 0x1EB9)]
	public class TinkerTools : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefTinkering.CraftSystem; } }

		[Constructable]
		public TinkerTools()
			: base(0x1EB8)
		{
			Weight = 1.0;
		}

		[Constructable]
		public TinkerTools(int uses)
			: base(uses, 0x1EB8)
		{
			Weight = 1.0;
		}

		public TinkerTools(Serial serial)
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

	// Tinker Tools Old School
	public class TinkerToolsOS : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefTinkering.CraftSystem; } }

		[Constructable]
		public TinkerToolsOS()
			: base(0x1EBC)
		{
			Weight = 1.0;
		}

		[Constructable]
		public TinkerToolsOS(int uses)
			: base(uses, 0x1EBC)
		{
			Weight = 1.0;
		}

		public TinkerToolsOS(Serial serial)
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