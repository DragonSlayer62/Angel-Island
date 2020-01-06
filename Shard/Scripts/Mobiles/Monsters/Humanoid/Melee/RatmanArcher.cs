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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/RatmanArcher.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions, changed rangefight to 6
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *	7/1/04
 *		up the damage a bit: From SetDamage( 4, 10 ) to SetDamage( 15, 25 );
 *		move the arrows back to OnBeforeDeath() and reduce the amount a lot
 *		(The ratman does not consume arrows as he fires.)
 *	6/11/04, mith
 *		Moved the equippable combat items out of OnBeforeDeath()
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
	[CorpseName("a ratman archer corpse")]
	public class RatmanArcher : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public RatmanArcher()
			: base(AIType.AI_Archer, FightMode.All | FightMode.Closest, 10, 6, 0.25, 0.5)
		{

			BaseSoundID = 437;

			SetStr(146, 180);
			SetDex(101, 130);
			SetInt(116, 140);

			SetHits(88, 108);

			// Adam: up the damage a bit. Was ( 4, 10 )
			if (Core.UOAI || Core.UOAR)
				SetDamage(15, 25);
			else
				SetDamage(4, 10);

			SetSkill(SkillName.Anatomy, 60.2, 100.0);
			SetSkill(SkillName.Archery, 80.1, 90.0);
			SetSkill(SkillName.MagicResist, 65.1, 90.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 6500;
			Karma = -6500;

			InitBody();
			InitOutfit();

			VirtualArmor = 56;

		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public override void InitBody()
		{
			Name = NameList.RandomName("ratman");
			Body = 0x8E;
		}
		public override void InitOutfit()
		{
			WipeLayers();

			// add a bow so the archer can shoot!
			//	(arrows aren't needed here, see: OnBeforeDeath)
			AddItem(new Bow());
		}

		public RatmanArcher(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				// adam: 20-30 arrows seem about right
				PackItem(new Arrow(Utility.RandomMinMax(20, 30)));
				PackGold(175, 225);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020214225651/uo.stratics.com/hunters/ratmanarcher.shtml
					// ? (unknown) - use RunUO loot

					if (Spawning)
						PackItem(new Arrow(Utility.RandomMinMax(50, 70)));

					AddLoot(LootPack.Rich);
				}
				else
				{
					if (Spawning)
						PackItem(new Arrow(Utility.RandomMinMax(50, 70)));

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

			if (Body == 42)
			{
				Body = 0x8E;
				Hue = 0;
			}
		}
	}
}
