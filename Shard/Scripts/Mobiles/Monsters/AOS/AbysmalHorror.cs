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

/* Scripts/Mobiles/Monsters/AOS/AbysmalHorror.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
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
	[CorpseName("an abyssmal horror corpse")]
	public class AbysmalHorror : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.MortalStrike;
		}

		[Constructable]
		public AbysmalHorror()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "an abyssmal horror";
			Body = 312;
			BaseSoundID = 0x451;
			BardImmune = true;

			SetStr(401, 420);
			SetDex(81, 90);
			SetInt(401, 420);

			SetHits(6000);

			SetDamage(13, 17);

			SetSkill(SkillName.EvalInt, 200.0);
			SetSkill(SkillName.Magery, 112.6, 117.5);
			SetSkill(SkillName.Meditation, 200.0);
			SetSkill(SkillName.MagicResist, 117.6, 120.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 84.1, 88.0);

			Fame = 26000;
			Karma = -26000;

			VirtualArmor = 54;
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (!Summoned && !NoKillAwards && DemonKnight.CheckArtifactChance(this))
				DemonKnight.DistributeArtifact(this);
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public AbysmalHorror(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(2000, 2500);
			PackMagicEquipment(2, 3, 1.0, 1.0);
			PackMagicEquipment(2, 3, 0.50, 0.50);
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

			if (BaseSoundID == 357)
				BaseSoundID = 0x451;
		}
	}
}
