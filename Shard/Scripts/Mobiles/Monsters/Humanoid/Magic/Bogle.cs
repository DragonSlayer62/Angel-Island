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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/Bogle.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 */

using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("a ghostly corpse")]
	public class Bogle : BaseCreature
	{
		[Constructable]
		public Bogle()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a bogle";
			Body = 153;
			BaseSoundID = 0x482;

			SetStr(76, 100);
			SetDex(76, 95);
			SetInt(36, 60);

			SetHits(46, 60);

			SetDamage(7, 11);

			SetSkill(SkillName.EvalInt, 55.1, 70.0);
			SetSkill(SkillName.Magery, 35.1, 40.0);
			SetSkill(SkillName.MagicResist, 35.1, 40.0);
			SetSkill(SkillName.Tactics, 45.1, 60.0);
			SetSkill(SkillName.Wrestling, 45.1, 55.0);

			Fame = 4000;
			Karma = -4000;
		}

		public override Poison PoisonImmune { get { return Poison.Lethal; } }

		public Bogle(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(10, 50);
				PackItem(Loot.RandomWeapon());
				PackItem(new Bone());
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// none circa 2002
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
					if (Spawning)
					{
						PackItem(Loot.RandomWeapon());
						PackItem(new Bone());
					}

					AddLoot(LootPack.Meager);
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
		}
	}
}