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

/****************************************
 * NAME    : White Stone Wall           *
 * SCRIPT  : WhiteStoneWall.cs          *
 * VERSION : v1.00                      *
 * CREATOR : Mans Sjoberg (Allmight)    *
 * CREATED : 10-07.2002                 *
 * **************************************/

using System;

namespace Server.Items
{
	public enum WhiteStoneWallTypes
	{
		EastWall,
		SouthWall,
		SECorner,
		NWCornerPost,
		EastArrowLoop,
		SouthArrowLoop,
		EastWindow,
		SouthWindow,
		SouthWallMedium,
		EastWallMedium,
		SECornerMedium,
		NWCornerPostMedium,
		SouthWallShort,
		EastWallShort,
		SECornerShort,
		NWCornerPostShort,
		NECornerPostShort,
		SWCornerPostShort,
		SouthWallVShort,
		EastWallVShort,
		SECornerVShort,
		NWCornerPostVShort,
		SECornerArch,
		SouthArch,
		WestArch,
		EastArch,
		NorthArch,
		EastBattlement,
		SECornerBattlement,
		SouthBattlement,
		NECornerBattlement,
		SWCornerBattlement,
		Column,
		SouthWallVVShort,
		EastWallVVShort
	}

	public class WhiteStoneWall : BaseWall
	{
		[Constructable]
		public WhiteStoneWall(WhiteStoneWallTypes type)
			: base(0x0057 + (int)type)
		{
		}

		public WhiteStoneWall(Serial serial)
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