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
	public class CityMap : MapItem
	{
		[Constructable]
		public CityMap()
		{
			SetDisplay(0, 0, 5119, 4095, 400, 400);
		}

		public override void CraftInit(Mobile from)
		{
			double skillValue = from.Skills[SkillName.Cartography].Value;
			int dist = 64 + (int)(skillValue * 4);

			if (dist < 200)
				dist = 200;

			int size = 32 + (int)(skillValue * 2);

			if (size < 200)
				size = 200;
			else if (size > 400)
				size = 400;

			SetDisplay(from.X - dist, from.Y - dist, from.X + dist, from.Y + dist, size, size);
		}

		public override int LabelNumber { get { return 1015231; } } // city map

		public CityMap(Serial serial)
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