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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/ArticOgreLord.cs
 * ChangeLog
 *	4/12/09, Adam
 *		Update special armor drop to not use SDrop system
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 * 	4/11/05, Adam
 *		Update to use new version of Loot.ImbueWeaponOrArmor()
 *	3/28/05, Adam
 *		Use weighted table selection code for weapon/armor attr in Loot.cs
 *	3/21/05, Adam
 *		Cleaned up weighted table selection code for weapon/armor attr
 *	9/14/04, Pigpen
 *		Remove Treasure map from loot.
 *	9/11/04, Adam
 *		Replace lvl 3 Treasure Map with lvl 5
 *		Change helm drop to 2%
 *  9/10/04, Pigpen
 *  	add Armor type of Arctic Storm to Random Drop
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
	[CorpseName("a frozen ogre lord's corpse")]
	[TypeAlias("Server.Mobiles.ArticOgreLord")]
	public class ArcticOgreLord : BaseCreature
	{
		[Constructable]
		public ArcticOgreLord()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "an arctic ogre lord";
			Body = 135;
			BaseSoundID = 427;

			SetStr(1100, 1200);
			SetDex(66, 75);
			SetInt(46, 70);

			SetHits(1100, 1200);

			SetDamage(20, 25);

			SetSkill(SkillName.MagicResist, 125.1, 140.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;
		}

		public override int Meat { get { return 2; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override AuraType MyAura { get { return AuraType.Ice; } }
		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }

		public ArcticOgreLord(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(2500, 3500);
				PackItem(new Club());
				//PackMagicEquipment( 1, 3, 0.30, 0.30 );

				// adam: add 25% chance to get a Random Slayer Instrument
				PackSlayerInstrument(.25);

				// Pigpen: Add the Arctic Storm (rare) armor to this mini-boss.
				if (Utility.RandomDouble() < 0.10)
				{
					switch (Utility.Random(7))
					{
						case 0: PackItem(new ArcticStormArmor(), false); break;	// female chest
						case 1: PackItem(new ArcticStormArms(), false); break;	// arms
						case 2: PackItem(new ArcticStormTunic(), false); break;	// male chest
						case 3: PackItem(new ArcticStormGloves(), false); break;	// gloves
						case 4: PackItem(new ArcticStormGorget(), false); break;	// gorget
						case 5: PackItem(new ArcticStormLeggings(), false); break;// legs
						case 6: PackItem(new ArcticStormHelm(), false); break;	// helm
					}
				}


				// Use our unevenly weighted table for chance resolution
				Item item;
				item = Loot.RandomArmorOrShieldOrWeapon();
				PackItem(Loot.ImbueWeaponOrArmor(item, 6, 0, false));
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202092201/uo.stratics.com/hunters/arcticogrelord.shtml
					// 500 - 700 Gold, Magic Items, a normal club and a gem
					if (Spawning)
					{
						PackGold(500, 700);
					}
					else
					{
						if (Utility.RandomBool())
							PackMagicEquipment(1, 3);
						else
							PackMagicItem(2, 3, 0.10);

						PackItem(new Club());
						PackGem();
					}
				}
				else
				{
					if (Spawning)
						PackItem(new Club());

					AddLoot(LootPack.FilthyRich);
					AddLoot(LootPack.Rich);
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
