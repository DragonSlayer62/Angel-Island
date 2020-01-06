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

/* Scripts/Mobiles/Monsters/Elemental/Melee/SnowElemental.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a snow elemental corpse")]
	public class SnowElemental : BaseCreature
	{
		[Constructable]
		public SnowElemental()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a snow elemental";
			Body = 163;
			BaseSoundID = 263;

			SetStr(326, 355);
			SetDex(166, 185);
			SetInt(71, 95);

			SetHits(196, 213);

			SetDamage(11, 17);

			SetSkill(SkillName.MagicResist, 50.1, 65.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 80.1, 100.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 50;
		}

		public override int TreasureMapLevel { get { return Utility.RandomList(2, 3); } }

		public override AuraType MyAura { get { return AuraType.Ice; } }

		public SnowElemental(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackItem(new IronOre(3));
				PackItem(new BlackPearl(3));
				PackGold(175, 225);
				PackMagicEquipment(1, 5);
				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020213041409/uo.stratics.com/hunters/snowelemental.shtml
					// 200 to 250 Gold, Ore, Black Pearl
					if (Spawning)
					{
						PackGold(200, 250);
					}
					else
					{
						PackItem(new IronOre(3));
						PackItem(new BlackPearl(3));
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new IronOre(3));
						PackItem(new BlackPearl(3));
					}

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
