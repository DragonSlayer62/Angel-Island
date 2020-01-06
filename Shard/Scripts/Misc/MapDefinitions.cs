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

/* Scripts/Misc/MapDefinitions.cs
 * ChangeLog
 *  12/23/04, Jade
 *      Changed Ilshenar to a Felucca ruleset.
 *  5/26/04, Old Salty
 * 		Changed Felucca season to 1 (Old Style UO landscape).  
 *	5/21/04, mith
 *		Changed season to 5 (Blooming/Spring). Added comment detailing which numbers = which seasons.
 */

using System;
using Server;

namespace Server.Misc
{
	public class MapDefinitions
	{
		public static void Configure()
		{
			/* Here we configure all maps. Some notes:
			 * 
			 * 1) The first 32 maps are reserved for core use.
			 * 2) Map 0x7F is reserved for core use.
			 * 3) Map 0xFF is reserved for core use.
			 * 4) Changing or removing any predefined maps may cause server instability.
			 */

			RegisterMap(0, 0, 0, 6144, 4096, 1, "Felucca", MapRules.FeluccaRules);
			RegisterMap(1, 1, 0, 6144, 4096, 0, "Trammel", MapRules.TrammelRules);
			RegisterMap(2, 2, 2, 2304, 1600, 1, "Ilshenar", MapRules.FeluccaRules);
			RegisterMap(3, 3, 3, 2560, 2048, 1, "Malas", MapRules.TrammelRules);

			RegisterMap(0x7F, 0x7F, 0x7F, Map.SectorSize, Map.SectorSize, 1, "Internal", MapRules.Internal);

			/* Example of registering a custom map:
			 * RegisterMap( 32, 0, 0, 6144, 4096, 3, "Iceland", MapRules.FeluccaRules );
			 * 
			 * Defined:
			 * RegisterMap( <index>, <mapID>, <fileIndex>, <width>, <height>, <season>, <name>, <rules> );
			 *  - <index> : An unreserved unique index for this map
			 *  - <mapID> : An identification number used in client communications. For any visible maps, this value must be from 0-3
			 *  - <fileIndex> : A file identification number. For any visible maps, this value must be 0, 2, or 3
			 *  - <width>, <height> : Size of the map (in tiles)
			 *  - <season> : 0 = spring, 1 = summer, 2 = autumn, 3 = winter, 4 = desolation, 5 = blooming (pre-UO:R Feluca)
			 *  - <name> : Reference name for the map, used in props gump, get/set commands, region loading, etc
			 *  - <rules> : Rules and restrictions associated with the map. See documentation for details
			*/
		}

		public static void RegisterMap(int mapIndex, int mapID, int fileIndex, int width, int height, int season, string name, MapRules rules)
		{
			Map newMap = new Map(mapID, mapIndex, fileIndex, width, height, season, name, rules);

			Map.Maps[mapIndex] = newMap;
			Map.AllMaps.Add(newMap);
		}
	}
}
