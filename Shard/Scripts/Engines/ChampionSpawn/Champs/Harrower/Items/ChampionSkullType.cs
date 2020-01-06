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


/* Scripts\Engines\ChampionSpawn\Champs\Harrower\Items\ChampionSkullType.cs
 * ChangeLog
 *  03/09/07, plasma    
 *      Removed cannedevil namespace reference
 *  1/11/07, Adam
 *      Add the 'None' type for the special champs
 *  01/05/07, plasma!
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 */
using System;
using Server;

namespace Server.Engines.ChampionSpawn
{
	public enum ChampionSkullType
	{
		Power,
		Enlightenment,
		Venom,
		Pain,
		Greed,
		Death,
		None        // special non-champ champs 
	}
}