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

namespace Server.Engines.Mahjong
{
	public class MahjongWallBreakIndicator
	{
		public static MahjongPieceDim GetDimensions(Point2D position)
		{
			return new MahjongPieceDim(position, 20, 20);
		}

		private MahjongGame m_Game;
		private Point2D m_Position;

		public MahjongGame Game { get { return m_Game; } }
		public Point2D Position { get { return m_Position; } }

		public MahjongWallBreakIndicator(MahjongGame game, Point2D position)
		{
			m_Game = game;
			m_Position = position;
		}

		public MahjongPieceDim Dimensions
		{
			get { return GetDimensions(m_Position); }
		}

		public void Move(Point2D position)
		{
			MahjongPieceDim dim = GetDimensions(position);

			if (!dim.IsValid())
				return;

			m_Position = position;

			m_Game.Players.SendGeneralPacket(true, true);
		}

		public void Save(GenericWriter writer)
		{
			writer.Write((int)0); // version

			writer.Write(m_Position);
		}

		public MahjongWallBreakIndicator(MahjongGame game, GenericReader reader)
		{
			m_Game = game;

			int version = reader.ReadInt();

			m_Position = reader.ReadPoint2D();
		}
	}
}