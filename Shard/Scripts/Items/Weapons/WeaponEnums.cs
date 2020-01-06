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

using System;

namespace Server.Items
{
	public enum WeaponQuality
	{
		Low,
		Regular,
		Exceptional
	}

	public enum WeaponType
	{
		Axe,		// Axes, Hatches, etc. These can give concussion blows
		Slashing,	// Katana, Broadsword, Longsword, etc. Slashing weapons are poisonable
		Staff,		// Staves
		Bashing,	// War Hammers, Maces, Mauls, etc. Two-handed bashing delivers crushing blows
		Piercing,	// Spears, Warforks, Daggers, etc. Two-handed piercing delivers paralyzing blows
		Polearm,	// Halberd, Bardiche
		Ranged,		// Bow, Crossbows
		Fists		// Fists
	}

	public enum WeaponDamageLevel
	{
		Regular,
		Ruin,
		Might,
		Force,
		Power,
		Vanquishing
	}

	public enum WeaponAccuracyLevel
	{
		Regular,
		Accurate,
		Surpassingly,
		Eminently,
		Exceedingly,
		Supremely
	}

	public enum WeaponDurabilityLevel
	{
		Regular,
		Durable,
		Substantial,
		Massive,
		Fortified,
		Indestructible
	}

	public enum WeaponAnimation
	{
		Slash1H = 9,
		Pierce1H = 10,
		Bash1H = 11,
		Bash2H = 12,
		Slash2H = 13,
		Pierce2H = 14,
		ShootBow = 18,
		ShootXBow = 19,
		Wrestle = 31
	}
}