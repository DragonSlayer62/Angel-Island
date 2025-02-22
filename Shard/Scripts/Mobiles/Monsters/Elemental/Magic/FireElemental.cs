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

/* Scripts/Mobiles/Monsters/Elemental/Magic/FireElemental.cs
 * ChangeLog
 *	7/16/10, adam
 *		o decrease average dex
 *		o increase average int 
 *		o increase average hp
 *		o decrease average damage
 *		o decrease average magery
 *		o new skill meditation
 *		o increase Dispel Difficulty
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	4/27/05, Kit
 *		Adjusted dispell difficulty
 *	7/6/04, Adam
 *		1. implement Jade's new Category Based Drop requirements
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a fire elemental corpse")]
	public class FireElemental : BaseCreature
	{
		public override double DispelDifficulty { get { return Summoned ? 100 : 73; } }
		public override double DispelFocus { get { return 45.0; } }

		[Constructable]
		public FireElemental()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a fire elemental";
			Body = 15;
			BaseSoundID = 838;

			SetStr(126, 155);
			SetDex(166, 185);
			SetInt(101, 125);
			SetHits(76, 93);
			SetDamage(7, 9);

			SetSkill(SkillName.EvalInt, 60.1, 75.0);
			SetSkill(SkillName.Magery, 60.1, 75.0);
			SetSkill(SkillName.MagicResist, 75.2, 105.0);
			SetSkill(SkillName.Tactics, 80.1, 100.0);
			SetSkill(SkillName.Wrestling, 70.1, 100.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
			ControlSlots = 4;

			AddItem(new LightSource());
		}

		public FireElemental(bool summoned)
			: this()
		{
			if (summoned == true)
			{
				SetStr(126, 155);
				SetDex(75 - 10, 75 + 10);
				SetInt(400 - 10, 400 + 10);
				SetHits(150 - 10, 150 + 10);
				SetDamage(14 - 2, 14 + 2);

				SetSkill(SkillName.EvalInt, 60.1, 75.0);
				SetSkill(SkillName.Magery, 90 - 10, 90 + 10);
				SetSkill(SkillName.MagicResist, 75.2, 105.0);
				SetSkill(SkillName.Tactics, 80.1, 100.0);
				SetSkill(SkillName.Wrestling, 70.1, 100.0);
				SetSkill(SkillName.Meditation, 100 - 10, 100.0 + 10);
			}
		}

		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 0; } }

		public FireElemental(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();

				PackItem(new SulfurousAsh(3));
				PackGold(100, 130);

				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020202091822/uo.stratics.com/hunters/fireelemental.shtml
					// 150 to 300 Gold, Gems, Sulphurous Ash

					if (Spawning)
					{
						PackGold(150, 300);
					}
					else
					{
						PackGem(1, .9);
						PackGem(1, .05);
						PackItem(new SulfurousAsh(3));
					}
				}
				else
				{
					if (Spawning)
						PackItem(new SulfurousAsh(3));

					AddLoot(LootPack.Average);
					AddLoot(LootPack.Meager);
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

			if (BaseSoundID == 274)
				BaseSoundID = 838;
		}
	}
}
