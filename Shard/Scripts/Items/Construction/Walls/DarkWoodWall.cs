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

namespace Server.Items
{
	public enum DarkWoodWallTypes
	{
		Corner,
		SouthWall,
		EastWall,
		CornerPost,
		EastDoorFrame,
		SouthDoorFrame,
		WestDoorFrame,
		NorthDoorFrame,
		SouthWindow,
		EastWindow,
		CornerMedium,
		EastWallMedium,
		SouthWallMedium,
		CornerPostMedium,
		CornerShort,
		EastWallShort,
		SouthWallShort,
		CornerPostShort,
		SouthWallVShort,
		EastWallVShort
	}

	public class DarkWoodWall : BaseWall
	{
		[Constructable]
		public DarkWoodWall(DarkWoodWallTypes type)
			: base(0x0006 + (int)type)
		{
		}

		public DarkWoodWall(Serial serial)
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