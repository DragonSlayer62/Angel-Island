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

/* Scripts/Regions/NoHousingRegion.cs
 * CHANGELOG
 *	04/24/09, plasma
 *		Commented out all regions, replaced with DRDT
 *	?? unknown
 *		Added "Yew Orc Fort Small Area" as a no housing region
 *	3/11/04: Pixie
 *		Added Ocllo Island no housing region.
 *	3/6/05, Adam
 *		Add "Shame Entrance", "Ice Entrance", "Hythloth Entrance", 
 * 			"Destard Entrance", "Deceit Entrance"
 *	6/15/04, Pixie
 *		Added "Moongate Houseblockers" region to stop people
 *		from placing right next to a moongate.
 */

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Spells;

namespace Server.Regions
{
	public class NoHousingRegion : Region
	{
		public static void Initialize()
		{
			/* The first parameter is a boolean value:
			 *  - False: this uses 'stupid OSI' house placement checking: part of the house may be placed here provided that the center is not in the region
			 *  -  True: this uses 'smart RunUO' house placement checking: no part of the house may be in the region
			 */
																																 /*
			Region.AddRegion( new NoHousingRegion( false, "", "Britain Graveyard", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Wrong Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Covetous Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Despise Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Shame Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Ice Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Hythloth Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Destard Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Deceit Entrance", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Despise Passage", Map.Felucca ) );
			Region.AddRegion( new NoHousingRegion( false, "", "Jhelom Islands", Map.Felucca ) );

			Region.AddRegion( new NoHousingRegion( true, "", "Moongate Houseblockers", Map.Felucca ) );

			Region.AddRegion( new NoHousingRegion( true, "", "Ocllo Island", Map.Felucca ) );

			Region.AddRegion( new NoHousingRegion( false, "", "Yew Orc Fort Small Area", Map.Felucca ) );
																																	 */
//			Region.AddRegion( new NoHousingRegion( false, "", "Britain Graveyard", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion( false, "", "Wrong Entrance", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion( false, "", "Covetous Entrance", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion( false, "", "Despise Entrance", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion( false, "", "Despise Passage", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion( false, "", "Jhelom Islands", Map.Trammel ) );
//			Region.AddRegion( new NoHousingRegion(  true, "", "Haven Island", Map.Trammel ) );
//
//			Region.AddRegion( new NoHousingRegion( false, "", "Crystal Cave Entrance", Map.Malas ) );
//			Region.AddRegion( new NoHousingRegion(  true, "", "Protected Island", Map.Malas ) );
		}

		private bool m_SmartChecking;

		public bool SmartChecking{ get{ return m_SmartChecking; } }

		public NoHousingRegion( bool smartChecking, string prefix, string name, Map map ) : base( prefix, name, map )
		{
			m_SmartChecking = smartChecking;
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return m_SmartChecking;
		}

		public override void OnEnter( Mobile m )
		{
		}

		public override void OnExit( Mobile m )
		{
		}
	}
}
#endif
