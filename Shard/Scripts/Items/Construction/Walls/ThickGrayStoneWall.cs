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
 * NAME    : Thick Gray Stone Wall      *
 * SCRIPT  : ThickGrayStoneWall.cs      *
 * VERSION : v1.00                      *
 * CREATOR : Mans Sjoberg (Allmight)    *
 * CREATED : 10-07.2002                 *
 * **************************************/

using System;

namespace Server.Items
{
	public enum ThickGrayStoneWallTypes
	{
		WestArch,
		NorthArch,
		SouthArchTop,
		EastArchTop,
		EastArch,
		SouthArch,
		Wall1,
		Wall2,
		Wall3,
		SouthWindow,
		Wall4,
		EastWindow,
		WestArch2,
		NorthArch2,
		SouthArchTop2,
		EastArchTop2,
		EastArch2,
		SouthArch2,
		SWArchEdge2,
		SouthWindow2,
		NEArchEdge2,
		EastWindow2
	}

	public class ThickGrayStoneWall : BaseWall
	{
		[Constructable]
		public ThickGrayStoneWall(ThickGrayStoneWallTypes type)
			: base(0x007A + (int)type)
		{
		}

		public ThickGrayStoneWall(Serial serial)
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