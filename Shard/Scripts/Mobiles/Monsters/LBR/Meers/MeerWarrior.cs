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

/* Scripts/Mobiles/Monsters/LBR/Meers/MeerWarrior.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Misc;
using Server.Items;
using Server.Spells;

namespace Server.Mobiles
{
	[CorpseName("a meer corpse")]
	public class MeerWarrior : BaseCreature
	{
		[Constructable]
		public MeerWarrior()
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			Name = "a meer warrior";
			Body = 771;
			BardImmune = true;

			SetStr(86, 100);
			SetDex(186, 200);
			SetInt(86, 100);

			SetHits(52, 60);

			SetDamage(12, 19);

			SetSkill(SkillName.MagicResist, 91.0, 100.0);
			SetSkill(SkillName.Tactics, 91.0, 100.0);
			SetSkill(SkillName.Wrestling, 91.0, 100.0);

			VirtualArmor = 22;

			Fame = 2000;
			Karma = 5000;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override bool InitialInnocent { get { return true; } }

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && !willKill && amount > 3 && from != null && !InRange(from, 7))
			{
				this.MovingEffect(from, 0xF51, 10, 0, false, false);
				SpellHelper.Damage(TimeSpan.FromSeconds(1.0), from, this, Utility.RandomMinMax(30, 40) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);
			}

			base.OnDamage(amount, from, willKill);
		}

		public override int GetHurtSound()
		{
			return 0x156;
		}

		public override int GetDeathSound()
		{
			return 0x15C;
		}

		public MeerWarrior(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// no LBR
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					// TODO: standard runuo
				}
			}
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
