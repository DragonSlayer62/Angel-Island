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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/BoneKnightLord.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 7 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/19/04, Adam
 *		1. Create from BoneKnight.cs
 *		2. stats and loot based on a scaled down Executioner
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a bone knight lord corpse")]
	public class BoneKnightLord : BaseCreature
	{
		[Constructable]
		public BoneKnightLord()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a bone knight lord";
			Body = 57;
			BaseSoundID = 451;
			IOBAlignment = IOBAlignment.Undead;
			ControlSlots = 3;

			SetStr(196 + 20, 250 + 20);
			SetDex(76, 95);
			SetInt(36, 60);

			SetHits(118 + 20, 150 + 20);

			SetDamage(8 + 1, 18 + 2);

			SetSkill(SkillName.MagicResist, 65.1, 80.0);
			SetSkill(SkillName.Tactics, 85.1, 100.0);
			SetSkill(SkillName.Wrestling, 85.1, 95.0);

			Fame = 3000;
			Karma = -3000;

			VirtualArmor = 40;
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				switch (Utility.Random(6))
				{
					case 0: PackItem(new PlateArms()); break;
					case 1: PackItem(new PlateChest()); break;
					case 2: PackItem(new PlateGloves()); break;
					case 3: PackItem(new PlateGorget()); break;
					case 4: PackItem(new PlateLegs()); break;
					case 5: PackItem(new PlateHelm()); break;
				}

				PackItem(new Scimitar());
				PackItem(new WoodenShield());
				PackItem(new Bone(Utility.Random(9, 16)));

				PackGold(200, 400);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// ai special
					if (Spawning)
					{
						PackGold(0);
					}
					else
					{
					}
				}
				else
				{	// ai special
					AddLoot(LootPack.Average);
					AddLoot(LootPack.Meager);
				}
			}
		}

		public BoneKnightLord(Serial serial)
			: base(serial)
		{
		}

		public override bool OnBeforeDeath()
		{
			return base.OnBeforeDeath();
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
