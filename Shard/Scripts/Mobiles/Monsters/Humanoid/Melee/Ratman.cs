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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Ratman.cs
 * ChangeLog
 *	12/23/10, adam
 *		Fix body code to allow for the 3 varients (unarmed, sword, and axe)
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
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
	[CorpseName("a ratman's corpse")]
	public class Ratman : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Ratman; } }

		[Constructable]
		public Ratman()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Body = Utility.RandomList(42, 44, 45);
			//Body = 42;	// unarmed
			//Body = 44;	// axe
			//Body = 45;	// sword gray coat
			//Body = 142;	// sword purple coat - archer
			//Body = 143;	// sword red coat - mage

			BaseSoundID = 437;

			SetStr(96, 120);
			SetDex(81, 100);
			SetInt(36, 60);

			SetHits(58, 72);

			SetDamage(4, 5);

			SetSkill(SkillName.MagicResist, 35.1, 60.0);
			SetSkill(SkillName.Tactics, 50.1, 75.0);
			SetSkill(SkillName.Wrestling, 50.1, 75.0);

			Fame = 1500;
			Karma = -1500;

			InitBody();

			VirtualArmor = 28;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public override void InitBody()
		{
			Name = NameList.RandomName("ratman");
		}
		public override void InitOutfit()
		{

		}

		public Ratman(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020212095745/uo.stratics.com/hunters/ratman.shtml
					// 0 to 50 Gold, Weapon Carried, Reagents, 8 Hides (carved)

					if (Spawning)
					{
						PackGold(0, 50);
					}
					else
					{
						//Body = 42;	// unarmed
						//Body = 44;	// axe
						//Body = 45;	// sword gray coat

						// Weapon Carried
						if (this.Body == 44)
							PackItem(new Axe());
						else if (this.Body == 45)
							PackItem(new Longsword());

						PackReg(1, 4);
					}
				}
				else
				{
					AddLoot(LootPack.Meager);
					// TODO: weapon, misc
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
