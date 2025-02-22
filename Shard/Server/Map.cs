/***************************************************************************
 *                                  Map.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id: Map.cs,v 1.25 2011/02/24 18:32:23 luket Exp $
 *   $Author: luket $
 *   $Date: 2011/02/24 18:32:23 $
 *
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

/* Server/Map.cs
 * CHANGELOG:
 *	10/21/08, Adam
 *		Remove the following debug line:
 *		string message = String.Format("PooledEnumerable.Instantiate() called after the derived class was: {0}", e.Status.ToString());
 *	10/9/08, Adam
 *		I don't believe we are suffereing a memory leak from the PooledEnumerable Depth warning. My tests show that the memory is freed
 *			when the Dispose is called. We therefore replaced the Exception with a Console notification of the form:
 *			PooledEnumerable: Depth = {0}, High Water Mark = {1}
 *  7/21/08, Adam
 *      Looks like our exception below is fireing a lot, only call the exception once. Too many times will lag stuff out. 
 *      Note: We need to understand this better.
 *	7/8/08, Adam
 *		Replace Console.WriteLine("Warning: Make sure to call .Free() on pooled enumerables.");
 *		with try { throw (new ApplicationException("Warning: Make sure to call .Free() on pooled enumerables.")); }
 * 		catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
 *		This ensures we will see the error. I suspect this could be happening unbeknownst to development.
 *	7/7/08, Adam
 *		Replace exception logic for int.Parse(value, out index) with normal error handling (int.TryParse()).
 *  3/19/07, Adam
 *      Recfactor CanFit to use CanFitFlags.
 *      CanFit() was up to 10 overloads and some had different parameters in different places.
 *	02/01/07, Pix
 *		Made CanFit() take another parameter to tell us whether to ignore ghosts when considering mobiles.
 *	4/28/06, weaver
 *		Added overloaded LOS to take audible check flag (indication to ignore
 *		TileFlag.NoShoot - this is to differentiate LOF from LOS checks)
 *	9/12/05, Pix
 *		LOS special case for right flap of orc fort
 */

using System;
using System.Collections;
using System.Collections.Generic;
using Server.Network;
using System.Runtime.Serialization;
using Server.Targeting;
using Server.Items;

namespace Server
{
	[Flags]
	public enum MapRules
	{
		None = 0x0000,
		Internal = 0x0001, // Internal map (used for dragging, commodity deeds, etc)
		FreeMovement = 0x0002, // Anyone can move over anyone else without taking stamina loss
		BeneficialRestrictions = 0x0004, // Disallow performing beneficial actions on criminals/murderers
		HarmfulRestrictions = 0x0008, // Disallow performing harmful actions on innocents
		TrammelRules = FreeMovement | BeneficialRestrictions | HarmfulRestrictions,
		FeluccaRules = None
	}

	[Flags]
	public enum CanFitFlags
	{
		none = 0x0000,
		checkBlocksFit = 0x0001,
		checkMobiles = 0x0002,
		requireSurface = 0x0004,
		ignoreDeadMobiles = 0x0008,
		canSwim = 0x0010,
		cantWalk = 0x0020,
	}

	public interface IPooledEnumerable : IEnumerable
	{
		void Free();
	}

	public interface IPooledEnumerator : IEnumerator
	{
		IPooledEnumerable Enumerable { get; set; }
		void Free();
	}

	[Parsable]
	//[CustomEnum( new string[]{ "Felucca", "Trammel", "Ilshenar", "Malas", "Internal" } )]
	public sealed class Map
	{
		public const int SectorSize = 16;
		public const int SectorShift = 4;
		public static int SectorActiveRange = 2;

		//Pix: this is THE orc flap special case.
		private static Point3D ORCFLAPRIGHT = new Point3D(0x27A, 0x5D5, 0xE);

		private static Map[] m_Maps = new Map[0x100];

		public static Map[] Maps { get { return m_Maps; } }

		public static Map Felucca { get { return m_Maps[0]; } }
		public static Map Trammel { get { return m_Maps[1]; } }
		public static Map Ilshenar { get { return m_Maps[2]; } }
		public static Map Malas { get { return m_Maps[3]; } }
		public static Map Tokuno { get { return m_Maps[4]; } }
		public static Map Internal { get { return m_Maps[0x7F]; } }

		private static ArrayList m_AllMaps = new ArrayList();

		public static ArrayList AllMaps { get { return m_AllMaps; } }

		/*public static readonly Map Felucca	= new Map( 0, 0, 6144, 4096, 4, "Felucca",	MapRules.FeluccaRules );
		public static readonly Map Trammel	= new Map( 1, 0, 6144, 4096, 0, "Trammel",	MapRules.TrammelRules );
		public static readonly Map Ilshenar = new Map( 2, 2, 2304, 1600, 1, "Ilshenar",	MapRules.TrammelRules );
		public static readonly Map Malas	= new Map( 3, 3, 2560, 2048, 1, "Malas",	MapRules.TrammelRules );

		public static readonly Map Internal = new Map( 0x7F, 0x7F, SectorSize, SectorSize, 1, "Internal", MapRules.Internal );*/

		private int m_MapID, m_MapIndex, m_FileIndex;

		private int m_Width, m_Height;
		private int m_SectorsWidth, m_SectorsHeight;
		private int m_Season;
		private ArrayList m_Regions;
		private Region m_DefaultRegion;

		public int Season { get { return m_Season; } set { m_Season = value; } }

		private string m_Name;
		private MapRules m_Rules;
		private Sector[][] m_Sectors;
		private Sector m_InvalidSector;

		private TileMatrix m_Tiles;

		private static string[] m_MapNames;
		private static Map[] m_MapValues;

		public static string[] GetMapNames()
		{
			CheckNamesAndValues();
			return m_MapNames;
		}

		public static Map[] GetMapValues()
		{
			CheckNamesAndValues();
			return m_MapValues;
		}

		public static Map Parse(string value)
		{
			CheckNamesAndValues();

			// try as a string
			for (int i = 0; i < m_MapNames.Length; ++i)
			{
				if (Insensitive.Equals(m_MapNames[i], value))
					return m_MapValues[i];
			}

			// try as a number now
			int index;
			if (int.TryParse(value, out index) == true)
			{
				if (index >= 0 && index < m_Maps.Length && m_Maps[index] != null)
					return m_Maps[index];
			}

			return null;
		}

		private static void CheckNamesAndValues()
		{
			if (m_MapNames != null && m_MapNames.Length == m_AllMaps.Count)
				return;

			m_MapNames = new string[m_AllMaps.Count];
			m_MapValues = new Map[m_AllMaps.Count];

			for (int i = 0; i < m_AllMaps.Count; ++i)
			{
				Map map = (Map)m_AllMaps[i];

				m_MapNames[i] = map.Name;
				m_MapValues[i] = map;
			}
		}

		public override string ToString()
		{
			return m_Name;
		}

		public int GetAverageZ(int x, int y)
		{
			int z = 0, avg = 0, top = 0;

			GetAverageZ(x, y, ref z, ref avg, ref top);

			return avg;
		}

