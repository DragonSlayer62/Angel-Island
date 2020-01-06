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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Cyclops.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a cyclops corpse")]
	public class Cyclops : BaseCreature
	{
		[Constructable]
		public Cyclops()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a cyclops";
			Body = 75;
			BaseSoundID = 604;

			SetStr(336, 385);
			SetDex(96, 115);
			SetInt(31, 55);

			SetHits(202, 231);
			SetMana(0);

			SetDamage(7, 23);

			SetSkill(SkillName.MagicResist, 60.3, 105.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 80.1, 90.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 48;
		}

		public override int Meat { get { return Core.UOAI || Core.UOAR ? 2 : 4; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 3 : 0; } }

		public Cyclops(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(200, 250);
				PackMagicEquipment(1, 2, 0.15, 0.15);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202091348/uo.stratics.com/hunters/cyclops.shtml
					// 450 to 650 Gold, Potions, Arrows, Gems, Magic Items
					if (Spawning)
					{
						PackGold(450, 650);
					}
					else
					{
						PackPotion();
						PackPotion(.5);
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));
						PackGem(Utility.RandomMinMax(1, 4));
						if (Utility.RandomBool())
							PackMagicEquipment(1, 2, 0.15, 0.15);
						else
							PackMagicItem(1, 1, 0.05);
					}
				}
				else
				{
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average);
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
