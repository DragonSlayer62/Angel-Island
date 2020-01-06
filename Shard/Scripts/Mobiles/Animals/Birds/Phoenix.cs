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

/* Scripts/Mobiles/Animals/Birds/Phoenix.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a phoenix corpse")]
	public class Phoenix : BaseCreature
	{
		[Constructable]
		public Phoenix()
			: base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			Name = "a phoenix";
			Body = 5;
			Hue = 0x674;
			BaseSoundID = 0x8F;

			SetStr(504, 700);
			SetDex(202, 300);
			SetInt(504, 700);

			SetHits(340, 383);

			SetDamage(25);

			SetSkill(SkillName.EvalInt, 90.2, 100.0);
			SetSkill(SkillName.Magery, 90.2, 100.0);
			SetSkill(SkillName.Meditation, 75.1, 100.0);
			SetSkill(SkillName.MagicResist, 86.0, 135.0);
			SetSkill(SkillName.Tactics, 80.1, 90.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = 0;

			VirtualArmor = 60;
		}

		public override int Meat { get { return 1; } }
		public override MeatType MeatType { get { return MeatType.Bird; } }
		public override int Feathers { get { return 36; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 4 : 0; } }

		public Phoenix(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(500, 600);
				PackScroll(2, 7);
				PackMagicEquipment(2, 3, 0.50, 0.50);
				PackMagicEquipment(2, 3, 0.10, 0.10);
				PackMagicItem(1, 2, 1.0);
				PackMagicItem(1, 2, 0.70);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020403082709/uo.stratics.com/hunters/phoenix.shtml
					// 600 - 1000 Gold, Magic Items, 36 feathers, raw bird

					if (Spawning)
					{
						PackGold(600, 1000);
					}
					else
					{
						PackMagicEquipment(2, 3);
						PackMagicItem(1, 2);
					}
				}
				else
				{
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
