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

/* Scripts/Mobiles/Monsters/AOS/TreeFellow.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 5 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a treefellow corpse")]
	public class Treefellow : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.Dismount;
		}

		[Constructable]
		public Treefellow()
			: base(AIType.AI_Melee, FightMode.Aggressor | FightMode.Evil, 10, 1, 0.25, 0.5)
		{
			Name = "a treefellow";
			Body = 301;

			SetStr(196, 220);
			SetDex(31, 55);
			SetInt(66, 90);

			SetHits(118, 132);

			SetDamage(12, 16);

			SetSkill(SkillName.MagicResist, 40.1, 55.0);
			SetSkill(SkillName.Tactics, 65.1, 90.0);
			SetSkill(SkillName.Wrestling, 65.1, 85.0);

			Fame = 500;
			Karma = 1500;

			VirtualArmor = 24;
		}

		public override int GetIdleSound()
		{
			return 443;
		}

		public override int GetDeathSound()
		{
			return 31;
		}

		public override int GetAttackSound()
		{
			return 672;
		}

		public Treefellow(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGold(200, 350);

			if (0.25 > Utility.RandomDouble())
				PackItem(new Board(25));
			else
				PackItem(new Log(25));
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
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

			if (BaseSoundID == 442)
				BaseSoundID = -1;
		}
	}
}
