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

/* Scripts/Mobiles/Animals/Reptiles/IceSerpent.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	[CorpseName("an ice serpent corpse")]
	[TypeAlias("Server.Mobiles.Iceserpant")]
	public class IceSerpent : BaseCreature
	{
		[Constructable]
		public IceSerpent()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			Name = "a giant ice serpent";
			Body = 89;
			BaseSoundID = 219;

			SetStr(216, 245);
			SetDex(26, 50);
			SetInt(66, 85);

			SetHits(130, 147);
			SetMana(0);

			SetDamage(7, 17);

			SetSkill(SkillName.Anatomy, 27.5, 50.0);
			SetSkill(SkillName.MagicResist, 25.1, 40.0);
			SetSkill(SkillName.Tactics, 75.1, 80.0);
			SetSkill(SkillName.Wrestling, 60.1, 80.0);

			Fame = 3500;
			Karma = -3500;

			VirtualArmor = 32;
		}

		public override int Meat { get { return 4; } }
		public override int Hides { get { return 15; } }
		public override HideType HideType { get { return HideType.Spined; } }

		public IceSerpent(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(25, 50);
				PackItem(Loot.RandomArmorOrShieldOrWeapon());
				PackItem(new Bone());
				// TODO: body parts, glacial staff
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20011031224927/uo.stratics.com/hunters/gianticeserpent.shtml
					// 50 Gold, Weapon or Armor, Body Parts, Jewelry, Glacial Staff
					if (Spawning)
					{
						PackGold(50);
					}
					else
					{
						PackItem(Loot.RandomArmorOrWeapon());

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

						PackItem(Loot.RandomJewelry());

						// TODO: complete the instalation and see if it existed on Siege
						//if (0.025 > Utility.RandomDouble())
						//PackItem(new GlacialStaff());
					}
				}
				else
				{
					if (Spawning)
					{
						PackItem(Loot.RandomArmorOrShieldOrWeapon());

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

						// TODO: complete the instalation
						if (0.025 > Utility.RandomDouble())
							PackItem(new GlacialStaff());
					}

					AddLoot(LootPack.Meager);
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
