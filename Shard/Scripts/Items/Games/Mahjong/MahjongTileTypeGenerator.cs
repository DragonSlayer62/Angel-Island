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
using Server;

namespace Server.Engines.Mahjong
{
	public class MahjongTileTypeGenerator
	{
		private ArrayList m_LeftTileTypes;

		public ArrayList LeftTileTypes { get { return m_LeftTileTypes; } }

		public MahjongTileTypeGenerator(int count)
		{
			m_LeftTileTypes = new ArrayList(34 * count);

			for (int i = 1; i <= 34; i++)
			{
				for (int j = 0; j < count; j++)
				{
					m_LeftTileTypes.Add((MahjongTileType)i);
				}
			}
		}

		public MahjongTileType Next()
		{
			int random = Utility.Random(m_LeftTileTypes.Count);
			MahjongTileType next = (MahjongTileType)m_LeftTileTypes[random];
			m_LeftTileTypes.RemoveAt(random);

			return next;
		}
	}
}