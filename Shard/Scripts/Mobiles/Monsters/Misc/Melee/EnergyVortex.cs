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

/* Scripts/Mobiles/Monsters/Misc/Melee/Energyvortex.cs
 * ChangeLog
 *	2/9/11, adam
 *		Add Llama vortices
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  4/27/05, Kit
 *		Adjusted dispell difficulty
 *  4/27/05, Kit
 *		Adapted to use new ev/bs logic
 *  7,17,04, Old Salty
 * 		Changed ActiveSpeed to make EV's a little slower.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	5/7/04, mith
 *		Increased Damage from 14-17 to 25-30.
 */

using System;
using Server;
using Server.Items;
using System.Collections;

namespace Server.Mobiles
{
	[CorpseName("an energy vortex corpse")]
	public class EnergyVortex : BaseCreature
	{
		public override bool DeleteCorpseOnDeath { get { return Summoned; } }
		public override bool AlwaysMurderer { get { return true; } } // Or Llama vortices will appear gray.

		public override double DispelDifficulty { get { return Core.UOAI || Core.UOAR ? 56.0 : 80.0; } }
		public override double DispelFocus { get { return Core.UOAI || Core.UOAR ? 45.0 : 20; } }


		[Constructable]
		public EnergyVortex()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest | FightMode.Int, 6, 1, 0.199, 0.350)
		{
			Name = "an energy vortex";
			if (0.02 > Utility.RandomDouble()) // Tested on OSI, but is this right? Who knows.
			{
				// Llama vortex!
				Body = 0xDC;
				Hue = 0x76;
			}
			else
			{
				Body = 164;
			}

			SetStr(200);
			SetDex(200);
			SetInt(100);

			SetHits(70);
			SetStam(250);
			SetMana(0);

			if (Core.UOAI || Core.UOAR)
				SetDamage(25, 30);
			else
				SetDamage(14, 17);

			SetSkill(SkillName.MagicResist, 99.9);
			SetSkill(SkillName.Tactics, 90.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 0;
			Karma = 0;

			VirtualArmor = 40;
			ControlSlots = 1;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool SpeedOverrideOK { get { return true; } }

		public override int GetAngerSound()
		{
			return 0x15;
		}

		public override int GetAttackSound()
		{
			return 0x28;
		}

		public EnergyVortex(Serial serial)
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

			if (BaseSoundID == 263)
				BaseSoundID = 0;
		}
	}
}
