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

/* Scripts/Items/FlowersPlants/CottonPlant.cs
 *
 *	ChangeLog:
 * 2/11/07, Pix
 *		Finally added range check to picking cotton.
 *	5/26/04 Created by smerX
 *
 */

using System;

namespace Server.Items
{
	public class CottonPlant : Item
	{
		[Constructable]
		public CottonPlant()
			: base(Utility.RandomList(0xc51, 0xc52, 0xc53, 0xc54))
		{
			Weight = 0;
			Name = "a cotton plant";
			Movable = false;
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!from.InRange(GetWorldLocation(), 1))
			{
				SendLocalizedMessageTo(from, 501816); // You are too far away to do that.
			}
			else
			{
				Cotton cotton = new Cotton();
				cotton.MoveToWorld(new Point3D(this.X, this.Y, this.Z), this.Map);

				this.Delete();
			}
		}

		public CottonPlant(Serial serial)
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