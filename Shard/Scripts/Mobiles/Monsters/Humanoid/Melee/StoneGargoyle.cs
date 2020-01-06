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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/StoneGargoyle.cs
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
	[CorpseName("a gargoyle corpse")]
	public class StoneGargoyle : BaseCreature
	{
		[Constructable]
		public StoneGargoyle()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a stone gargoyle";
			Body = 67;
			BaseSoundID = 0x174;

			SetStr(246, 275);
			SetDex(76, 95);
			SetInt(81, 105);

			SetHits(148, 165);

			SetDamage(11, 17);

			SetSkill(SkillName.MagicResist, 85.1, 100.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 60.1, 100.0);

			Fame = 4000;
			Karma = -4000;

			VirtualArmor = 50;
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 0; } }

		public StoneGargoyle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackPotion();
				PackItem(new Arrow(10));
				PackGold(150, 250);
				PackScroll(1, 5);
				// TODO: Ore

				if (0.05 > Utility.RandomDouble())
					PackItem(new GargoylesPickaxe());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20021015002109/uo.stratics.com/hunters/stonegargoyle.shtml
					// 150 to 250 Gold, Potions, Arrows, Gems, Scrolls, Ore, "a gargoyle's pickaxe"
					if (Spawning)
					{
						PackGold(150, 250);
					}
					else
					{
						PackPotion();
						PackPotion(0.3);
						PackItem(new Arrow(Utility.RandomMinMax(1, 4)));
						PackGem(Utility.RandomMinMax(1, 4));
						PackScroll(1, 6);
						PackScroll(1, 6, 0.5);
						PackItem(new IronIngot(12));

						if (0.05 > Utility.RandomDouble())
							PackItem(new GargoylesPickaxe());
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new IronIngot(12));

						if (0.05 > Utility.RandomDouble())
							PackItem(new GargoylesPickaxe());
					}

					AddLoot(LootPack.Average, 2);
					AddLoot(LootPack.Gems, 1);
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
