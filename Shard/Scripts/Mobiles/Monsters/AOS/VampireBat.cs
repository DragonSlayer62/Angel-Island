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

/* Scripts/Mobiles/Monsters/AOS/VampireBat.cs
 * ChangeLog
 *	6/28/08, Adam
 *		if a silver weapon, do 150% damage
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  9/21/04, Jade
 *      Increased gold drop from (20, 60) to (100, 150)
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a vampire bat corpse")]
	public class VampireBat : BaseCreature
	{
		[Constructable]
		public VampireBat()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a vampire bat";
			Body = 317;
			BaseSoundID = 0x270;

			SetStr(91, 110);
			SetDex(91, 115);
			SetInt(26, 50);

			SetHits(55, 66);

			SetDamage(7, 9);

			SetSkill(SkillName.MagicResist, 70.1, 95.0);
			SetSkill(SkillName.Tactics, 55.1, 80.0);
			SetSkill(SkillName.Wrestling, 30.1, 55.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 14;
		}

		public override int GetIdleSound()
		{
			return 0x29B;
		}

		public VampireBat(Serial serial)
			: base(serial)
		{
		}

		// just add the vampirebat to the table of undead
		/*public override void CheckWeaponImmunity(BaseWeapon wep, int damagein, out int damage)
		{

			// if a silver weapon, do 150% damage
			if (wep.Slayer == SlayerName.Silver || wep.HolyBlade == true)
				damage = (int)(damagein * 1.5);
			// otherwise do only 25% damage
			else
				damage = (int)(damagein * .25);
		}*/

		public override void GenerateLoot()
		{
			PackGold(100, 150);
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
