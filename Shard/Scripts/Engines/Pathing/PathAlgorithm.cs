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

namespace Server.PathAlgorithms
{
	public abstract class PathAlgorithm
	{
		public abstract bool CheckCondition(Mobile m, Map map, Point3D start, Point3D goal);
		public abstract Direction[] Find(Mobile m, Map map, Point3D start, Point3D goal);

		private static Direction[] m_CalcDirections = new Direction[9]
			{
				Direction.Up,
				Direction.North,
				Direction.Right,
				Direction.West,
				Direction.North,
				Direction.East,
				Direction.Left,
				Direction.South,
				Direction.Down
			};

		public Direction GetDirection(int xSource, int ySource, int xDest, int yDest)
		{
			int x = xDest + 1 - xSource;
			int y = yDest + 1 - ySource;
			int v = (y * 3) + x;

			if (v < 0 || v >= 9)
				return Direction.North;

			return m_CalcDirections[v];
		}
	}
}