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

/* Scripts/Mobiles/Animals/Reptiles/LavaSerpent.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("a lava serpent corpse")]
	[TypeAlias("Server.Mobiles.Lavaserpant")]
	public class LavaSerpent : BaseCreature
	{
		[Constructable]
		public LavaSerpent()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a lava serpent";
			Body = 90;
			BaseSoundID = 219;

			SetStr(386, 415);
			SetDex(56, 80);
			SetInt(66, 85);

			SetHits(232, 249);
			SetMana(0);

			SetDamage(10, 22);

			SetSkill(SkillName.MagicResist, 25.3, 70.0);
			SetSkill(SkillName.Tactics, 65.1, 70.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 40;
		}

		public override bool HasBreath { get { return true; } } // fire breath enabled
		public override int Meat { get { return 4; } }
		public override int Hides { get { return 15; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public LavaSerpent(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(90, 130);
				PackItem(new SulfurousAsh(3));
				PackArmor(1, 4);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// TODO: need reference from 2002, best we can find is Sep 2008
					// http://web.archive.org/web/20080804183706/uo.stratics.com/database/view.php?db_content=hunters&id=225
					// 200 - 250 Gold. 3 Sulfurous Ash, Bones, Body Parts, Armor
					//	Carved: 15 Spined Hides, 4 Raw Ribs
					if (Spawning)
					{
						PackGold(200, 250);
					}
					else
					{
						PackItem(new SulfurousAsh(3));
						switch (Utility.Random(10))
						{
							case 0: PackItem(new LeftArm()); break;
							case 1: PackItem(new RightArm()); break;
							case 2: PackItem(new Torso()); break;
							case 3: PackItem(new Bone()); break;
							case 4: PackItem(new RibCage()); break;
							case 5: PackItem(new RibCage()); break;
							case 6: PackItem(new BonePile()); break;
							case 7: PackItem(new BonePile()); break;
							case 8: PackItem(new BonePile()); break;
							case 9: PackItem(new BonePile()); break;
						}
						PackItem(Loot.RandomArmor());
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(new SulfurousAsh(3));
						PackItem(new Bone());
						// TODO: body parts, armour
					}

					AddLoot(LootPack.Average);
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

			if (BaseSoundID == -1)
				BaseSoundID = 219;
		}
	}
}
