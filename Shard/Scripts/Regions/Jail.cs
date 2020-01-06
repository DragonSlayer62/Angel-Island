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

/* Scripts/Regions/Jail.cs
 * CHANGELOG:
 *  *	04/24/09, plasma
 *		Commented out all regions, replaced with DRDT
 *	3/6/05: Pixie
 *		Addition of Jail.Special stuff
 */

using System;
using System.Collections;
using System.IO;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Commands;

namespace Server.Regions
{
	public class Jail : Region
	{
		private static string[] SpecialNames;

		public static void Initialize()
		{
			//Region.AddRegion( new Jail( Map.Felucca ) );
			// Region.AddRegion( new Jail( Map.Trammel ) );
			
			SpecialNames = new string[0];
			//load special
			ArrayList special = new ArrayList();

			try 
			{
				using (StreamReader sr = new StreamReader("Data\\Special.txt")) 
				{
					String line;
					while ((line = sr.ReadLine()) != null) 
					{
						if( line != null && line.Length > 0 )
						{
							special.Add(line);
						}
					}
				}
			}
			catch (Exception e) 
			{
				LogHelper.LogException(e);
				Console.WriteLine("The Data\\Special.txt file could not be read:");
				Console.WriteLine(e.Message);
			}
			
			SpecialNames = (string[])special.ToArray( typeof(string) );
		}

		public static bool IsInSpecial( string name )
		{
			try
			{
				if( SpecialNames != null )
				{
					for( int i=0; i<SpecialNames.Length; i++ )
					{
						if( SpecialNames[i] != null )
						{
							if( name.ToLower() == SpecialNames[i].ToLower() )
							{
								return true;
							}
						}
					}
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			return false;
		}

		public Jail( Map map ) : base( "", "Jail", map )
		{
		}

		public override bool AllowBenificial( Mobile from, Mobile target )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendMessage( "You may not do that in jail." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool AllowHarmful( Mobile from, Mobile target )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendMessage( "You may not do that in jail." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

		public override void AlterLightLevel( Mobile m, ref int global, ref int personal )
		{
			global = LightCycle.JailLevel;
		}

		public override bool OnBeginSpellCast( Mobile from, ISpell s )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendLocalizedMessage( 502629 ); // You cannot cast spells here.

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool OnSkillUse( Mobile from, int Skill )
		{
			if ( from.AccessLevel == AccessLevel.Player )
				from.SendMessage( "You may not use skills in jail." );

			return ( from.AccessLevel > AccessLevel.Player );
		}

		public override bool OnCombatantChange( Mobile from, Mobile Old, Mobile New )
		{
			return ( from.AccessLevel > AccessLevel.Player );
		}

	}
}
#endif
