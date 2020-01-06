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
	public class WorldMap : MapItem
	{
		[Constructable]
		public WorldMap()
		{
			SetDisplay(0, 0, 5119, 4095, 400, 400);
		}

		public override void CraftInit(Mobile from)
		{
			// Unlike the others, world map is not based on crafted location

			double skillValue = from.Skills[SkillName.Cartography].Value;
			int x20 = (int)(skillValue * 20);
			int size = 25 + (int)(skillValue * 6.6);

			if (size < 200)
				size = 200;
			else if (size > 400)
				size = 400;

			SetDisplay(1344 - x20, 1600 - x20, 1472 + x20, 1728 + x20, size, size);
		}

		public override int LabelNumber { get { return 1015233; } } // world map

		public WorldMap(Serial serial)
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
}