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

/* Scripts\Items\Weapons\SlayerName.cs
 * CHANGELOG
 * 2010.05.22 - Pix
 *      Added utility class SlayerLabel for 'naming' the slayer enums.
 */

using System;

namespace Server.Items
{
	public enum SlayerName
	{
		None,
		Silver,
		OrcSlaying,
		TrollSlaughter,
		OgreTrashing,
		Repond,
		DragonSlaying,
		Terathan,
		SnakesBane,
		LizardmanSlaughter,
		ReptilianDeath,
		DaemonDismissal,
		GargoylesFoe,
		BalronDamnation,
		Exorcism,
		Ophidian,
		SpidersDeath,
		ScorpionsBane,
		ArachnidDoom,
		FlameDousing,
		WaterDissipation,
		Vacuum,
		ElementalHealth,
		EarthShatter,
		BloodDrinking,
		SummerWind,
		ElementalBan // Bane?
	}

	public class SlayerLabel
	{
		public static string GetSlayerLabel(SlayerName name)
		{
			switch (name)
			{
				case SlayerName.None:
					return "";
					break;
				case SlayerName.Silver:
					return "Silver";
					break;
				case SlayerName.OrcSlaying:
					return "Orc Slaying";
					break;
				case SlayerName.TrollSlaughter:
					return "Troll Slaughter";
					break;
				case SlayerName.OgreTrashing:
					return "Ogre Thrashing";
					break;
				case SlayerName.Repond:
					return "Repond";
					break;
				case SlayerName.DragonSlaying:
					return "Dragon Slaying";
					break;
				case SlayerName.Terathan:
					return "Terathan";
					break;
				case SlayerName.SnakesBane:
					return "Snakes Bane";
					break;
				case SlayerName.LizardmanSlaughter:
					return "Lizardman Slaughter";
					break;
				case SlayerName.ReptilianDeath:
					return "Reptillian Death";
					break;
				case SlayerName.DaemonDismissal:
					return "Daemon Dismissal";
					break;
				case SlayerName.GargoylesFoe:
					return "Gargoles' Foe";
					break;
				case SlayerName.BalronDamnation:
					return "Balron Damnation";
					break;
				case SlayerName.Exorcism:
					return "Exorcism";
					break;
				case SlayerName.Ophidian:
					return "Ophidian";
					break;
				case SlayerName.SpidersDeath:
					return "Spiders' Death";
					break;
				case SlayerName.ScorpionsBane:
					return "Scorpions' Bane";
					break;
				case SlayerName.ArachnidDoom:
					return "Arachnid Doom";
					break;
				case SlayerName.FlameDousing:
					return "Flame Dousing";
					break;
				case SlayerName.WaterDissipation:
					return "Water Dissipation";
					break;
				case SlayerName.Vacuum:
					return "Vacuum";
					break;
				case SlayerName.ElementalHealth:
					return "Elemental Health";
					break;
				case SlayerName.EarthShatter:
					return "Earth Shatter";
					break;
				case SlayerName.BloodDrinking:
					return "Blood Drinking";
					break;
				case SlayerName.SummerWind:
					return "Summer Wind";
					break;
				case SlayerName.ElementalBan:
					return "Elemental Bane";
					break;
				default:
					return "unknown slaying";
					break;
			}

		}
	}
}