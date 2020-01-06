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

/* Scripts/Mobiles/Animals/Mounts/Nightmare.cs
 * ChangeLog
 *	4/10/10, adam
 *		Add speed management MCi to tune dragon speeds.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a nightmare corpse")]
	public class Nightmare : BaseMount
	{
		[Constructable]
		public Nightmare()
			: this("a nightmare")
		{
		}

		private static double MareActiveSpeed { get { return Server.Items.Consoles.DragonSpeedMCi.MareActiveSpeed; } }
		private static double MarePassiveSpeed { get { return Server.Items.Consoles.DragonSpeedMCi.MarePassiveSpeed; } }

		[Constructable]
		public Nightmare(string name)
			: base(name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, MareActiveSpeed, MarePassiveSpeed)
		{
			BaseSoundID = 0xA8;

			SetStr(496, 525);
			SetDex(86, 105);
			SetInt(86, 125);

			SetHits(298, 315);

			SetDamage(16, 22);

			SetSkill(SkillName.EvalInt, 10.4, 50.0);
			SetSkill(SkillName.Magery, 10.4, 50.0);
			SetSkill(SkillName.MagicResist, 85.3, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 80.5, 92.5);

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 60;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 95.1;

			switch (Utility.Random(3))
			{
				case 0:
					{
						BodyValue = 116;
						ItemID = 16039;
						break;
					}
				case 1:
					{
						BodyValue = 178;
						ItemID = 16041;
						break;
					}
				case 2:
					{
						BodyValue = 179;
						ItemID = 16055;
						break;
					}
			}
		}

		public override int GetAngerSound()
		{
			if (!Controlled)
				return 0x16A;

			return base.GetAngerSound();
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Meat { get { return 5; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public Nightmare(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGold(250, 350);
				PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
				PackScroll(1, 5);
				PackPotion();
				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// note we had to go all the way to june 2002 to get the loot
					// http://web.archive.org/web/20020607065005/uo.stratics.com/hunters/nightmare.shtml
					//	250-350 gold, 3-5 Sulphurous Ash, Gems, Scroll, Potion, Arrows

					if (Spawning)
					{
						PackGold(250, 350);
					}

					PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));
					PackGem(1, .9);
					PackGem(1, .05);
					PackScroll(1, 7);
					PackPotion();
					PackItem(new Arrow(Utility.RandomMinMax(3, 5)));
				}
				else
				{
					if (Spawning)
						PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average);
					AddLoot(LootPack.LowScrolls);
					AddLoot(LootPack.Potions);
				}
			}
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

			if (BaseSoundID == 0x16A)
				BaseSoundID = 0xA8;
		}
	}
}
