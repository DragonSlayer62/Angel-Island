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

/* Scripts/Mobiles/Monsters/Misc/Melee/BladeSpirits.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 * 4/27/05, Kit
 *	Adjusted dispell difficulty
 *  4/27/05, Kit
 *	Adapted to use new ev/bs logic
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a blade spirit corpse")]
	public class BladeSpirits : BaseCreature
	{
		public override bool DeleteCorpseOnDeath { get { return Core.AOS; } }
		public override bool IsHouseSummonable { get { return true; } }

		public override double DispelDifficulty { get { return Core.UOAI || Core.UOAR ? 56.0 : 0; } }
		public override double DispelFocus { get { return Core.UOAI || Core.UOAR ? 45.0 : 20.0; } }

		[Constructable]
		public BladeSpirits()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest | FightMode.Dex, 10, 1, 0.2, 0.4)
		{
			Name = "a blade spirit";
			Body = 574;

			SetStr(150);
			SetDex(150);
			SetInt(100);

			SetHits(80);
			SetStam(250);
			SetMana(0);

			SetDamage(10, 14);

			SetSkill(SkillName.MagicResist, 70.0);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.Wrestling, 90.0);

			Fame = 0;
			Karma = 0;

			VirtualArmor = 40;
			ControlSlots = 1;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public override int GetAngerSound()
		{
			return 0x23A;
		}

		public override int GetAttackSound()
		{
			return 0x3B8;
		}

		public override int GetHurtSound()
		{
			return 0x23A;
		}

		public BladeSpirits(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
