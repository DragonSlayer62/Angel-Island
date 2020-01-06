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

/* Scripts/Mobiles/Monsters/Elemental/Magic/BloodElemental.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
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

namespace Server.Mobiles
{
	[CorpseName("a blood elemental corpse")]
	public class BloodElemental : BaseCreature
	{
		[Constructable]
		public BloodElemental()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a blood elemental";
			Body = 159;
			BaseSoundID = 278;

			SetStr(526, 615);
			SetDex(66, 85);
			SetInt(226, 350);

			SetHits(316, 369);

			SetDamage(17, 27);

			SetSkill(SkillName.EvalInt, 85.1, 100.0);
			SetSkill(SkillName.Magery, 85.1, 100.0);
			SetSkill(SkillName.Meditation, 10.4, 50.0);
			SetSkill(SkillName.MagicResist, 80.1, 95.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 80.1, 100.0);

			Fame = 12500;
			Karma = -12500;

			VirtualArmor = 60;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 5; } }

		public BloodElemental(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(500, 700);
				PackMagicEquipment(1, 3, 0.50, 0.50);
				PackMagicEquipment(1, 3, 0.10, 0.10);

				if (Utility.RandomDouble() >= 0.10)
					PackItem(new BloodVial(3));

				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20021015000826/uo.stratics.com/hunters/bloodelemental.shtml
					// 1100 to 1400 Gold, Magic items, Gems, Scrolls, Vials of Blood reagent, Level 5 Treasure Maps

					if (Spawning)
					{
						PackGold(1100, 1400);
					}
					else
					{
						PackMagicStuff(1, 3, 0.05);
						PackMagicStuff(1, 3, 0.03);
						PackGem(1, .9);
						PackGem(1, .5);
						PackScroll(1, 6, .9);
						PackScroll(1, 6, .5);
						PackItem(new BloodVial(3));
					}
				}
				else
				{	// standard runuo
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
