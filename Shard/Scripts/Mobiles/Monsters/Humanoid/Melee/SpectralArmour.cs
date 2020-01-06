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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/SpectralArmor.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{

	public class SpectralArmour : BaseCreature
	{
		[Constructable]
		public SpectralArmour()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Body = 637;
			Hue = 32;
			Name = "spectral armour";
			BaseSoundID = 451;

			SetStr(309, 333);
			SetDex(99, 106);
			SetInt(101, 110);
			SetSkill(SkillName.Wrestling, 78.1, 95.5);
			SetSkill(SkillName.Tactics, 91.1, 99.7);
			SetSkill(SkillName.MagicResist, 92.4, 79);
			SetSkill(SkillName.Swords, 78.1, 97.4);

			VirtualArmor = 40;
			SetFameLevel(3);
			SetKarmaLevel(3);
		}

		public override Poison PoisonImmune { get { return Poison.Regular; } }

		[CommandProperty(AccessLevel.GameMaster)]
		public override int HitsMax { get { return 323; } }

		public SpectralArmour(Serial serial)
			: base(serial)
		{
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

		public override void GenerateLoot()
		{
			// no corpse, so see OnBeforeDeath()
		}

		public override bool OnBeforeDeath()
		{
			if (!base.OnBeforeDeath())
				return false;

			if (Core.UOAI || Core.UOAR)
			{
				Scimitar weapon = new Scimitar();
				weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(0, 5);
				weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(0, 5);
				weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(0, 5);
				weapon.MoveToWorld(this.Location, this.Map);

				Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{
					Scimitar weapon = new Scimitar();
					weapon.DamageLevel = (WeaponDamageLevel)Utility.Random(0, 5);
					weapon.DurabilityLevel = (WeaponDurabilityLevel)Utility.Random(0, 5);
					weapon.AccuracyLevel = (WeaponAccuracyLevel)Utility.Random(0, 5);
					weapon.MoveToWorld(this.Location, this.Map);

					Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
				}
				else
				{
					// run uo code 
					Gold gold = new Gold(Utility.RandomMinMax(240, 375));
					gold.MoveToWorld(Location, Map);

					Effects.SendLocationEffect(Location, Map, 0x376A, 10, 1);
				}
			}

			// we don't want a corpse, so refuse the 'death' and just delete the creature
			this.Delete();
			return false;
		}
	}
}