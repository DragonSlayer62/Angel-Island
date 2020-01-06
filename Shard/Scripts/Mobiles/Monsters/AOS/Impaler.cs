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

/* Scripts/Mobiles/Monsters/AOS/Impaler.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
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
	[CorpseName("an impaler corpse")]
	public class Impaler : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return Utility.RandomBool() ? WeaponAbility.MortalStrike : WeaponAbility.BleedAttack;
		}

		[Constructable]
		public Impaler()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = NameList.RandomName("impaler");
			Body = 306;
			BaseSoundID = 0x2A7;
			BardImmune = true;

			SetStr(190);
			SetDex(45);
			SetInt(190);

			SetHits(5000);

			SetDamage(31, 35);

			SetSkill(SkillName.Meditation, 120.0);
			SetSkill(SkillName.Poisoning, 160.0);
			SetSkill(SkillName.MagicResist, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 80.0);

			Fame = 24000;
			Karma = -24000;

			VirtualArmor = 49;
		}

		public override void OnDeath(Container c)
		{
			base.OnDeath(c);

			if (!Summoned && !NoKillAwards && DemonKnight.CheckArtifactChance(this))
				DemonKnight.DistributeArtifact(this);
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override Poison HitPoison { get { return (0.8 >= Utility.RandomDouble() ? Poison.Greater : Poison.Deadly); } }

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public Impaler(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(1800, 2500);
			PackMagicEquipment(2, 3, 0.90, 0.90);
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

			if (BaseSoundID == 1200)
				BaseSoundID = 0x2A7;
		}
	}
}
