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
using Server.Engines.Craft;

namespace Server.Items
{
	[FlipableAttribute(0xE8A, 0xE89)]
	public class Blowpipe : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefGlassblowing.CraftSystem; } }

		public override int LabelNumber { get { return 1044608; } } // blow pipe

		[Constructable]
		public Blowpipe()
			: base(0xE8A)
		{
			Weight = 4.0;
			Hue = 0x3B9;
		}

		[Constructable]
		public Blowpipe(int uses)
			: base(uses, 0xE8A)
		{
			Weight = 4.0;
			Hue = 0x3B9;
		}

		public Blowpipe(Serial serial)
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

			if (Weight == 2.0)
				Weight = 4.0;
		}
	}
}