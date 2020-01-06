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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/FireGargoyle.cs
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
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a charred corpse")]
	public class FireGargoyle : BaseCreature
	{
		[Constructable]
		public FireGargoyle()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = NameList.RandomName("fire gargoyle");
			Body = 130;
			BaseSoundID = 0x174;

			SetStr(351, 400);
			SetDex(126, 145);
			SetInt(226, 250);

			SetHits(211, 240);

			SetDamage(7, 14);

			SetSkill(SkillName.Anatomy, 75.1, 85.0);
			SetSkill(SkillName.EvalInt, 90.1, 105.0);
			SetSkill(SkillName.Magery, 90.1, 105.0);
			SetSkill(SkillName.Meditation, 90.1, 105.0);
			SetSkill(SkillName.MagicResist, 90.1, 105.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 40.1, 80.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 1; } }
		public override AuraType MyAura { get { return AuraType.Fire; } }

		public FireGargoyle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGold(200, 250);
				PackMagicEquipment(1, 2, 0.20, 0.20);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020220114113/uo.stratics.com/hunters/firegargoyle.shtml
					// 200-300 Gold, Magic Items, Gems, Lvl one Treasure Maps

					if (Spawning)
					{
						PackGold(200, 300);
					}
					else
					{
						PackMagicEquipment(1, 2);
						PackMagicItem(1, 1, 0.05);
						PackGem(1, .9);
						PackGem(1, .05);
					}
				}
				else
				{
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Gems);
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
