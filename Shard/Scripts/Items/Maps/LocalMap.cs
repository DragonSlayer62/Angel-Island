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
	public class LocalMap : MapItem
	{
		[Constructable]
		public LocalMap()
		{
			SetDisplay(0, 0, 5119, 4095, 400, 400);
		}

		public override void CraftInit(Mobile from)
		{
			double skillValue = from.Skills[SkillName.Cartography].Value;
			int dist = 64 + (int)(skillValue * 2);

			SetDisplay(from.X - dist, from.Y - dist, from.X + dist, from.Y + dist, 200, 200);
		}

		public override int LabelNumber { get { return 1015230; } } // local map

		public LocalMap(Serial serial)
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