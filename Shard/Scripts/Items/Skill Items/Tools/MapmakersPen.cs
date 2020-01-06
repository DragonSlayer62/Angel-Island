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
	[FlipableAttribute(0x0FBF, 0x0FC0)]
	public class MapmakersPen : BaseTool
	{
		public override CraftSystem CraftSystem { get { return DefCartography.CraftSystem; } }

		public override int LabelNumber { get { return 1044167; } } // mapmaker's pen

		[Constructable]
		public MapmakersPen()
			: base(0x0FBF)
		{
			Weight = 1.0;
		}

		[Constructable]
		public MapmakersPen(int uses)
			: base(uses, 0x0FBF)
		{
			Weight = 1.0;
		}

		public MapmakersPen(Serial serial)
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
				Weight = 1.0;
		}
	}
}