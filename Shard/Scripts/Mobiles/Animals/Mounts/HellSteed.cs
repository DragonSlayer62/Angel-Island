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

/* Scripts/Mobiles/Animals/Mounts/HellSteed.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	12/12/04, Adam
 *		Remove IOBAlignment - tamables should not have alignment
 *	11/19/04
 *		1. AIType.AI_Melee
 *		2. Night mare loot and resources
 *		3. Add firebreath
 *		a. Base on Drake with bump in damage (SetDamage)
 *	11/19/04, Adam
 *		Make tamable: 98 skill, 2 control slots
 *	11/17/04, Adam
 *		Set the IOBAlignment to Undead
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 */

using System;
using Server.Mobiles;
using Server.Misc;
using Server.Network;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a hellsteed corpse")]
	public class HellSteed : BaseMount
	{
		[Constructable]
		public HellSteed()
			: this("a hellsteed")
		{
		}

		[Constructable]
		public HellSteed(string name)
			: base(name, 793, 0x3EBB, AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			// Adam: Remove IOBAlignment - tamables should not have alignment
			IOBAlignment = IOBAlignment.None;

			SetStr(401, 430);
			SetDex(133, 152);
			SetInt(101, 140);

			SetHits(241, 258);

			SetDamage(16, 20);

			SetSkill(SkillName.MagicResist, 65.1, 80.0);
			SetSkill(SkillName.Tactics, 65.1, 90.0);
			SetSkill(SkillName.Wrestling, 65.1, 80.0);

			Fame = 5500;
			Karma = -5500;

			VirtualArmor = 46;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 98.9;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Meat { get { return 5; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public HellSteed(Serial serial)
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
				{
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
					// no loot
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
		}
	}
}
