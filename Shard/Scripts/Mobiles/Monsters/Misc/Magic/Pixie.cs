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

/* Scripts/Mobiles/Monsters/Misc/Magic/Pixie.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName("a pixie corpse")]
	public class Pixie : BaseCreature
	{
		public override bool InitialInnocent { get { return true; } }

		[Constructable]
		public Pixie()
			: base(AIType.AI_Mage, FightMode.Aggressor | FightMode.Evil, 10, 1, 0.175, 0.350)
		{

			BaseSoundID = 0x467;

			SetStr(21, 30);
			SetDex(301, 400);
			SetInt(201, 250);

			SetHits(13, 18);

			SetDamage(9, 15);

			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 100.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 100.5, 150.0);
			SetSkill(SkillName.Tactics, 10.1, 20.0);
			SetSkill(SkillName.Wrestling, 10.1, 12.5);

			Fame = 7000;
			Karma = 7000;

			InitBody();

			VirtualArmor = 100;
		}

		public override HideType HideType { get { return HideType.Spined; } }
		public override int Hides { get { return Core.UOAI || Core.UOAR ? 5 : 20; } }
		public override int Meat { get { return Core.UOAI || Core.UOAR ? 1 : 19; } }


		public override void InitBody()
		{
			Name = NameList.RandomName("pixie");
			Body = 128;
		}
		public override void InitOutfit()
		{

		}
		public Pixie(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackGem();
				PackScroll(1, 7);
				// TODO: Statue
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806152337/uo.stratics.com/hunters/pixie.shtml
					// gems, scrolls, statues, 20 hides, 19 ribs
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);

						PackScroll(3, 7);
						PackScroll(3, 7, .8);
						PackScroll(3, 7, .5);

						if (0.02 > Utility.RandomDouble())
							PackStatue();
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
					{
						if (0.02 > Utility.RandomDouble())
							PackStatue();
					}

					AddLoot(LootPack.LowScrolls);
					AddLoot(LootPack.Gems, 2);
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
