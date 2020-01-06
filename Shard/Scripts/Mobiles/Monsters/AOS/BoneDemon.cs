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

/* Scripts/Mobiles/Monsters/AOS/BoneDemon.cs
 * ChangeLog
 *	4/12/09, Adam
 *		Update special armor drop to not use SDrop system
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 * 	4/11/05, Adam
 *		Update to use new version of Loot.ImbueWeaponOrArmor()
 *	3/28/05, Adam
 *		Use weighted table selection code for weapon/armor attr in Loot.cs
 *	3/21/05, Adam
 *		Cleaned up weighted table selection code for weapon/armor attr
 *	9/14/04, Pigpen
 *		Remove treasure map from Loot.
 *	9/11/04, Adam
 *		Add TreasureMapLevel 5
 *		UnholyBoneTunic ==> UnholyBoneArmor
 *  9/11/04, Pigpen
 *		add Armor type of Unholy Bone to random drop.
 *		add Weighted system of high end loot. with 5% chance of slayer on wep drops.
 *		Changed gold drop to 2500-3500gp 
 *  7/24/04, Adam
 *		add 25% chance to get a Random Slayer Instrument
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a bone demon corpse")]
	public class BoneDemon : BaseCreature
	{
		[Constructable]
		public BoneDemon()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a bone demon";
			Body = 308;
			BaseSoundID = 0x48D;
			BardImmune = true;

			SetStr(1000);
			SetDex(151, 175);
			SetInt(171, 220);

			SetHits(2200);

			SetDamage(34, 36);

			SetSkill(SkillName.EvalInt, 77.6, 87.5);
			SetSkill(SkillName.Magery, 77.6, 87.5);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.MagicResist, 50.1, 75.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 44;
		}

		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public BoneDemon(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			PackGem();
			PackGold(2500, 3500);
			//PackMagicEquipment( 1, 3, 0.80, 0.80 );
			//PackMagicEquipment( 1, 3, 0.20, 0.20 );
			PackItem(new Bone(100));

			// adam: add 25% chance to get a Random Slayer Instrument
			PackSlayerInstrument(.25);

			// Pigpen: Add the Unholy Bone (rare) armor to this mini-boss.
			if (Utility.RandomDouble() < 0.10)
			{
				switch (Utility.Random(5))
				{
					case 0: PackItem(new UnholyBoneLegs(), false); break;	// Leggings
					case 1: PackItem(new UnholyBoneArms(), false); break;	// arms
					case 2: PackItem(new UnholyBoneArmor(), false); break;	// Chest
					case 3: PackItem(new UnholyBoneGloves(), false); break;	// gloves
					case 4: PackItem(new UnholyBoneHelm(), false); break;	// helm
				}
			}

			// Use our unevenly weighted table for chance resolution
			Item item;
			item = Loot.RandomArmorOrShieldOrWeapon();
			PackItem(Loot.ImbueWeaponOrArmor(item, 6, 0, false));
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
