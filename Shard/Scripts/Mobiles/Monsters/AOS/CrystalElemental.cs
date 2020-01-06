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

/* Scripts/Mobiles/Monsters/AOS/CrystalElemental.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a crystal elemental corpse")]
	public class CrystalElemental : BaseCreature
	{
		public override WeaponAbility GetWeaponAbility()
		{
			return WeaponAbility.BleedAttack;
		}

		[Constructable]
		public CrystalElemental()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a crystal elemental";
			Body = 300;
			BaseSoundID = 278;

			SetStr(136, 160);
			SetDex(51, 65);
			SetInt(86, 110);

			SetHits(150);

			SetDamage(10, 15);

			SetSkill(SkillName.EvalInt, 70.1, 75.0);
			SetSkill(SkillName.Magery, 70.1, 75.0);
			SetSkill(SkillName.Meditation, 65.1, 75.0);
			SetSkill(SkillName.MagicResist, 80.1, 90.0);
			SetSkill(SkillName.Tactics, 75.1, 85.0);
			SetSkill(SkillName.Wrestling, 65.1, 75.0);

			Fame = 6500;
			Karma = -6500;

			VirtualArmor = 54;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }

		public CrystalElemental(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(200, 350);
			PackItem(new IronOre(3));
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
