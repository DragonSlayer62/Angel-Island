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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/OgreLord.cs
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
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Factions;

namespace Server.Mobiles
{
	[CorpseName("an ogre lords corpse")]
	public class OgreLord : BaseCreature
	{
		public override Faction FactionAllegiance { get { return Minax.Instance; } }
		public override Ethics.Ethic EthicAllegiance { get { return Ethics.Ethic.Evil; } }

		[Constructable]
		public OgreLord()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "an ogre lord";
			Body = 83;
			BaseSoundID = 427;

			SetStr(767, 945);
			SetDex(66, 75);
			SetInt(46, 70);

			SetHits(476, 552);

			SetDamage(20, 25);

			SetSkill(SkillName.MagicResist, 125.1, 140.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;
		}

		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 5 : 0; } }
		public override int Meat { get { return 2; } }

		public OgreLord(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();

				PackItem(new Club());
				PackItem(new Arrow(10));
				PackGold(550, 700);
				PackMagicEquipment(2, 3, 0.40, 0.40);
				PackMagicEquipment(2, 3, 0.15, 0.15);
				// Category 3 MID
				PackMagicItem(1, 2, 0.10);
				PackMagicItem(1, 2, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202091246/uo.stratics.com/hunters/ogrelord.shtml
					// 200 to 250 Gold, Magic items, Weapon Carried, 2 Raw Ribs (carved)

					if (Spawning)
					{
						PackGold(200, 250);
					}
					else
					{
						PackMagicEquipment(2, 3);
						PackMagicItem(1, 2, 0.05);
						// stratics says "Weapon Carried", I think they mean a club as the ogre lord doesn't carry a weapon
						PackItem(new Club());
					}
				}
				else
				{
					if (Spawning)
						PackItem(new Club());

					AddLoot(LootPack.Rich, 2);
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
