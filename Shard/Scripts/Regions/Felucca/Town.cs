#if OLD_REGIONS
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

/* Scripts/Regions/Felucca/Town.cs
 * ChangeLog
 *	04/24/09, plasma
 *		Commented out all regions, replaced with DRDT
 *	2/2/06, Adam
 *		Remove Yew from the guarded regions
 *	9/16/05, Adam
 *		Remove Vesper from the guarded regions
 *	4/11/05, Adam
 *		Remove Ocllo from the guarded regions
 *	9/17/04, Adam
 *		Remove Serpent's Hold from the guarded regions
 *	3/26/04 changes by mith
 *		Initialize():  Removed AngelIsland initialization and placed new Initialize event in AngelIsland.cs.
 *	3/15/04 changes by mith	
 *		Initialize(): Added new GuardedRegion, AngelIsland, based on definition in Regions.xml.
 */

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
	public class FeluccaTown : GuardedRegion
	{
		public static new void Initialize()
		{
			//replaced with dynamic region(s)
			/*
			Region.AddRegion( new FeluccaTown( "Cove" ) );
			Region.AddRegion( new FeluccaTown( "Britain" ) );
			Region.AddRegion( new FeluccaTown( "Minoc" ) );
			Region.AddRegion( new FeluccaTown( "Trinsic" ) );
			Region.AddRegion( new FeluccaTown( "Skara Brae" ) );
			Region.AddRegion( new FeluccaTown( "Nujel'm" ) );
			Region.AddRegion( new FeluccaTown( "Moonglow" ) );
			Region.AddRegion( new FeluccaTown( "Magincia" ) );
			Region.AddRegion( new FeluccaTown( "Delucia" ) );
			Region.AddRegion( new FeluccaTown( "Papua" ) );
			*/
			
			// Region.AddRegion(new FeluccaTown("Jhelom"));
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Wind" ) ) );
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Serpent's Hold" ) ) );
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Buccaneer's Den" ) ) );
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Ocllo" ) ) );
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Vesper" ) ) );
			//Region.AddRegion( GuardedRegion.Disable( new FeluccaTown( "Yew" ) ) );

			//Region.AddRegion( new GuardedRegion( "", "Moongates", Map.Felucca, typeof( WarriorGuard ) ) );
		}

		public FeluccaTown( string name ) : this( name, typeof( WarriorGuard ) )
		{
		}

		public FeluccaTown( string name, Type guardType ) : base( "the town of", name, Map.Felucca, guardType )
		{
		}
	}
}
#endif
