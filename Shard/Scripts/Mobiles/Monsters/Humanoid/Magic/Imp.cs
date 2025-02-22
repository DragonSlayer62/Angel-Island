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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/Imp.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	1/14/05, Adam
 *		Give the imp the EvalInt and Magery of a Shade.
 *		(Was too powerful for the first wave of the champ spawn.)
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("an imp corpse")]
	public class Imp : BaseCreature
	{
		[Constructable]
		public Imp()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "an imp";
			Body = 74;
			BaseSoundID = 422;

			SetStr(91, 115);
			SetDex(61, 80);
			SetInt(86, 105);

			SetHits(55, 70);

			SetDamage(10, 14);

			SetSkill(SkillName.EvalInt, 55.1, 70.0);
			SetSkill(SkillName.Magery, 35.1, 45.0);
			SetSkill(SkillName.MagicResist, 30.1, 50.0);
			SetSkill(SkillName.Tactics, 42.1, 50.0);
			SetSkill(SkillName.Wrestling, 40.1, 44.0);

			Fame = 2500;
			Karma = -2500;

			VirtualArmor = 30;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 83.1;
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 6; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override PackInstinct PackInstinct { get { return PackInstinct.Daemon; } }

		public Imp(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
				PackScroll(1, 7);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020207055230/uo.stratics.com/hunters/imp.shtml
					// no loot
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{
					AddLoot(LootPack.Meager);
					AddLoot(LootPack.MedScrolls, 2);
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
