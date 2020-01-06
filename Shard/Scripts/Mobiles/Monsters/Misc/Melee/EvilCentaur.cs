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

/* Scripts/Mobiles/Monsters/Misc/Melee/EvilCentaur.cs
 * ChangeLog
 *  07/02/06, Kit
 *		InitBody/InitOutfit additions, changed rangefight to 6
 *  08/29/05 TK
 *		Changed AIType to Archer
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
 *	3/5/05, Adam
 *		1. First time checkin - based on centaur.cs
 *		2. Add healing
 *		3. Make evil (red)
 *		4. set FightMode to "Weakest". This is anti-bard code :)
 *		5. Add neg karma for kill
 *		6. reduce arrows from 80-90 to 20-30
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;

namespace Server.Mobiles
{
	[CorpseName("a centaur corpse")]
	public class EvilCentaur : BaseCreature
	{
		[Constructable]
		public EvilCentaur()
			: base(AIType.AI_Archer, FightMode.All | FightMode.Weakest, 10, 6, 0.2, 0.4)
		{

			BaseSoundID = 679;

			SetStr(202, 300);
			SetDex(104, 260);
			SetInt(91, 100);

			SetHits(130, 172);

			SetDamage(13, 24);

			SetSkill(SkillName.Anatomy, 95.1, 115.0);
			SetSkill(SkillName.Archery, 95.1, 100.0);
			SetSkill(SkillName.MagicResist, 50.3, 80.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 95.1, 100.0);

			Fame = 6500;
			Karma = -6500;

			InitBody();
			InitOutfit();

			VirtualArmor = 50;


			PackItem(new Arrow(Utility.RandomMinMax(20, 30)));
			PackItem(new Bandage(Utility.RandomMinMax(1, 15)));
		}

		public override int Meat { get { return 1; } }
		public override int Hides { get { return 8; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanBandage { get { return true; } }
		public override TimeSpan BandageDelay { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(10, 13)); } }

		public override void InitBody()
		{
			Name = NameList.RandomName("centaur");
			Body = 101;
		}
		public override void InitOutfit()
		{
			WipeLayers();
			AddItem(new Bow());

		}
		public EvilCentaur(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(180, 250);
				PackGem();
				PackMagicEquipment(1, 2, 0.15, 0.15);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
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
				{	// Standard RunUO
					// ai special
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

			if (BaseSoundID == 678)
				BaseSoundID = 679;
		}
	}
}
