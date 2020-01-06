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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Ettin.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("an ettins corpse")]
	public class Ettin : BaseCreature
	{
		[Constructable]
		public Ettin()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "an ettin";
			Body = 18;
			BaseSoundID = 367;

			SetStr(136, 165);
			SetDex(56, 75);
			SetInt(31, 55);

			SetHits(82, 99);

			SetDamage(7, 17);

			SetSkill(SkillName.MagicResist, 40.1, 55.0);
			SetSkill(SkillName.Tactics, 50.1, 70.0);
			SetSkill(SkillName.Wrestling, 50.1, 60.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 38;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override int Meat { get { return Core.UOAI || Core.UOAR ? 4 : 5; } }

		public Ettin(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackPotion();
				PackGold(100, 150);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020414130245/uo.stratics.com/hunters/ettin.shtml
					// 	50 to 150 Gold, Potions, Arrows, Gems, 5 Raw Ribs (carved)
					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion();
						PackItem(new Arrow(Utility.Random(1, 2)));
						PackGem(1, .9);
						PackGem(1, .05);
					}
				}
				else
				{
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.Average);
					AddLoot(LootPack.Potions);
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
