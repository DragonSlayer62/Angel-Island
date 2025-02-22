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

/* Scripts/Mobiles/Monsters/Reptile/Melee/Lizardman.cs
 * ChangeLog
 *  2/9/11, Adam
 *		spawnewd from orc camp.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Misc;

namespace Server.Mobiles
{
	[CorpseName("a lizardman corpse")]
	public class Lizardman : BaseCreature
	{
		public override InhumanSpeech SpeechType { get { return InhumanSpeech.Lizardman; } }

		[Constructable]
		public Lizardman()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = NameList.RandomName("lizardman");
			Body = Utility.RandomList(35, 36);
			BaseSoundID = 417;

			SetStr(96, 120);
			SetDex(86, 105);
			SetInt(36, 60);

			SetHits(58, 72);

			SetDamage(5, 7);

			SetSkill(SkillName.MagicResist, 35.1, 60.0);
			SetSkill(SkillName.Tactics, 55.1, 80.0);
			SetSkill(SkillName.Wrestling, 50.1, 70.0);

			Fame = 1500;
			Karma = -1500;

			VirtualArmor = 28;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int Meat { get { return 1; } }
		public override int Hides { get { return 12; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public Lizardman(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
				PackGold(25, 50);
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020213041111/uo.stratics.com/hunters/lizardman.shtml
					// 0 to 50 Gold, Weapon Carried, 1 Raw Ribs (carved), 12 Hides (carved)

					if (Spawning)
					{
						PackGold(0, 50);
					}
					else
					{
						// Weapon Carried
						if (this.Body == 0x23)
							PackItem(new ShortSpear());
						else if (this.Body == 0x24)
							PackItem(new WarMace());
					}
				}
				else
				{
					AddLoot(LootPack.Meager);
					// TODO: weapon
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
