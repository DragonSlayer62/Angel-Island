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
	public class MahjongDices
	{
		private MahjongGame m_Game;
		private int m_First;
		private int m_Second;

		public MahjongGame Game { get { return m_Game; } }
		public int First { get { return m_First; } }
		public int Second { get { return m_Second; } }

		public MahjongDices(MahjongGame game)
		{
			m_Game = game;
			m_First = Utility.Random(1, 6);
			m_Second = Utility.Random(1, 6);
		}

		public void RollDices(Mobile from)
		{
			m_First = Utility.Random(1, 6);
			m_Second = Utility.Random(1, 6);

			m_Game.Players.SendGeneralPacket(true, true);

			if (from != null)
				m_Game.Players.SendLocalizedMessage(1062695, string.Format("{0}\t{1}\t{2}", from.Name, m_First, m_Second)); // ~1_name~ rolls the dice and gets a ~2_number~ and a ~3_number~!
		}

		public void Save(GenericWriter writer)
		{
			writer.Write((int)0); // version

			writer.Write(m_First);
			writer.Write(m_Second);
		}

		public MahjongDices(MahjongGame game, GenericReader reader)
		{
			m_Game = game;

			int version = reader.ReadInt();

			m_First = reader.ReadInt();
			m_Second = reader.ReadInt();
		}
	}
}