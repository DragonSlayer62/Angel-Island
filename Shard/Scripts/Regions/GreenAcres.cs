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

/* Scripts/Regions/GreenAcres.cs
 * CHANGELOG
 *	04/24/09, plasma
 *		Commented out all regions, replaced with DRDT
 *  5/1/07, Adam
 *      Allow house placement by players if TestCenter.Enabled == true
 */

using System;
using Server;
using Server.Mobiles;
using Server.Spells;
using Server.Spells.Seventh;
using Server.Spells.Fourth;
using Server.Spells.Sixth;
using Server.Misc;                      // test center

namespace Server.Regions
{
	public class GreenAcres : Region
	{
		public static void Initialize()
		{
			//Region.AddRegion( new GreenAcres( Map.Felucca ) );
			// Region.AddRegion( new GreenAcres( Map.Trammel ) );
		}

		public GreenAcres(Map map)
			: base("", "Green Acres", map)
		{
		}

		public override bool AllowHousing(Mobile from, Point3D p)
		{
			// we allow players to place houses on Green Acres on Test Center
			if (from.AccessLevel == AccessLevel.Player && TestCenter.Enabled == false)
				return false;
			else
				return true;
		}

		public override void OnEnter(Mobile m)
		{
		}

		public override void OnExit(Mobile m)
		{
		}

		public override bool OnBeginSpellCast(Mobile m, ISpell s)
		{
			if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell) && m.AccessLevel == AccessLevel.Player)
			{
				m.SendMessage("You cannot cast that spell here.");
				return false;
			}
			else
			{
				return base.OnBeginSpellCast(m, s);
			}
		}
	}
}
#endif