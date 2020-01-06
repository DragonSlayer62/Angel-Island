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

/* Scripts/Regions/Felucca/Dungeon.cs
 * ChangeLog
 *	04/24/09, plasma
 *		Commented out all regions, replaced with DRDT
 *	9/21/05, Adam
 *		Remove Wind and Deceit as they are controlled by DRDT
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	5/11/04, mith
 *		Moved Wind from Town.cs to Dungeon.cs, this removes guards and prevents recall, mark, and gate.
 */

using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;

namespace Server.Regions
{
	public class FeluccaDungeon : Region
	{
		public static void Initialize()
		{
			/*
			Region.AddRegion( new FeluccaDungeon( "Covetous" ) );
			Region.AddRegion( new FeluccaDungeon( "Despise" ) );
			Region.AddRegion( new FeluccaDungeon( "Destard" ) );
			Region.AddRegion( new FeluccaDungeon( "Hythloth" ) );
			Region.AddRegion( new FeluccaDungeon( "Shame" ) );
			Region.AddRegion( new FeluccaDungeon( "Wrong" ) );
			Region.AddRegion( new FeluccaDungeon( "Terathan Keep" ) );
			Region.AddRegion( new FeluccaDungeon( "Fire" ) );
			Region.AddRegion( new FeluccaDungeon( "Ice" ) );
			Region.AddRegion( new FeluccaDungeon( "Orc Cave" ) );
			*/
			// Controlled by DRDT
			//Region.AddRegion( new FeluccaDungeon( "Wind" ) );
			//Region.AddRegion( new FeluccaDungeon( "Deceit" ) );
		}

		public FeluccaDungeon( string name ) : base( "the dungeon", name, Map.Felucca )
		{
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void OnEnter( Mobile m )
		{
			//base.OnEnter( m ); // You have entered the dungeon {0}
		}

		public override void OnExit( Mobile m )
		{
			//base.OnExit( m );
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = LightCycle.DungeonLevel;
		}

		/*RunUO 1.0RC0 had this commented out*/
		/**/public override bool OnBeginSpellCast( Mobile m, ISpell s )
		{
			if ( s is GateTravelSpell || s is RecallSpell || s is MarkSpell )
			{
				m.SendMessage( "You cannot cast that spell here." );
				return false;
			}
			else
			{
				return base.OnBeginSpellCast( m, s );
			}
		}/**/
	}
}
#endif