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
using System.Collections;

namespace Server.Items
{
	public class CheckerBoard : BaseBoard
	{
		public override int LabelNumber { get { return 1016449; } } // a checker board

		public override int DefaultGumpID { get { return 0x91A; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(0, 0, 282, 210); }
		}

		[Constructable]
		public CheckerBoard()
			: base(0xFA6)
		{
		}

		public override void CreatePieces()
		{
			for (int i = 0; i < 4; i++)
			{
				CreatePiece(new PieceWhiteChecker(this), (50 * i) + 45, 25);
				CreatePiece(new PieceWhiteChecker(this), (50 * i) + 70, 50);
				CreatePiece(new PieceWhiteChecker(this), (50 * i) + 45, 75);
				CreatePiece(new PieceBlackChecker(this), (50 * i) + 70, 150);
				CreatePiece(new PieceBlackChecker(this), (50 * i) + 45, 175);
				CreatePiece(new PieceBlackChecker(this), (50 * i) + 70, 200);
			}
		}

		public CheckerBoard(Serial serial)
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