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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/Betrayer.cs
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
using Server.Misc;
using Server.Items;

namespace Server.Mobiles
{
	public class Betrayer : BaseCreature
	{
		[Constructable]
		public Betrayer()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = NameList.RandomName("male");
			Title = "the betrayer";
			Body = 767;
			BardImmune = true;


			SetStr(401, 500);
			SetDex(81, 100);
			SetInt(151, 200);

			SetHits(241, 300);

			SetDamage(16, 22);

			SetSkill(SkillName.Anatomy, 90.1, 100.0);
			SetSkill(SkillName.EvalInt, 90.1, 100.0);
			SetSkill(SkillName.Magery, 50.1, 100.0);
			SetSkill(SkillName.Meditation, 90.1, 100.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 65;
			SpeechHue = Utility.RandomDyedHue();
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int Meat { get { return 1; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }

		public Betrayer(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();

				switch (Utility.Random(4))
				{
					case 0: PackItem(new Katana()); break;
					case 1: PackItem(new BodySash()); break;
					case 2: PackItem(new Halberd()); break;
					case 3: PackItem(new LapHarp()); break;
				}

				PackGold(200, 300);

				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// no LBR
					//http://web.archive.org/web/20020403051743/uo.stratics.com/hunters/betrayer.shtml
					// 200-300 Gold, Gem, Power Crystal, Level 5 Treasue Map, Blackthorne's "A welcome"
					if (Spawning)
					{
						PackGold(200, 300);
					}
					else
					{
					}
				}
				else
				{
					if (Spawning)
					{	// probably not for siege
						//PackItem(new PowerCrystal());

						//if (0.02 > Utility.RandomDouble())
						//PackItem(new BlackthornWelcomeBook());
					}

					AddLoot(LootPack.FilthyRich);
					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Gems, 1);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