		public void GetAverageZ(int x, int y, ref int z, ref int avg, ref int top)
		{
			int zTop = Tiles.GetLandTile(x, y).Z;
			int zLeft = Tiles.GetLandTile(x, y + 1).Z;
			int zRight = Tiles.GetLandTile(x + 1, y).Z;
			int zBottom = Tiles.GetLandTile(x + 1, y + 1).Z;

			z = zTop;
			if (zLeft < z)
				z = zLeft;
			if (zRight < z)
				z = zRight;
			if (zBottom < z)
				z = zBottom;

			top = zTop;
			if (zLeft > top)
				top = zLeft;
			if (zRight > top)
				top = zRight;
			if (zBottom > top)
				top = zBottom;

			if (Math.Abs(zTop - zBottom) > Math.Abs(zLeft - zRight))
				avg = (int)Math.Floor((zLeft + zRight) / 2.0);
			else
				avg = (int)Math.Floor((zTop + zBottom) / 2.0);

			//avg = (int)Math.Floor( (zTop + zLeft + zRight + zBottom) / 4.0 );

			//avg = z + ((top - z) / 2);
			//avg = (zTop+zLeft+zRight+zBottom+zTop+zBottom)/6;
			//avg=(zTop+zBottom)/2;
			//avg = (z + top) / 2;
		}

		public IPooledEnumerable GetObjectsInRange(Point3D p)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(ObjectEnumerator.Instantiate(this, new Rectangle2D(p.m_X - 18, p.m_Y - 18, 37, 37)));
		}

