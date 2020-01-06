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

/* Scripts/Mobiles/Monsters/Elemental/Magic/Efreet.cs
 * ChangeLog
 *	4/5/09, Adam
 *		change Daemon bone armor drop rate from 25% to 5%
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 *	4/24/04, adam
 *		Add break statements for the OnBeforeDeath switch statement
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("an efreet corpse")]
	public class Efreet : BaseCreature
	{
		[Constructable]
		public Efreet()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "an efreet";
			Body = 131;
			BaseSoundID = 768;

			SetStr(326, 355);
			SetDex(266, 285);
			SetInt(171, 195);

			SetHits(196, 213);

			SetDamage(11, 13);

			SetSkill(SkillName.EvalInt, 60.1, 75.0);
			SetSkill(SkillName.Magery, 60.1, 75.0);
			SetSkill(SkillName.MagicResist, 60.1, 75.0);
			SetSkill(SkillName.Tactics, 60.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 10000;
			Karma = -10000;

			VirtualArmor = 56;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }

		public Efreet(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();

				PackItem(new Arrow(10));
				PackGold(200, 250);

				PackMagicEquipment(1, 2, 0.15, 0.15);

				// 5% drop chance
				if (Utility.RandomChance(5))
					switch (Utility.Random(1, 5))
					{
						case 1: PackItem(new DaemonHelm()); break;
						case 2: PackItem(new DaemonChest()); break;
						case 3: PackItem(new DaemonGloves()); break;
						case 4: PackItem(new DaemonLegs()); break;
						case 5: PackItem(new DaemonArms()); break;
					}

				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020403232027/uo.stratics.com/hunters/efreet.shtml
					// 	250 to 500 Gold, Potions, Arrows, Magic Items, Gems, Daemon Bone Armor
					if (Spawning)
					{
						PackGold(250, 500);
					}
					else
					{
						PackPotion();
						PackPotion(.8);
						PackPotion(.5);
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));

						if (Utility.RandomBool())
							PackMagicEquipment(1, 2, 0.15, 0.15);
						else
							PackMagicItem(1, 1, 0.05);

						PackGem(1, .9);
						PackGem(1, .05);

						if (0.02 > Utility.RandomDouble())
							switch (Utility.Random(5))
							{
								case 0: PackItem(new DaemonArms()); break;
								case 1: PackItem(new DaemonChest()); break;
								case 2: PackItem(new DaemonGloves()); break;
								case 3: PackItem(new DaemonLegs()); break;
								case 4: PackItem(new DaemonHelm()); break;
							}
					}
				}
				else
				{
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average);
					AddLoot(LootPack.Gems);

					// I think RunUO has this wrong. They are calling this random drop in both GenerateLoot invocations, 
					// Spawning and not Spawning. I'm assuming it should really be !Spawning, i.e., onDeath
					if (!Spawning)
						if (0.02 > Utility.RandomDouble())
						{
							switch (Utility.Random(5))
							{
								case 0: PackItem(new DaemonArms()); break;
								case 1: PackItem(new DaemonChest()); break;
								case 2: PackItem(new DaemonGloves()); break;
								case 3: PackItem(new DaemonLegs()); break;
								case 4: PackItem(new DaemonHelm()); break;
							}
						}
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
