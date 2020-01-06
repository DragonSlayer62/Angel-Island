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

/* Scripts/Mobiles/Monsters/AOS/WailingBanshee.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a wailing banshee corpse")]
	public class WailingBanshee : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		[Constructable]
		public WailingBanshee()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a wailing banshee";
			Body = 310;
			BaseSoundID = 0x482;

			SetStr(126, 150);
			SetDex(76, 100);
			SetInt(86, 110);

			SetHits(76, 90);

			SetDamage(10, 14);

			SetSkill(SkillName.MagicResist, 70.1, 95.0);
			SetSkill(SkillName.Tactics, 45.1, 70.0);
			SetSkill(SkillName.Wrestling, 50.1, 70.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 19;
		}

		public WailingBanshee(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGold(100, 250);
			// Category 2 MID
			PackMagicItem(1, 1, 0.05);
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