		public IPooledEnumerable GetObjectsInRange(Point3D p, int range)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(ObjectEnumerator.Instantiate(this, new Rectangle2D(p.m_X - range, p.m_Y - range, range * 2 + 1, range * 2 + 1)));
		}

		public IPooledEnumerable GetObjectsInBounds(Rectangle2D bounds)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(ObjectEnumerator.Instantiate(this, bounds));
		}

		public IPooledEnumerable GetClientsInRange(Point3D p)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - 18, p.m_Y - 18, 37, 37), SectorEnumeratorType.Clients));
		}

		public IPooledEnumerable GetClientsInRange(Point3D p, int range)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - range, p.m_Y - range, range * 2 + 1, range * 2 + 1), SectorEnumeratorType.Clients));
		}

		public IPooledEnumerable GetClientsInBounds(Rectangle2D bounds)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, bounds, SectorEnumeratorType.Clients));
		}

		public IPooledEnumerable GetItemsInRange(Point3D p)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - 18, p.m_Y - 18, 37, 37), SectorEnumeratorType.Items));
		}

		public IPooledEnumerable GetItemsInRange(Point3D p, int range)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - range, p.m_Y - range, range * 2 + 1, range * 2 + 1), SectorEnumeratorType.Items));
		}

		public IPooledEnumerable GetItemsInBounds(Rectangle2D bounds)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, bounds, SectorEnumeratorType.Items));
		}

		public IPooledEnumerable GetMobilesInRange(Point3D p)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - 18, p.m_Y - 18, 37, 37), SectorEnumeratorType.Mobiles));
		}

		public IPooledEnumerable GetMobilesInRange(Point3D p, int range)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, new Rectangle2D(p.m_X - range, p.m_Y - range, range * 2 + 1, range * 2 + 1), SectorEnumeratorType.Mobiles));
		}

		public IPooledEnumerable GetMobilesInBounds(Rectangle2D bounds)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(TypedEnumerator.Instantiate(this, bounds, SectorEnumeratorType.Mobiles));
		}

		public IPooledEnumerable GetMultiTilesAt(int x, int y)
		{
			if (this == Map.Internal)
				return NullEnumerable.Instance;

			Sector sector = GetSector(x, y);

			if (sector.Multis.Count == 0)
				return NullEnumerable.Instance;

			return PooledEnumerable.Instantiate(MultiTileEnumerator.Instantiate(sector, new Point2D(x, y)));
		}

		public bool CanFit(Point2D p, int z, int height)
		{
			return CanFit(p.m_X, p.m_Y, z, height, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		public bool CanFit(Point3D p, int height)
		{
			return CanFit(p.m_X, p.m_Y, p.m_Z, height, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		public bool CanFit(Point2D p, int z, int height, CanFitFlags flags)
		{
			return CanFit(p.m_X, p.m_Y, z, height, flags);
		}

		public bool CanFit(Point3D p, int height, CanFitFlags flags)
		{
			return CanFit(p.m_X, p.m_Y, p.m_Z, height, flags);
		}

		public bool CanFit(int x, int y, int z, int height)
		{
			return CanFit(x, y, z, height, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		//Pix: added ignoreDeadMobiles -- default is false (meaning we take them into account)
		// ignoreDeadMobiles==true means that we'll treat dead mobiles like they're not there.
		public bool CanFit(int x, int y, int z, int height, CanFitFlags flags)
		{
			bool checkBlocksFit = (flags & CanFitFlags.checkBlocksFit) != 0;
			bool checkMobiles = (flags & CanFitFlags.checkMobiles) != 0;
			bool requireSurface = (flags & CanFitFlags.requireSurface) != 0;
			bool ignoreDeadMobiles = (flags & CanFitFlags.ignoreDeadMobiles) != 0;
			bool canSwim = (flags & CanFitFlags.canSwim) != 0;
			bool cantWalk = (flags & CanFitFlags.cantWalk) != 0;

			if (this == Map.Internal)
				return false;

			if (x < 0 || y < 0 || x >= m_Width || y >= m_Height)
				return false;

			bool hasSurface = false;

			Tile lt = Tiles.GetLandTile(x, y);
			int lowZ = 0, avgZ = 0, topZ = 0;

			GetAverageZ(x, y, ref lowZ, ref avgZ, ref topZ);
			TileFlag landFlags = TileData.LandTable[lt.ID & 0x3FFF].Flags;

			if ((landFlags & TileFlag.Impassable) != 0 && avgZ > z && (z + height) > lowZ)
				return false;
			// can walk on land
			else if (!cantWalk && (landFlags & TileFlag.Impassable) == 0 && z == avgZ && !lt.Ignored)
				hasSurface = true;
			// can swim in water
			else if (canSwim && (landFlags & TileFlag.Impassable) != 0 && (landFlags & TileFlag.Wet) != 0 && z == avgZ && !lt.Ignored)
				hasSurface = true;

			Tile[] staticTiles = Tiles.GetStaticTiles(x, y, true);

			bool surface, impassable;

			for (int i = 0; i < staticTiles.Length; ++i)
			{
				ItemData id = TileData.ItemTable[staticTiles[i].ID & 0x3FFF];
				surface = id.Surface;
				impassable = id.Impassable;

				if ((surface || impassable) && (staticTiles[i].Z + id.CalcHeight) > z && (z + height) > staticTiles[i].Z)
					return false;
				else if (surface && !impassable && z == (staticTiles[i].Z + id.CalcHeight))
					hasSurface = true;
			}

			Sector sector = GetSector(x, y);
			Dictionary<Server.Serial, object>.ValueCollection items = sector.Items.Values, mobs = sector.Mobiles.Values;

			foreach (Item item in items)
			{
				if (item == null)
					continue;

				if (item.ItemID < 0x4000 && item.AtWorldPoint(x, y))
				{
					ItemData id = item.ItemData;
					surface = id.Surface;
					impassable = id.Impassable;

					if ((surface || impassable || (checkBlocksFit && item.BlocksFit)) && (item.Z + id.CalcHeight) > z && (z + height) > item.Z)
						return false;
					else if (surface && !impassable && z == (item.Z + id.CalcHeight))
						hasSurface = true;
				}
			}

			if (checkMobiles)
			{
				foreach (Mobile m in mobs)
				{
					if (m == null)
						continue;

					if (m.Alive == true || ignoreDeadMobiles == false)
					{
						if (m.Location.m_X == x && m.Location.m_Y == y)
						{
							if ((m.Z + 16) > z && (z + height) > m.Z)
							{
								return false;
							}
						}
					}
				}
			}

			return !requireSurface || hasSurface;
		}

		public bool CanSpawnMobile(Point3D p)
		{
			return CanSpawnMobile(p.m_X, p.m_Y, p.m_Z, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		public bool CanSpawnMobile(Point3D p, CanFitFlags flags)
		{
			return CanSpawnMobile(p.m_X, p.m_Y, p.m_Z, flags);
		}

		public bool CanSpawnMobile(Point2D p, int z)
		{
			return CanSpawnMobile(p.m_X, p.m_Y, z, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		public bool CanSpawnMobile(Point2D p, int z, CanFitFlags flags)
		{
			return CanSpawnMobile(p.m_X, p.m_Y, z, flags);
		}

		public bool CanSpawnMobile(int x, int y, int z)
		{
			return CanSpawnMobile(x, y, z, CanFitFlags.checkMobiles | CanFitFlags.requireSurface);
		}

		public bool CanSpawnMobile(int x, int y, int z, CanFitFlags flags)
		{
			if (!Region.Find(new Point3D(x, y, z), this).AllowSpawn())
				return false;

			return CanFit(x, y, z, 16, flags);
		}

		private class ZComparer : IComparer
		{
			public static readonly IComparer Default = new ZComparer();

			public int Compare(object x, object y)
			{
				Item a = (Item)x;
				Item b = (Item)y;

				return a.Z.CompareTo(b.Z);
			}
		}

		public int FixColumn(int x, int y)
		{
			int numberMoved = 0;
			Tile landTile = Tiles.GetLandTile(x, y);

			int landZ = 0, landAvg = 0, landTop = 0;
			GetAverageZ(x, y, ref landZ, ref landAvg, ref landTop);

			Tile[] tiles = Tiles.GetStaticTiles(x, y, true);

			ArrayList items = new ArrayList();

			IPooledEnumerable eable = GetItemsInRange(new Point3D(x, y, 0), 0);

			foreach (Item item in eable)
			{
				if (item.ItemID < 0x4000)
				{
					items.Add(item);

					if (items.Count > 100)
						break;
				}
			}

			eable.Free();

			if (items.Count > 100)
				return 0;

			items.Sort(ZComparer.Default);

			for (int i = 0; i < items.Count; ++i)
			{
				Item toFix = (Item)items[i];

				if (!toFix.Movable)
					continue;

				int z = int.MinValue;
				int currentZ = toFix.Z;

				if (!landTile.Ignored && landAvg <= currentZ)
					z = landAvg;

				for (int j = 0; j < tiles.Length; ++j)
				{
					Tile tile = tiles[j];
					ItemData id = TileData.ItemTable[tile.ID & 0x3FFF];

					int checkZ = tile.Z;
					int checkTop = checkZ + id.CalcHeight;

					if (checkTop == checkZ && !id.Surface)
						++checkTop;

					if (checkTop > z && checkTop <= currentZ)
						z = checkTop;
				}

				for (int j = 0; j < items.Count; ++j)
				{
					if (j == i)
						continue;

					Item item = (Item)items[j];
					ItemData id = item.ItemData;

					int checkZ = item.Z;
					int checkTop = checkZ + id.CalcHeight;

					if (checkTop == checkZ && !id.Surface)
						++checkTop;

					if (checkTop > z && checkTop <= currentZ)
						z = checkTop;
				}

				if (z != int.MinValue)
				{
					toFix.Location = new Point3D(toFix.X, toFix.Y, z);
					numberMoved++;
				}
			}

			if (numberMoved > 0)
				EventSink.InvokeDropMobile(new DropMobileEventArgs(this, x, y));

			return numberMoved;
		}

		public ArrayList GetTilesAt(Point2D p, bool items, bool land, bool statics)
		{
			ArrayList list = new ArrayList();

			if (this == Map.Internal)
				return list;

			if (land)
			{
				list.Add(Tiles.GetLandTile(p.m_X, p.m_Y));
			}

			if (statics)
			{
				list.AddRange(Tiles.GetStaticTiles(p.m_X, p.m_Y, true));
			}

			if (items)
			{
				Sector sector = GetSector(p);

				foreach (Item item in sector.Items.Values)
				{
					if (item == null)
						continue;

					if (item.AtWorldPoint(p.m_X, p.m_Y))
					{
						list.Add(new Tile((short)((item.ItemID & 0x3FFF) + 0x4000), (sbyte)item.Z));
					}
				}
			}

			return list;
		}

		public void Bound(int x, int y, out int newX, out int newY)
		{
			if (x < 0) newX = 0;
			else if (x >= m_Width) newX = m_Width - 1;
			else newX = x;

			if (y < 0) newY = 0;
			else if (y >= m_Height) newY = m_Height - 1;
			else newY = y;
		}

		public Point2D Bound(Point2D p)
		{
			int x = p.m_X, y = p.m_Y;

			if (x < 0) x = 0;
			else if (x >= m_Width) x = m_Width - 1;

			if (y < 0) y = 0;
			else if (y >= m_Height) y = m_Height - 1;

			return new Point2D(x, y);
		}

		public Map(int mapID, int mapIndex, int fileIndex, int width, int height, int season, string name, MapRules rules)
		{
			m_MapID = mapID;
			m_MapIndex = mapIndex;
			m_FileIndex = fileIndex;
			m_Width = width;
			m_Height = height;
			m_Season = season;
			m_Name = name;
			m_Rules = rules;
			m_Regions = new ArrayList();

			//m_Tiles = new TileMatrix( fileIndex, width, height );

			m_InvalidSector = new Sector(0, 0, this);

			m_SectorsWidth = width >> SectorShift;
			m_SectorsHeight = height >> SectorShift;

			m_Sectors = new Sector[m_SectorsWidth][];
		}

		public Sector GetSector(Point3D p)
		{
			return InternalGetSector(p.m_X >> SectorShift, p.m_Y >> SectorShift);
		}

		public Sector GetSector(Point2D p)
		{
			return InternalGetSector(p.m_X >> SectorShift, p.m_Y >> SectorShift);
		}

		public Sector GetSector(IPoint2D p)
		{
			return InternalGetSector(p.X >> SectorShift, p.Y >> SectorShift);
		}

		public Sector GetSector(int x, int y)
		{
			return InternalGetSector(x >> SectorShift, y >> SectorShift);
		}

		public Sector GetRealSector(int x, int y)
		{
			return InternalGetSector(x, y);
		}

		private Sector InternalGetSector(int x, int y)
		{
			if (x >= 0 && x < m_SectorsWidth && y >= 0 && y < m_SectorsHeight)
			{
				Sector[] xSectors = m_Sectors[x];

				if (xSectors == null)
					m_Sectors[x] = xSectors = new Sector[m_SectorsHeight];

				Sector sec = xSectors[y];

				if (sec == null)
					xSectors[y] = sec = new Sector(x, y, this);

				return sec;
			}
			else
			{
				return m_InvalidSector;
			}
		}

		public void ActivateSectors(int cx, int cy)
		{
			for (int x = cx - SectorActiveRange; x <= cx + SectorActiveRange; ++x)
			{
				for (int y = cy - SectorActiveRange; y <= cy + SectorActiveRange; ++y)
				{
					Sector sect = GetRealSector(x, y);
					if (sect != m_InvalidSector)
						sect.Activate();
				}
			}
		}

		public void DeactivateSectors(int cx, int cy)
		{
			for (int x = cx - SectorActiveRange; x <= cx + SectorActiveRange; ++x)
			{
				for (int y = cy - SectorActiveRange; y <= cy + SectorActiveRange; ++y)
				{
					Sector sect = GetRealSector(x, y);
					if (sect != m_InvalidSector && !PlayersInRange(sect, SectorActiveRange))
						sect.Deactivate();
				}
			}
		}

		private bool PlayersInRange(Sector sect, int range)
		{
			for (int x = sect.X - range; x <= sect.X + range; ++x)
			{
				for (int y = sect.Y - range; y <= sect.Y + range; ++y)
				{
					Sector check = GetRealSector(x, y);
					if (check != m_InvalidSector && check.Players.Count > 0)
						return true;
				}
			}

			return false;
		}

		public void OnClientChange(NetState oldState, NetState newState, Mobile m)
		{
			if (this == Map.Internal)
				return;

			GetSector(m).OnClientChange(oldState, newState);
		}

		public void OnEnter(Mobile m)
		{
			if (this == Map.Internal)
				return;

			Sector sector = GetSector(m);

			sector.OnEnter(m);

			if (sector.Active)
				m.OnSectorActivate();
			else
				m.OnSectorDeactivate();
		}

		public void OnEnter(Item item)
		{
			if (this == Map.Internal)
				return;

			GetSector(item).OnEnter(item);

			if (item is BaseMulti)
			{
				MultiComponentList mcl = ((BaseMulti)item).Components;

				Sector start = GetMultiMinSector(item.Location, mcl);
				Sector end = GetMultiMaxSector(item.Location, mcl);

				AddMulti(item, start, end);
			}
		}

		public void OnLeave(Mobile m)
		{
			if (this == Map.Internal)
				return;

			GetSector(m).OnLeave(m);
		}

		public void OnLeave(Item item)
		{
			if (this == Map.Internal)
				return;

			GetSector(item).OnLeave(item);

			if (item is BaseMulti)
			{
				MultiComponentList mcl = ((BaseMulti)item).Components;

				Sector start = GetMultiMinSector(item.Location, mcl);
				Sector end = GetMultiMaxSector(item.Location, mcl);

				RemoveMulti(item, start, end);
			}
		}

		public void RemoveMulti(Item item, Sector start, Sector end)
		{
			if (this == Map.Internal)
				return;

			for (int x = start.X; x <= end.X; ++x)
				for (int y = start.Y; y <= end.Y; ++y)
					InternalGetSector(x, y).OnMultiLeave(item);
		}

		public void AddMulti(Item item, Sector start, Sector end)
		{
			if (this == Map.Internal)
				return;

			for (int x = start.X; x <= end.X; ++x)
				for (int y = start.Y; y <= end.Y; ++y)
					InternalGetSector(x, y).OnMultiEnter(item);
		}

		public Sector GetMultiMinSector(Point3D loc, MultiComponentList mcl)
		{
			return GetSector(Bound(new Point2D(loc.m_X + mcl.Min.m_X, loc.m_Y + mcl.Min.m_Y)));
		}

		public Sector GetMultiMaxSector(Point3D loc, MultiComponentList mcl)
		{
			return GetSector(Bound(new Point2D(loc.m_X + mcl.Max.m_X, loc.m_Y + mcl.Max.m_Y)));
		}

		public void OnMove(Point3D oldLocation, Mobile m)
		{
			if (this == Map.Internal)
				return;

			Sector oldSector = GetSector(oldLocation);
			Sector newSector = GetSector(m.Location);

			if (oldSector != newSector)
			{
				oldSector.OnLeave(m);
				newSector.OnEnter(m);

				if (oldSector.Active != newSector.Active)
				{
					if (newSector.Active)
						m.OnSectorActivate();
					else
						m.OnSectorDeactivate();
				}
			}
		}

		public void OnMove(Point3D oldLocation, Item item)
		{
			if (this == Map.Internal)
				return;

			Sector oldSector = GetSector(oldLocation);
			Sector newSector = GetSector(item.Location);

			if (oldSector != newSector)
			{
				oldSector.OnLeave(item);
				newSector.OnEnter(item);
			}

			if (item is BaseMulti)
			{
				MultiComponentList mcl = ((BaseMulti)item).Components;

				Sector start = GetMultiMinSector(item.Location, mcl);
				Sector end = GetMultiMaxSector(item.Location, mcl);

				Sector oldStart = GetMultiMinSector(oldLocation, mcl);
				Sector oldEnd = GetMultiMaxSector(oldLocation, mcl);

				if (oldStart != start || oldEnd != end)
				{
					RemoveMulti(item, oldStart, oldEnd);
					AddMulti(item, start, end);
				}
			}
		}

		public TileMatrix Tiles
		{
			get
			{
				if (m_Tiles == null)
					m_Tiles = new TileMatrix(this, m_FileIndex, m_MapID, m_Width, m_Height);

				return m_Tiles;
			}
		}

		public int MapID
		{
			get
			{
				return m_MapID;
			}
		}

		public int MapIndex
		{
			get
			{
				return m_MapIndex;
			}
		}

		public int Width
		{
			get
			{
				return m_Width;
			}
		}

		public int Height
		{
			get
			{
				return m_Height;
			}
		}

		public ArrayList Regions
		{
			get
			{
				return m_Regions;
			}
		}

		public Region DefaultRegion
		{
			get
			{
				if (m_DefaultRegion == null)
					m_DefaultRegion = new Region("", "", this);

				return m_DefaultRegion;
			}
			set
			{
				m_DefaultRegion = value;
			}
		}

		public MapRules Rules
		{
			get
			{
				return m_Rules;
			}
			set
			{
				m_Rules = value;
			}
		}

		public Sector InvalidSector
		{
			get
			{
				return m_InvalidSector;
			}
		}

		public string Name
		{
			get
			{
				return m_Name;
			}
			set
			{
				m_Name = value;
			}
		}

		private enum SectorEnumeratorType
		{
			Mobiles,
			Items,
			Clients
		}

		public class NullEnumerable : IPooledEnumerable
		{
			private InternalEnumerator m_Enumerator;

			public static readonly NullEnumerable Instance = new NullEnumerable();

			private NullEnumerable()
			{
				m_Enumerator = new InternalEnumerator();
			}

			public IEnumerator GetEnumerator()
			{
				return m_Enumerator;
			}

			public void Free()
			{
			}

			private class InternalEnumerator : IEnumerator
			{
				public void Reset()
				{
				}

				public object Current
				{
					get
					{
						return null;
					}
				}

				public bool MoveNext()
				{
					return false;
				}
			}
		}

		private class PooledEnumerable : IPooledEnumerable, IDisposable
		{
			private IPooledEnumerator m_Enumerator;

			private static Queue m_InstancePool = new Queue();
			private static int m_Depth = 0;
			private static int m_HighWaterMark = 0;

			public enum status { normal, derived_disposed, derived_moveNext };
			public status Status = status.normal;

			public static PooledEnumerable Instantiate(IPooledEnumerator etor)
			{
				++m_Depth;                          // track current depth
				if (m_Depth > m_HighWaterMark)      // track max depth
					m_HighWaterMark = m_Depth;

				if (m_Depth >= 5)
					Console.WriteLine("PooledEnumerable: Depth = {0}, High Water Mark = {1}", m_Depth, m_HighWaterMark);

				PooledEnumerable e;

				if (m_InstancePool.Count > 0)
				{
					e = (PooledEnumerable)m_InstancePool.Dequeue();
					e.m_Enumerator = etor;

					if (e != null && e.Status != PooledEnumerable.status.normal && e.Status != PooledEnumerable.status.derived_moveNext)
					{	// always happens. Back to the drawing board.
						// string message = String.Format("PooledEnumerable.Instantiate() called after the derived class was: {0}", e.Status.ToString());
						// Console.WriteLine(message);
					}

					if (etor == null)
					{   // TEST
						string message = String.Format("PooledEnumerable.Instantiate() etor starting out null");
						Console.WriteLine(message);
					}

				}
				else
				{
					e = new PooledEnumerable(etor);
				}

				etor.Enumerable = e;

				// reset
				e.Status = PooledEnumerable.status.normal;

				return e;
			}

			private PooledEnumerable(IPooledEnumerator etor)
			{
				m_Enumerator = etor;
			}

			public IEnumerator GetEnumerator()
			{
				if (m_Enumerator == null)
				{
					string message = "GetEnumerator() called after Free()";
					if (Status != status.normal)
						message = String.Format("GetEnumerator() called after the derived class was: {0}", Status.ToString());
					throw new ObjectDisposedException("PooledEnumerable", message);
				}

				return m_Enumerator;
			}

			public void Free()
			{
				if (m_Enumerator != null)
				{
					// reset
					this.Status = PooledEnumerable.status.normal;

					m_InstancePool.Enqueue(this);

					m_Enumerator.Free();
					m_Enumerator = null;

					--m_Depth;
				}
			}

			public void Dispose()
			{
				//if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
				//(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_disposed;

				Free();
			}
		}

		private class TypedEnumerator : IPooledEnumerator, IDisposable
		{
			private IPooledEnumerable m_Enumerable;

			public IPooledEnumerable Enumerable
			{
				get { return m_Enumerable; }
				set { m_Enumerable = value; }
			}

			private Map m_Map;
			private Rectangle2D m_Bounds;
			private SectorEnumerator m_Enumerator;
			private SectorEnumeratorType m_Type;
			private object m_Current;

			private static Queue m_InstancePool = new Queue();

			public static TypedEnumerator Instantiate(Map map, Rectangle2D bounds, SectorEnumeratorType type)
			{
				TypedEnumerator e;

				if (m_InstancePool.Count > 0)
				{
					e = (TypedEnumerator)m_InstancePool.Dequeue();

					e.m_Map = map;
					e.m_Bounds = bounds;
					e.m_Type = type;

					e.Reset();
				}
				else
				{
					e = new TypedEnumerator(map, bounds, type);
				}

				return e;
			}

			public void Free()
			{
				if (m_Map == null)
					return;

				m_InstancePool.Enqueue(this);

				m_Map = null;

				if (m_Enumerator != null)
				{
					m_Enumerator.Free();
					m_Enumerator = null;
				}

				if (m_Enumerable != null)
				{
					m_Enumerable.Free();
				}
			}

			public TypedEnumerator(Map map, Rectangle2D bounds, SectorEnumeratorType type)
			{
				m_Map = map;
				m_Bounds = bounds;
				m_Type = type;

				Reset();
			}

			public object Current
			{
				get
				{
					return m_Current;
				}
			}

			public bool MoveNext()
			{
				while (true)
				{
					if (m_Enumerator.MoveNext())
					{
						object o = m_Enumerator.Current;

						if (o is Mobile)
						{
							Mobile m = (Mobile)o;

							if (!m.Deleted && m_Bounds.Contains(m.Location))
							{
								m_Current = o;
								return true;
							}
						}
						else if (o is Item)
						{
							Item item = (Item)o;

							if (!item.Deleted && item.Parent == null && m_Bounds.Contains(item.Location))
							{
								m_Current = o;
								return true;
							}
						}
						else if (o is NetState)
						{
							Mobile m = ((NetState)o).Mobile;

							if (m != null && !m.Deleted && m_Bounds.Contains(m.Location))
							{
								m_Current = o;
								return true;
							}
						}
					}
					else
					{
						m_Current = null;

						if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
							(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_moveNext;

						m_Enumerator.Free();
						m_Enumerator = null;

						return false;
					}
				}
			}

			public void Reset()
			{
				m_Current = null;

				if (m_Enumerator != null)
					m_Enumerator.Free();

				m_Enumerator = SectorEnumerator.Instantiate(m_Map, m_Bounds, m_Type);//new SectorEnumerator( m_Map, m_Origin, m_Type, m_Range );
			}

			public void Dispose()
			{
				if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
					(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_disposed;

				Free();
			}
		}

		private class ListManager
		{
			private Dictionary<Server.Serial, object>.ValueCollection m_vc;
			private Dictionary<Server.Serial, object>.ValueCollection.Enumerator m_enum;

			public ListManager(Dictionary<Server.Serial, object>.ValueCollection vc)
			{
				m_vc = vc;
				m_enum = vc.GetEnumerator();
			}
			public bool MoveNext() { return m_enum.MoveNext(); }
			public object Current { get { return m_enum.Current; /*m_enum.Value*/ } }
			public void Reset() { m_enum = m_vc.GetEnumerator(); /*m_enum.Reset();*/ }
			public int Count { get { return m_vc.Count; } }
		}

		private class MultiTileEnumerator : IPooledEnumerator, IDisposable
		{
			private IPooledEnumerable m_Enumerable;

			public IPooledEnumerable Enumerable
			{
				get { return m_Enumerable; }
				set { m_Enumerable = value; }
			}

			//private ArrayList m_List;
			private ListManager m_ListManager;
			private Point2D m_Location;
			private object m_Current;
			private int m_Index;

			private static Queue m_InstancePool = new Queue();

			public static MultiTileEnumerator Instantiate(Sector sector, Point2D loc)
			{
				MultiTileEnumerator e;

				if (m_InstancePool.Count > 0)
				{
					e = (MultiTileEnumerator)m_InstancePool.Dequeue();

					e.m_ListManager = new ListManager(sector.Multis.Values);
					//m_List = sector.Multis;
					e.m_Location = loc;

					e.Reset();
				}
				else
				{
					e = new MultiTileEnumerator(sector, loc);
				}

				return e;
			}

			private MultiTileEnumerator(Sector sector, Point2D loc)
			{
				m_ListManager = new ListManager(sector.Multis.Values);
				//m_List = sector.Multis;
				m_Location = loc;

				Reset();
			}

			public object Current
			{
				get
				{
					return m_Current;
				}
			}

			public bool MoveNext()
			{
				//while ( ++m_Index < m_List.Count )
				while (++m_Index < m_ListManager.Count)
				{
					m_ListManager.MoveNext();

					//BaseMulti m = m_List[m_Index] as BaseMulti;
					BaseMulti m = m_ListManager.Current as BaseMulti;

					if (m != null && !m.Deleted)
					{
						MultiComponentList list = m.Components;

						int xOffset = m_Location.m_X - (m.Location.m_X + list.Min.m_X);
						int yOffset = m_Location.m_Y - (m.Location.m_Y + list.Min.m_Y);

						if (xOffset >= 0 && xOffset < list.Width && yOffset >= 0 && yOffset < list.Height)
						{
							Tile[] tiles = list.Tiles[xOffset][yOffset];

							if (tiles.Length > 0)
							{
								// TODO: How to avoid this copy?
								Tile[] copy = new Tile[tiles.Length];

								for (int i = 0; i < copy.Length; ++i)
								{
									copy[i] = tiles[i];
									copy[i].Z += m.Z;
								}

								m_Current = copy;
								return true;
							}
						}
					}
				}

				return false;
			}

			public void Free()
			{
				//if ( m_List == null )
				//return;

				if (m_ListManager == null)
					return;

				m_InstancePool.Enqueue(this);

				//m_List = null;
				m_ListManager = null;

				if (m_Enumerable != null)
					m_Enumerable.Free();
			}

			public void Reset()
			{
				m_ListManager.Reset();
				m_Current = null;
				m_Index = -1;
			}

			public void Dispose()
			{
				if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
					(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_disposed;

				Free();
			}
		}

		private class ObjectEnumerator : IPooledEnumerator, IDisposable
		{
			private IPooledEnumerable m_Enumerable;

			public IPooledEnumerable Enumerable
			{
				get { return m_Enumerable; }
				set { m_Enumerable = value; }
			}

			private Map m_Map;
			private Rectangle2D m_Bounds;
			private SectorEnumerator m_Enumerator;
			private int m_Stage; // 0 = items, 1 = mobiles
			private object m_Current;

			private static Queue m_InstancePool = new Queue();

			public static ObjectEnumerator Instantiate(Map map, Rectangle2D bounds)
			{
				ObjectEnumerator e;

				if (m_InstancePool.Count > 0)
				{
					e = (ObjectEnumerator)m_InstancePool.Dequeue();

					e.m_Map = map;
					e.m_Bounds = bounds;

					e.Reset();
				}
				else
				{
					e = new ObjectEnumerator(map, bounds);
				}

				return e;
			}

			public void Free()
			{
				if (m_Map == null)
					return;

				m_InstancePool.Enqueue(this);

				m_Map = null;

				if (m_Enumerator != null)
				{
					m_Enumerator.Free();
					m_Enumerator = null;
				}

				if (m_Enumerable != null)
					m_Enumerable.Free();
			}

			private ObjectEnumerator(Map map, Rectangle2D bounds)
			{
				m_Map = map;
				m_Bounds = bounds;

				Reset();
			}

			public object Current
			{
				get
				{
					return m_Current;
				}
			}

			public bool MoveNext()
			{
				while (true)
				{
					if (m_Enumerator.MoveNext())
					{
						object o = m_Enumerator.Current;

						if (o is Mobile)
						{
							Mobile m = (Mobile)o;

							if (m_Bounds.Contains(m.Location))
							{
								m_Current = o;
								return true;
							}
						}
						else if (o is Item)
						{
							Item item = (Item)o;

							if (item.Parent == null && m_Bounds.Contains(item.Location))
							{
								m_Current = o;
								return true;
							}
						}
					}
					else if (m_Stage == 0)
					{
						m_Enumerator.Free();
						m_Enumerator = SectorEnumerator.Instantiate(m_Map, m_Bounds, SectorEnumeratorType.Mobiles);

						m_Current = null;
						m_Stage = 1;
					}
					else
					{
						m_Current = null;

						if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
							(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_moveNext;

						m_Enumerator.Free();
						m_Enumerator = null;

						m_Stage = -1;

						return false;
					}
				}
			}

			public void Reset()
			{
				m_Stage = 0;

				m_Current = null;

				if (m_Enumerator != null)
					m_Enumerator.Free();

				m_Enumerator = SectorEnumerator.Instantiate(m_Map, m_Bounds, SectorEnumeratorType.Items);
			}

			public void Dispose()
			{
				if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
					(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_disposed;

				Free();
			}
		}

		private class SectorEnumerator : IPooledEnumerator, IDisposable
		{
			private IPooledEnumerable m_Enumerable;

			public IPooledEnumerable Enumerable
			{
				get { return m_Enumerable; }
				set { m_Enumerable = value; }
			}

			private Map m_Map;
			private Rectangle2D m_Bounds;

			private int m_xSector, m_ySector;
			private int m_xSectorStart, m_ySectorStart;
			private int m_xSectorEnd, m_ySectorEnd;
			//private ArrayList m_CurrentList;
			private ListManager m_ListManager;
			private int m_CurrentIndex;
			private SectorEnumeratorType m_Type;

			private static Queue m_InstancePool = new Queue();

			public static SectorEnumerator Instantiate(Map map, Rectangle2D bounds, SectorEnumeratorType type)
			{
				SectorEnumerator e;

				if (m_InstancePool.Count > 0)
				{
					e = (SectorEnumerator)m_InstancePool.Dequeue();

					e.m_Map = map;
					e.m_Bounds = bounds;
					e.m_Type = type;

					e.Reset();
				}
				else
				{
					e = new SectorEnumerator(map, bounds, type);
				}

				return e;
			}

			public void Free()
			{
				if (m_Map == null)
					return;

				m_InstancePool.Enqueue(this);

				m_Map = null;

				if (m_Enumerable != null)
					m_Enumerable.Free();
			}

			private SectorEnumerator(Map map, Rectangle2D bounds, SectorEnumeratorType type)
			{
				m_Map = map;
				m_Bounds = bounds;
				m_Type = type;

				Reset();
			}

			private Dictionary<Server.Serial, object>.ValueCollection GetListForSector(Sector sector)
			{
				switch (m_Type)
				{
					case SectorEnumeratorType.Clients: return sector.Clients.Values;
					case SectorEnumeratorType.Mobiles: return sector.Mobiles.Values;
					case SectorEnumeratorType.Items: return sector.Items.Values;
					default: throw new Exception("Invalid SectorEnumeratorType");
				}
			}

			public object Current
			{
				get
				{
					//return m_CurrentList[m_CurrentIndex];
					return m_ListManager.Current;

					/*try
					{
						return m_CurrentList[m_CurrentIndex];
					}
					catch
					{
						Console.WriteLine( "Warning: Object removed during enumeration. May not be recoverable" );

						m_CurrentIndex = -1;
						m_CurrentList = GetListForSector( m_Map.InternalGetSector( m_xSector, m_ySector ) );

						if ( MoveNext() )
						{
							return Current;
						}
						else
						{
							throw new Exception( "Object disposed during enumeration. Was not recoverable." );
						}
					}*/
				}
			}

			public bool MoveNext()
			{
				while (true)
				{
					bool sanity = m_ListManager.MoveNext();

					++m_CurrentIndex;

					//if ( m_CurrentIndex == m_CurrentList.Count )
					if (m_CurrentIndex == m_ListManager.Count)
					{
						++m_ySector;

						if (m_ySector > m_ySectorEnd)
						{
							m_ySector = m_ySectorStart;
							++m_xSector;

							if (m_xSector > m_xSectorEnd)
							{
								m_CurrentIndex = -1;
								//m_CurrentList = null;
								m_ListManager = null;

								return false;
							}
						}

						m_CurrentIndex = -1;
						//m_CurrentList = GetListForSector( m_Map.InternalGetSector( m_xSector, m_ySector ) );//m_Map.m_Sectors[m_xSector][m_ySector] );
						m_ListManager = new ListManager(GetListForSector(m_Map.InternalGetSector(m_xSector, m_ySector)));
					}
					else
					{
						return true;
					}
				}
			}

			public void Reset()
			{
				m_Map.Bound(m_Bounds.Start.m_X, m_Bounds.Start.m_Y, out m_xSectorStart, out m_ySectorStart);
				m_Map.Bound(m_Bounds.End.m_X - 1, m_Bounds.End.m_Y - 1, out m_xSectorEnd, out m_ySectorEnd);

				m_xSector = m_xSectorStart >>= Map.SectorShift;
				m_ySector = m_ySectorStart >>= Map.SectorShift;

				m_xSectorEnd >>= Map.SectorShift;
				m_ySectorEnd >>= Map.SectorShift;

				m_CurrentIndex = -1;
				//m_CurrentList = GetListForSector( m_Map.InternalGetSector( m_xSector, m_ySector ) );
				m_ListManager = new ListManager(GetListForSector(m_Map.InternalGetSector(m_xSector, m_ySector)));
			}

			public void Dispose()
			{
				if (m_Enumerable is PooledEnumerable && (m_Enumerable as PooledEnumerable).Status == PooledEnumerable.status.normal)
					(m_Enumerable as PooledEnumerable).Status = PooledEnumerable.status.derived_disposed;

				Free();
			}
		}

		public bool LineOfSight(object from, object dest)
		{
			if (from == dest || (from is Mobile && ((Mobile)from).AccessLevel > AccessLevel.Player))
				return true;
			else if (dest is Item && from is Mobile && ((Item)dest).RootParent == from)
				return true;

			return LineOfSight(GetPoint(from, true), GetPoint(dest, false));
		}

		public Point3D GetPoint(object o, bool eye)
		{
			Point3D p;

			if (o is Mobile)
			{
				p = ((Mobile)o).Location;
				p.Z += 14;//eye ? 15 : 10;
			}
			else if (o is Item)
			{
				p = ((Item)o).GetWorldLocation();
				p.Z += (((Item)o).ItemData.Height / 2) + 1;
			}
			else if (o is Point3D)
			{
				p = (Point3D)o;
			}
			else if (o is LandTarget)
			{
				p = ((LandTarget)o).Location;

				int low = 0, avg = 0, top = 0;
				GetAverageZ(p.X, p.Y, ref low, ref avg, ref top);

				p.Z = top + 1;
			}
			else if (o is StaticTarget)
			{
				StaticTarget st = (StaticTarget)o;
				ItemData id = TileData.ItemTable[st.ItemID & 0x3FFF];

				p = new Point3D(st.X, st.Y, st.Z - id.CalcHeight + (id.Height / 2) + 1);
			}
			else if (o is IPoint3D)
			{
				p = new Point3D((IPoint3D)o);
			}
			else
			{
				Console.WriteLine("Warning: Invalid object ({0}) in line of sight", o);
				p = Point3D.Zero;
			}

			return p;
		}

		public bool LineOfSight(Mobile from, Point3D target)
		{
			if (from.AccessLevel > AccessLevel.Player)
				return true;

			Point3D eye = from.Location;

			eye.Z += 14;

			return LineOfSight(eye, target);
		}

		// wea: additional bool option for audible check
		// (ignores NoShoot TileFlag)
		public bool LineOfSight(Mobile from, Mobile to)
		{
			return LineOfSight(from, to, false);
		}

		public bool LineOfSight(Mobile from, Mobile to, bool audible)
		{
			if (from == to || from.AccessLevel > AccessLevel.Player)
				return true;

			Point3D eye = from.Location;
			Point3D target = to.Location;

			eye.Z += 14;
			target.Z += 14;//10;

			return LineOfSight(eye, target, audible);
		}

		private static int[] m_InvalidLandTiles = new int[] { 0x244 };

		public static int[] InvalidLandTiles
		{
			get { return m_InvalidLandTiles; }
			set { m_InvalidLandTiles = value; }
		}

		private static Point3DList m_PathList = new Point3DList();

		private static int m_MaxLOSDistance = 25;

		public static int MaxLOSDistance
		{
			get { return m_MaxLOSDistance; }
			set { m_MaxLOSDistance = value; }
		}

		// wea: additional bool option for audible check
		// (ignores NoShoot TileFlag)
		public bool LineOfSight(Point3D org, Point3D dest)
		{
			return LineOfSight(org, dest, false);
		}

		public bool LineOfSight(Point3D org, Point3D dest, bool audible)
		{
			if (this == Map.Internal)
				return false;

			if (!Utility.InRange(org, dest, m_MaxLOSDistance))
				return false;

			Point3D start = org;
			Point3D finsh = dest;

			if (org.X > dest.X || (org.X == dest.X && org.Y > dest.Y) || (org.X == dest.X && org.Y == dest.Y && org.Z > dest.Z))
			{
				Point3D swap = org;
				org = dest;
				dest = swap;
			}

			double rise, run, zslp;
			double sq3d;
			double x, y, z;
			int xd, yd, zd;
			int ix, iy, iz;
			int height;
			bool found;
			Point3D p;
			Point3DList path = m_PathList;
			TileFlag flags;

			if (org == dest)
				return true;

			if (path.Count > 0)
				path.Clear();

			xd = dest.m_X - org.m_X;
			yd = dest.m_Y - org.m_Y;
			zd = dest.m_Z - org.m_Z;
			zslp = Math.Sqrt(xd * xd + yd * yd);
			if (zd != 0)
				sq3d = Math.Sqrt(zslp * zslp + zd * zd);
			else
				sq3d = zslp;

			rise = ((float)yd) / sq3d;
			run = ((float)xd) / sq3d;
			zslp = ((float)zd) / sq3d;

			y = org.m_Y;
			z = org.m_Z;
			x = org.m_X;
			while (Utility.NumberBetween(x, dest.m_X, org.m_X, 0.5) && Utility.NumberBetween(y, dest.m_Y, org.m_Y, 0.5) && Utility.NumberBetween(z, dest.m_Z, org.m_Z, 0.5))
			{
				ix = (int)Math.Round(x);
				iy = (int)Math.Round(y);
				iz = (int)Math.Round(z);
				if (path.Count > 0)
				{
					p = path.Last;

					if (p.m_X != ix || p.m_Y != iy || p.m_Z != iz)
						path.Add(ix, iy, iz);
				}
				else
				{
					path.Add(ix, iy, iz);
				}
				x += run;
				y += rise;
				z += zslp;
			}

			if (path.Count == 0)
				return true;//<--should never happen, but to be safe.

			p = path.Last;

			if (p != dest)
				path.Add(dest);

			Point3D pTop = org, pBottom = dest;
			Utility.FixPoints(ref pTop, ref pBottom);

			int pathCount = path.Count;

			for (int i = 0; i < pathCount; ++i)
			{
				Point3D point = path[i];

				Tile landTile = Tiles.GetLandTile(point.X, point.Y);
				int landZ = 0, landAvg = 0, landTop = 0;
				GetAverageZ(point.m_X, point.m_Y, ref landZ, ref landAvg, ref landTop);

				if (landZ <= point.m_Z && landTop >= point.m_Z && (point.m_X != finsh.m_X || point.m_Y != finsh.m_Y || landZ > finsh.m_Z || landTop < finsh.m_Z) && !landTile.Ignored)
					return false;

				/* --Do land tiles need to be checked?  There is never land between two people, always statics.--
				Tile landTile = Tiles.GetLandTile( point.X, point.Y );
				if ( landTile.Z-1 >= point.Z && landTile.Z+1 <= point.Z && (TileData.LandTable[landTile.ID & 0x3FFF].Flags & TileFlag.Impassable) != 0 )
					return false;
				*/

				Tile[] statics = Tiles.GetStaticTiles(point.m_X, point.m_Y, true);

				bool contains = false;
				int ltID = landTile.ID;

				for (int j = 0; !contains && j < m_InvalidLandTiles.Length; ++j)
					contains = (ltID == m_InvalidLandTiles[j]);

				if (contains && statics.Length == 0)
				{
					IPooledEnumerable eable = GetItemsInRange(point, 0);

					foreach (Item item in eable)
					{
						if (item.Visible)
							contains = false;

						if (!contains)
							break;
					}

					eable.Free();

					if (contains)
						return false;
				}

				for (int j = 0; j < statics.Length; ++j)
				{
					Tile t = statics[j];

					ItemData id = TileData.ItemTable[t.ID & 0x3FFF];

					flags = id.Flags;
					height = id.CalcHeight;

					if (t.Z <= point.Z && t.Z + height >= point.Z && (flags & (TileFlag.Window | TileFlag.NoShoot)) != 0)
					{
						//Pix: special case for right flap of orc fort
						if (t.ID == 0x422D && (start == ORCFLAPRIGHT || finsh == ORCFLAPRIGHT))
							continue;

						if (point.m_X == finsh.m_X && point.m_Y == finsh.m_Y && t.Z <= finsh.m_Z && t.Z + height >= finsh.m_Z)
							continue;

						return false;
					}

					/*if ( t.Z <= point.Z && t.Z+height >= point.Z && (flags&TileFlag.Window)==0 && (flags&TileFlag.NoShoot)!=0
						&& ( (flags&TileFlag.Wall)!=0 || (flags&TileFlag.Roof)!=0 || (((flags&TileFlag.Surface)!=0 && zd != 0)) ) )*/
					/*{
						//Console.WriteLine( "LoS: Blocked by Static \"{0}\" Z:{1} T:{3} P:{2} F:x{4:X}", TileData.ItemTable[t.ID&0x3FFF].Name, t.Z, point, t.Z+height, flags );
						//Console.WriteLine( "if ( {0} && {1} && {2} && ( {3} || {4} || {5} || ({6} && {7} && {8}) ) )", t.Z <= point.Z, t.Z+height >= point.Z, (flags&TileFlag.Window)==0, (flags&TileFlag.Impassable)!=0, (flags&TileFlag.Wall)!=0, (flags&TileFlag.Roof)!=0, (flags&TileFlag.Surface)!=0, t.Z != dest.Z, zd != 0 ) ;
						return false;
					}*/
				}
			}

			Rectangle2D rect = new Rectangle2D(pTop.m_X, pTop.m_Y, (pBottom.m_X - pTop.m_X) + 1, (pBottom.m_Y - pTop.m_Y) + 1);

			IPooledEnumerable area = GetItemsInBounds(rect);

			foreach (Item i in area)
			{
				if (!i.Visible)
					continue;

				if (i.ItemID >= 0x4000)
					continue;

				ItemData id = i.ItemData;
				flags = id.Flags;

				// wea: altered so that continues if NoShoot only and this is audible check
				// if ( (flags & (TileFlag.Window | TileFlag.NoShoot)) == 0 )
				if ((audible || (flags & TileFlag.NoShoot) == 0) && (flags & (TileFlag.Window | TileFlag.Wall)) == 0)
					continue;

				height = id.CalcHeight;

				found = false;

				int count = path.Count;

				for (int j = 0; j < count; ++j)
				{
					Point3D point = path[j];
					Point3D loc = i.Location;

					//if ( t.Z <= point.Z && t.Z+height >= point.Z && ( height != 0 || ( t.Z == dest.Z && zd != 0 ) ) )
					if (loc.m_X == point.m_X && loc.m_Y == point.m_Y &&
						loc.m_Z <= point.m_Z && loc.m_Z + height >= point.m_Z)
					{
						if (loc.m_X == finsh.m_X && loc.m_Y == finsh.m_Y && loc.m_Z <= finsh.m_Z && loc.m_Z + height >= finsh.m_Z)
							continue;

						found = true;
						break;
					}
				}

				if (!found)
					continue;

				area.Free();

				return false;

				/*if ( (flags & (TileFlag.Impassable | TileFlag.Surface | TileFlag.Roof)) != 0 )

				//flags = TileData.ItemTable[i.ItemID&0x3FFF].Flags;
				//if ( (flags&TileFlag.Window)==0 && (flags&TileFlag.NoShoot)!=0 && ( (flags&TileFlag.Wall)!=0 || (flags&TileFlag.Roof)!=0 || (((flags&TileFlag.Surface)!=0 && zd != 0)) ) )
				{
					//height = TileData.ItemTable[i.ItemID&0x3FFF].Height;
					//Console.WriteLine( "LoS: Blocked by ITEM \"{0}\" P:{1} T:{2} F:x{3:X}", TileData.ItemTable[i.ItemID&0x3FFF].Name, i.Location, i.Location.Z+height, flags );
					area.Free();
					return false;
				}*/
			}

			area.Free();

			return true;
		}

		/*private static int m_GlobalLight = 0;
		public static int GlobalLight
		{
			get
			{
				return m_GlobalLight;
			}
			set
			{
				m_GlobalLight = value;
			}
		}*/
	}
}