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

namespace Server.Engines.Mahjong
{
	public enum MahjongPieceDirection
	{
		Up,
		Left,
		Down,
		Right
	}

	public enum MahjongWind
	{
		North,
		East,
		South,
		West
	}

	public enum MahjongTileType
	{
		Dagger1 = 1,
		Dagger2,
		Dagger3,
		Dagger4,
		Dagger5,
		Dagger6,
		Dagger7,
		Dagger8,
		Dagger9,
		Gem1,
		Gem2,
		Gem3,
		Gem4,
		Gem5,
		Gem6,
		Gem7,
		Gem8,
		Gem9,
		Number1,
		Number2,
		Number3,
		Number4,
		Number5,
		Number6,
		Number7,
		Number8,
		Number9,
		North,
		East,
		South,
		West,
		Green,
		Red,
		White
	}
}