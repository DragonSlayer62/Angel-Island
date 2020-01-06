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

/* Scripts/Engines/Plants/PlantTypes.cs
 *  04/06/05, Kitaras
 *	 Added PlantType Hedge for new treasure loot
 */
using System;
using Server;

namespace Server.Engines.Plants
{
	public enum PlantType
	{
		CampionFlowers,
		Poppies,
		Snowdrops,
		Bulrushes,
		Lilies,
		PampasGrass,
		Rushes,
		ElephantEarPlant,
		Fern,
		PonytailPalm,
		SmallPalm,
		CenturyPlant,
		WaterPlant,
		SnakePlant,
		PricklyPearCactus,
		BarrelCactus,
		TribarrelCactus,
		Hedge
	}

	public class PlantTypeInfo
	{
		private static PlantTypeInfo[] m_Table = new PlantTypeInfo[]
			{
				new PlantTypeInfo( 0xC83, 0, 0,		PlantType.CampionFlowers,		false, true ),
				new PlantTypeInfo( 0xC86, 0, 0,		PlantType.Poppies,				false, true ),
				new PlantTypeInfo( 0xC88, 0, 10,	PlantType.Snowdrops,			false, true ),
				new PlantTypeInfo( 0xC94, -15, 0,	PlantType.Bulrushes,			false, true ),
				new PlantTypeInfo( 0xC8B, 0, 0,		PlantType.Lilies,				false, true ),
				new PlantTypeInfo( 0xCA5, -8, 0,	PlantType.PampasGrass,			false, true ),
				new PlantTypeInfo( 0xCA7, -10, 0,	PlantType.Rushes,				false, true ),
				new PlantTypeInfo( 0xC97, -20, 0,	PlantType.ElephantEarPlant,		true, false ),
				new PlantTypeInfo( 0xC9F, -20, 0,	PlantType.Fern,					false, false ),
				new PlantTypeInfo( 0xCA6, -16, -5,	PlantType.PonytailPalm,			false, false ),
				new PlantTypeInfo( 0xC9C, -5, -10,	PlantType.SmallPalm,			false, false ),
				new PlantTypeInfo( 0xD31, 0, -27,	PlantType.CenturyPlant,			true, false ),
				new PlantTypeInfo( 0xD04, 0, 10,	PlantType.WaterPlant,			true, false ),
				new PlantTypeInfo( 0xCA9, 0, 0,		PlantType.SnakePlant,			true, false ),
				new PlantTypeInfo( 0xD2C, 0, 10,	PlantType.PricklyPearCactus,	false, false ),
				new PlantTypeInfo( 0xD26, 0, 10,	PlantType.BarrelCactus,			false, false ),
				new PlantTypeInfo( 0xD27, 0, 10,	PlantType.TribarrelCactus,		false, false ),
				new PlantTypeInfo( 3215, 0, 0,		PlantType.Hedge,			false, false )
			};

		public static PlantTypeInfo GetInfo(PlantType plantType)
		{
			int index = (int)plantType;

			if (index >= 0 && index < m_Table.Length)
				return m_Table[index];
			else
				return m_Table[0];
		}

		public static PlantType RandomFirstGeneration()
		{
			switch (Utility.Random(3))
			{
				case 0: return PlantType.CampionFlowers;
				case 1: return PlantType.Fern;
				default: return PlantType.TribarrelCactus;
			}
		}

		public static PlantType Cross(PlantType first, PlantType second)
		{
			int firstIndex = (int)first;
			int secondIndex = (int)second;

			if (firstIndex + 1 == secondIndex || firstIndex == secondIndex + 1)
				return Utility.RandomBool() ? first : second;
			else
				return (PlantType)((firstIndex + secondIndex) / 2);
		}

		private int m_ItemID;
		private int m_OffsetX;
		private int m_OffsetY;
		private PlantType m_PlantType;
		private bool m_ContainsPlant;
		private bool m_Flowery;

		public int ItemID { get { return m_ItemID; } }
		public int OffsetX { get { return m_OffsetX; } }
		public int OffsetY { get { return m_OffsetY; } }
		public PlantType PlantType { get { return m_PlantType; } }
		public int Name { get { return 1020000 + m_ItemID; } }
		public bool ContainsPlant { get { return m_ContainsPlant; } }
		public bool Flowery { get { return m_Flowery; } }

		private PlantTypeInfo(int itemID, int offsetX, int offsetY, PlantType plantType, bool containsPlant, bool flowery)
		{
			m_ItemID = itemID;
			m_OffsetX = offsetX;
			m_OffsetY = offsetY;
			m_PlantType = plantType;
			m_ContainsPlant = containsPlant;
			m_Flowery = flowery;
		}
	}
}