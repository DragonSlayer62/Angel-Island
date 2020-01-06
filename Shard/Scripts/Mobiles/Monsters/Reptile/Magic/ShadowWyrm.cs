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

/* Scripts/Mobiles/Monsters/Reptile/Magic/ShadowWyrm.cs
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
	[CorpseName("a shadow wyrm corpse")]
	public class ShadowWyrm : BaseCreature
	{
		[Constructable]
		public ShadowWyrm()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a shadow wyrm";
			Body = 106;
			BaseSoundID = 362;

			SetStr(898, 1030);
			SetDex(68, 200);
			SetInt(488, 620);

			SetHits(558, 599);

			SetDamage(29, 35);

			SetSkill(SkillName.EvalInt, 80.1, 100.0);
			SetSkill(SkillName.Magery, 80.1, 100.0);
			SetSkill(SkillName.Meditation, 52.5, 75.0);
			SetSkill(SkillName.MagicResist, 100.3, 130.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 97.6, 100.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 70;
		}

		public override int GetIdleSound()
		{
			return 0x2D5;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		// Auto-dispel is UOR - http://forums.uosecondage.com/viewtopic.php?f=8&t=6901
		public override bool AutoDispel { get { return Core.UOAI || Core.UOAR ? false : Core.PublishDate >= Core.EraREN ? true : false; } }
		public override Poison PoisonImmune { get { return Poison.Deadly; } }
		public override Poison HitPoison { get { return Poison.Deadly; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 5; } }

		public override int Meat { get { return Core.UOAI || Core.UOAR ? 20 : 19; } }
		public override int Hides { get { return Core.UOAI || Core.UOAR ? 40 : 20; } }
		public override int Scales { get { return (Core.UOAI || Core.UOAR || Core.PublishDate < Core.PlagueOfDespair) ? 0 : 10; } }
		public override ScaleType ScaleType { get { return ScaleType.Black; } }
		public override HideType HideType { get { return HideType.Barbed; } }

		public ShadowWyrm(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				for (int i = 0; i < 5; ++i)
					PackGem();

				PackGold(850, 1100);
				PackMagicEquipment(1, 3, 0.50, 0.50);
				PackMagicEquipment(1, 3, 0.20, 0.20);
				PackScroll(6, 8);
				PackScroll(6, 8);
				// Category 5 MID
				PackMagicItem(3, 3, 0.20);
				PackMagicItem(3, 3, 0.10);
				PackMagicItem(3, 3, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20021014225931/uo.stratics.com/hunters/shadowwyrm.shtml
					// 1800 to 2400 Gold, Gems, Magic items, 10 Black Scales, 19 Raw Ribs (carved), 20 Barbed Hides (carved), Level 5 treasure maps

					if (Spawning)
					{
						PackGold(1800, 2400);
					}
					else
					{
						PackGem(Utility.Random(4, 6), .9);	// TODO: no idea as to the level and rate
						PackGem(Utility.Random(4, 6), .5);

						PackMagicEquipment(1, 3);			// TODO: no idea as to the level and rate
						PackMagicEquipment(1, 3);
						PackMagicItem(3, 3);
						PackMagicItem(3, 3, 0.10);
					}
				}
				else
				{
					AddLoot(LootPack.FilthyRich, 3);
					AddLoot(LootPack.Gems, 5);
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
