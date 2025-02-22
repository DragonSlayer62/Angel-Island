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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Skeleton.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/16/04, Froste
 *      Added IOBAlignment=IOBAlignment.Undead, added the random IOB drop to loot
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("a skeletal corpse")]
	public class Skeleton : BaseCreature
	{
		[Constructable]
		public Skeleton()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.3, 0.6)
		{
			Name = "a skeleton";
			Body = Utility.RandomList(50, 56);
			BaseSoundID = 0x48D;
			IOBAlignment = IOBAlignment.Undead;
			ControlSlots = 1;

			SetStr(56, 80);
			SetDex(56, 75);
			SetInt(16, 40);

			SetHits(34, 48);

			SetDamage(3, 7);

			SetSkill(SkillName.MagicResist, 45.1, 60.0);
			SetSkill(SkillName.Tactics, 45.1, 60.0);
			SetSkill(SkillName.Wrestling, 45.1, 55.0);

			Fame = 450;
			Karma = -450;

			VirtualArmor = 16;
		}

		public override Poison PoisonImmune { get { return Poison.Lesser; } }

		public Skeleton(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				switch (Utility.Random(5))
				{
					case 0: PackItem(new BoneArms()); break;
					case 1: PackItem(new BoneChest()); break;
					case 2: PackItem(new BoneGloves()); break;
					case 3: PackItem(new BoneLegs()); break;
					case 4: PackItem(new BoneHelm()); break;
				}

				PackGold(0, 25);
				PackItem(new Bone(Utility.Random(7, 10)));
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
				{	// http://web.archive.org/web/20021014235748/uo.stratics.com/hunters/skeleton.shtml
					// 0 to 50 Gold, Sometimes Bone Armor, Weapon Carried
					if (Spawning)
					{
						PackGold(0, 50);
					}
					else
					{
						if (0.12 > Utility.RandomDouble())
							switch (Utility.Random(5))
							{
								case 0: PackItem(new BoneArms()); break;
								case 1: PackItem(new BoneChest()); break;
								case 2: PackItem(new BoneGloves()); break;
								case 3: PackItem(new BoneLegs()); break;
								case 4: PackItem(new BoneHelm()); break;
							}

						// Weapon Carried
						if (Body == 56)
							PackItem(new Hatchet());
					}
				}
				else
				{
					if (Spawning)
					{
						switch (Utility.Random(5))
						{
							case 0: PackItem(new BoneArms()); break;
							case 1: PackItem(new BoneChest()); break;
							case 2: PackItem(new BoneGloves()); break;
							case 3: PackItem(new BoneLegs()); break;
							case 4: PackItem(new BoneHelm()); break;
						}
					}

					AddLoot(LootPack.Poor);
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
