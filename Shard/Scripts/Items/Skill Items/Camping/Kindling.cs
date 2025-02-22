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

/* Items/Skill Items/Camping/Kindling.cs
 * CHANGELOG:
 *	2/22/11, Adam
 *		o Update to RunUO 2.0
 *		o Replace DungeonRegion check with IsDungeonRules
 *  11/21/06, Plasma
 *      Prevent secure message if in a no camp zone
 *	5/10/06, Adam
 *		remove assumption players region is non-null
 *	5/03/06, weaver
 *		Made camping work 100% of the time in tents.
 * 	5/12/04, Pixie
 *		Reversed the previous change - can camp in dungeons now.
 *	5/10/04, Pixie
 *		Made it so you can't camp in a dungeon
 *	5/10/04, Pixie
 *		Initial working revision
 */

using System;
using System.Collections;
using Server;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
	public class Kindling : Item
	{
		[Constructable]
		public Kindling()
			: this(1)
		{
		}

		[Constructable]
		public Kindling(int amount)
			: base(0xDE1)
		{
			Stackable = true;
			Weight = 5.0;
			Amount = amount;
		}

		public Kindling(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			if (!this.VerifyMove(from))
				return;

			if (!from.InRange(this.GetWorldLocation(), 2))
			{
				from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
				return;
			}

			Point3D fireLocation = GetFireLocation(from);

			if (fireLocation == Point3D.Zero)
			{
				from.SendLocalizedMessage(501695); // There is not a spot nearby to place your campfire.
			}
			else if (!from.CheckSkill(SkillName.Camping, 0.0, 100.0))
			{
				from.SendLocalizedMessage(501696); // You fail to ignite the campfire.
			}
			else
			{
				Consume();

				if (!this.Deleted && this.Parent == null)
					from.PlaceInBackpack(this);

				new Campfire().MoveToWorld(fireLocation, from.Map);
			}
		}

		private Point3D GetFireLocation(Mobile from)
		{
			//if (from.Region.IsPartOf(typeof(DungeonRegion)))
			//return Point3D.Zero;

			// AI style
			if (from.Region.IsDungeonRules)
				return Point3D.Zero;

			if (this.Parent == null)
				return this.Location;

			ArrayList list = new ArrayList(4);

			AddOffsetLocation(from, 0, -1, list);
			AddOffsetLocation(from, -1, 0, list);
			AddOffsetLocation(from, 0, 1, list);
			AddOffsetLocation(from, 1, 0, list);

			if (list.Count == 0)
				return Point3D.Zero;

			int idx = Utility.Random(list.Count);
			return (Point3D)list[idx];
		}

		private void AddOffsetLocation(Mobile from, int offsetX, int offsetY, ArrayList list)
		{
			Map map = from.Map;

			int x = from.X + offsetX;
			int y = from.Y + offsetY;

			Point3D loc = new Point3D(x, y, from.Z);

			if (map.CanFit(loc, 1) && from.InLOS(loc))
			{
				list.Add(loc);
			}
			else
			{
				loc = new Point3D(x, y, map.GetAverageZ(x, y));

				if (map.CanFit(loc, 1) && from.InLOS(loc))
					list.Add(loc);
			}
		}
	}
}