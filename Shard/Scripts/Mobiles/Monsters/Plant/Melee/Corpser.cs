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

/* Scripts/Mobiles/Monsters/Plant/Melee/Corpser.cs
 * ChangeLog
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
	[CorpseName("a corpser corpse")]
	public class Corpser : BaseCreature
	{
		[Constructable]
		public Corpser()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = "a corpser";
			Body = 8;
			BaseSoundID = 684;

			SetStr(156, 180);
			SetDex(26, 45);
			SetInt(26, 40);

			SetHits(94, 108);
			SetMana(0);

			SetDamage(10, 23);

			SetSkill(SkillName.MagicResist, 15.1, 20.0);
			SetSkill(SkillName.Tactics, 45.1, 60.0);
			SetSkill(SkillName.Wrestling, 45.1, 60.0);

			Fame = 1000;
			Karma = -1000;

			VirtualArmor = 18;
		}

		public override Poison PoisonImmune { get { return Poison.Lesser; } }
		public override bool DisallowAllMoves { get { return true; } }

		public Corpser(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				if (0.25 > Utility.RandomDouble())
					PackItem(new Board(10));
				else
					PackItem(new Log(10));

				PackReg(3);
				PackGold(25, 50);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020207054322/uo.stratics.com/hunters/corpser.shtml
					// 0 to 50 Gold, Logs or Boards, Executioner's Cap reagent

					if (Spawning)
					{
						PackGold(0, 50);
					}
					else
					{
						if (0.25 > Utility.RandomDouble())
							PackItem(new Board(10));
						else
							PackItem(new Log(10));

						//	http://uo.stratics.com/content/basics/reagenttome.shtml
						if (0.2 >= Utility.RandomDouble())
							PackItem(new ExecutionersCap());
					}
				}
				else
				{
					if (Spawning)
					{
						if (0.25 > Utility.RandomDouble())
							PackItem(new Board(10));
						else
							PackItem(new Log(10));

						PackItem(new MandrakeRoot(3));
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

			if (BaseSoundID == 352)
				BaseSoundID = 684;
		}
	}
}
