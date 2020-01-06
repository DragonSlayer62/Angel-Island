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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Ogre.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("an ogre corpse")]
	public class Ogre : BaseCreature
	{
		[Constructable]
		public Ogre()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "an ogre";
			Body = 1;
			BaseSoundID = 427;

			SetStr(166, 195);
			SetDex(46, 65);
			SetInt(46, 70);

			SetHits(100, 117);
			SetMana(0);

			SetDamage(9, 11);

			SetSkill(SkillName.MagicResist, 55.1, 70.0);
			SetSkill(SkillName.Tactics, 60.1, 70.0);
			SetSkill(SkillName.Wrestling, 70.1, 80.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 32;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 1 : 0; } }
		public override int Meat { get { return 2; } }

		public Ogre(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(60, 90);
				PackItem(new Club());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020214224839/uo.stratics.com/hunters/ogre.shtml
					// 50 to 150 Gold, Potions, Arrows, Gems, Weapon Carried, 2 Raw Ribs (carved)

					if (Spawning)
					{
						PackGold(50, 150);
					}
					else
					{
						PackPotion();
						PackItem(new Arrow(Utility.RandomMinMax(1, 3)));
						PackGem(1, .9);
						PackGem(1, .05);

						// stratics says "Weapon Carried", I think they mean a club as the ogre doesn't carry a weapon
						PackItem(new Club());
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new Club());
					}
					else
					{
						PackItem(new Arrow());
					}


					AddLoot(LootPack.Average);
					AddLoot(LootPack.Potions);
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
