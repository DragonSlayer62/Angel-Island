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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/LichLord.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/16/04, Froste
 *      Changed IOBAlignment to Council
 *  11/10/04, Froste
 *      Implemented new random IOB drop system and changed drop change to 12%
 *	11/05/04, Pigpen
 *		Made changes for Implementation of IOBSystem. Changes include:
 *		Removed IsEnemy and Aggressive Action Checks. These are now handled in BaseCreature.cs
 *		Set Creature IOBAlignment to Undead.
 *	11/2/04, Adam
 *		Increase gold if this is IOB mobile resides in it's Stronghold (Wind)
 *		Reduce IDWand drop to 10% from 20% and only drop if in Stronghold (Wind)
 *	9/26/04, Adam
 *		Add 5% IOB drop (BloodDrenchedBandana)
 *	8/9/04, Adam
 *		1. Add 10-20 Black Pearl to drop
 *	7/21/04, mith
 *		IsEnemy() and AggressiveAction() code added to support Brethren property of BloodDrenchedBandana.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *	7/2/04, Adam
 *		Change chance to drop a magic item to 30% 
 *		add a 5% chance for a bonus drop at next intensity level
 * 	6/26/04 Adam: liches carry IDWands. It's historical man!
 *		20% chance to get an IDWand
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a liche's corpse")]
	public class LichLord : BaseCreature
	{
		[Constructable]
		public LichLord()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a lich lord";
			Body = 79;
			BaseSoundID = 412;
			IOBAlignment = IOBAlignment.Council;
			ControlSlots = 4;

			SetStr(416, 505);
			SetDex(146, 165);
			SetInt(566, 655);

			SetHits(250, 303);

			SetDamage(11, 13);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 150.5, 200.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 50;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }

		public LichLord(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new GnarledStaff());
				PackScroll(3, 7);
				PackScroll(3, 7);
				PackMagicEquipment(2, 3, 0.60, 0.60);
				PackMagicEquipment(2, 3, 0.25, 0.25);
				PackReg(10, 20);

				// pack the gold
				PackGold(600, 800);

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);

					if (Utility.RandomDouble() < 0.10)
						PackItem(new IDWand());
				}

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				// Category 4 MID
				PackMagicItem(2, 3, 0.10);
				PackMagicItem(2, 3, 0.05);
				PackMagicItem(2, 3, 0.02);

				// pack bulk reg
				PackItem(new BlackPearl(Utility.RandomMinMax(10, 20)));
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020213054135/uo.stratics.com/hunters/lichlord.shtml
					// 400 to 700 Gold, Magic items, Gems, Scrolls, Blackmoor reagent

					if (Spawning)
					{
						PackGold(400, 700);
					}
					else
					{
						PackMagicEquipment(2, 3);
						PackMagicItem(2, 3, 0.10);
						PackGem(1, .9);
						PackGem(1, .05);
						PackScroll(4, 7, .9);
						PackScroll(4, 7, .05);
						PackItem(typeof(Blackmoor), 0.005);		// TODO: no idea the rarity, make it 5 in 1000
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new GnarledStaff());
						if (Core.AOS)
							PackNecroReg(12, 40);
					}

					AddLoot(LootPack.FilthyRich);
					AddLoot(LootPack.MedScrolls, 2);
				}
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.FeyAndUndead; }
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
