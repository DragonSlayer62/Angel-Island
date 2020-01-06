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
	[Flipable(0xE1C, 0xFAD)]
	public class Backgammon : BaseBoard
	{
		public override int DefaultGumpID { get { return 0x92E; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(0, 0, 282, 230); }
		}

		[Constructable]
		public Backgammon()
			: base(0xE1C)
		{
		}

		public override void CreatePieces()
		{
			for (int i = 0; i < 5; i++)
			{
				CreatePiece(new PieceWhiteChecker(this), 42, (17 * i) + 6);
				CreatePiece(new PieceBlackChecker(this), 42, (17 * i) + 119);

				CreatePiece(new PieceBlackChecker(this), 142, (17 * i) + 6);
				CreatePiece(new PieceWhiteChecker(this), 142, (17 * i) + 119);
			}

			for (int i = 0; i < 3; i++)
			{
				CreatePiece(new PieceBlackChecker(this), 108, (17 * i) + 6);
				CreatePiece(new PieceWhiteChecker(this), 108, (17 * i) + 153);
			}

			for (int i = 0; i < 2; i++)
			{
				CreatePiece(new PieceWhiteChecker(this), 223, (17 * i) + 6);
				CreatePiece(new PieceBlackChecker(this), 223, (17 * i) + 170);
			}
		}

		public Backgammon(Serial serial)
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
