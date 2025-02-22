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

/* Scripts/Mobiles/Monsters/Reptile/Melee/Drake.cs
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
	[CorpseName("a drake corpse")]
	public class Drake : BaseCreature
	{
		[Constructable]
		public Drake()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a drake";
			Body = Utility.RandomList(60, 61);
			BaseSoundID = 362;

			SetStr(401, 430);
			SetDex(133, 152);
			SetInt(101, 140);

			SetHits(241, 258);

			SetDamage(11, 17);

			SetSkill(SkillName.MagicResist, 65.1, 80.0);
			SetSkill(SkillName.Tactics, 65.1, 90.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 46;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 84.3;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 0; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return (Core.UOAI || Core.UOAR || Core.PublishDate < Core.PlagueOfDespair) ? 0 : 2; } }
		public override ScaleType ScaleType { get { return (Body == 60 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat | FoodType.Fish; } }

		public Drake(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGem();

				PackScroll(1, 6);
				PackScroll(1, 6);

				PackReg(3);
				PackGold(180, 220);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020214223637/uo.stratics.com/hunters/drake.shtml
					// 100 to 250 Gold, Gems, Garlic, Scrolls, Magic Weapons, 10 Raw Ribs (carved), 20 Hides (carved)

					if (Spawning)
					{
						PackGold(100, 250);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);
						PackItem(new Garlic(Utility.Random(1, 2)));
						PackScroll(1, 6);
						PackScroll(1, 6);
						PackMagicEquipment(1, 2, 0.00, 0.25);	// no chance at armor, 25% (12.5% actual) at low end magic weapon
					}
				}
				else
				{
					// standard runuo
					if (Spawning)
						PackReg(3);

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.MedScrolls, 2);
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
