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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/RatmanMage.cs
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
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a glowing ratman corpse")]
	public class RatmanMage : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public RatmanMage()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{

			BaseSoundID = 437;

			SetStr(146, 180);
			SetDex(101, 130);
			SetInt(186, 210);

			SetHits(88, 108);

			SetDamage(7, 14);

			SetSkill(SkillName.EvalInt, 70.1, 80.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.MagicResist, 65.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 7500;
			Karma = -7500;

			InitBody();
			InitOutfit();

			VirtualArmor = 44;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public override void InitBody()
		{
			Name = NameList.RandomName("ratman");
			Body = 0x8F;
		}
		public override void InitOutfit()
		{

		}

		public RatmanMage(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(175, 225);
				PackReg(6);
				PackScroll(1, 7);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202091244/uo.stratics.com/hunters/ratmanmage.shtml
					// 300 Gold, Reagents, Magic Items, 8 Hides (carved), Statues

					if (Spawning)
					{
						PackGold(300);
					}
					else
					{
						PackReg(6);
						if (Utility.RandomBool())
							PackMagicEquipment(1, 2);
						else
							PackMagicItem(1, 2, 1);
						if (0.02 > Utility.RandomDouble())
							PackStatue();
					}
				}
				else
				{
					if (Spawning)
					{
						PackReg(6);

						if (0.02 > Utility.RandomDouble())
							PackStatue();
					}

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.LowScrolls);
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

			if (Body == 42)
			{
				Body = 0x8F;
				Hue = 0;
			}
		}
	}
}
