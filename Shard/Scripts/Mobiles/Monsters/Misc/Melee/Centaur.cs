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

/* Scripts/Mobiles/Monsters/Misc/Melee/Centaur.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *  07/02/06, Kit
 *		InitBody/InitOutfit additions, changed range fight to 6
 *  08/29/05 TK
 *		Changed AIType to Archer
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *	6/11/04, mith
 *		Moved the equippable items out of OnBeforeDeath()
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a centaur corpse")]
	public class Centaur : BaseCreature
	{
		[Constructable]
		public Centaur()
			: base(AIType.AI_Archer, FightMode.Aggressor, 10, 6, 0.25, 0.5)
		{

			BaseSoundID = 679;

			SetStr(202, 300);
			SetDex(104, 260);
			SetInt(91, 100);

			SetHits(130, 172);

			SetDamage(13, 24);

			SetSkill(SkillName.Anatomy, 95.1, 115.0);
			SetSkill(SkillName.Archery, 95.1, 100.0);
			SetSkill(SkillName.MagicResist, 50.3, 80.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 95.1, 100.0);

			Fame = 6500;
			Karma = 0;

			InitBody();
			InitOutfit();

			VirtualArmor = 50;


			PackItem(new Arrow(Utility.RandomMinMax(80, 90))); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public Centaur(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
			Name = NameList.RandomName("centaur");
			Body = 101;

		}
		public override void InitOutfit()
		{
			WipeLayers();
			AddItem(new Bow());	// functional, used for shooting

		}
		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(180, 250);
				PackGem();
				PackMagicEquipment(1, 2, 0.15, 0.15);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020806171152/uo.stratics.com/hunters/centaur.shtml
					// 250-300 Gold, Scrolls, Bow, Magic Items
					if (Spawning)
					{
						PackGold(250, 300);
					}
					else
					{
						PackScroll(3, 7);
						PackScroll(3, 7, .8);

						// bow dropped as part of outfit

						if (Utility.RandomBool())
							PackMagicEquipment(1, 2, 0.15, 0.15);
						else
							PackMagicItem(1, 1, 0.15);
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
					{
						//	AddItem(new Bow()); - added in initoutfit
						PackItem(new Arrow(Utility.RandomMinMax(80, 90))); // OSI it is different: in a sub backpack, this is probably just a limitation of their engine
					}

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.Average);
					AddLoot(LootPack.Gems);
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

			if (BaseSoundID == 678)
				BaseSoundID = 679;
		}
	}
}
