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

/* Scripts/Mobiles/Monsters/AOS/MoundOfMaggots.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 4 lines removed.
 *  9/21/04, Jade
 *      Increased gold drop from (50, 250) to (150, 300)
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a maggoty corpse")] // TODO: Corpse name?
	public class MoundOfMaggots : BaseCreature
	{
		[Constructable]
		public MoundOfMaggots()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a mound of maggots";
			Body = 319;
			BaseSoundID = 898;

			SetStr(61, 70);
			SetDex(61, 70);
			SetInt(10);

			SetMana(0);

			SetDamage(3, 9);

			SetSkill(SkillName.Tactics, 50.0);
			SetSkill(SkillName.Wrestling, 50.1, 60.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 24;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public MoundOfMaggots(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(150, 300);
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
