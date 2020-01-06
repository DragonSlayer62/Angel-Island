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

/* Scripts/Mobiles/Monsters/Reptile/Magic/OphidianArchmage.cs
 * ChangeLog
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 6 lines removed.
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
	[CorpseName("an ophidian corpse")]
	[TypeAlias("Server.Mobiles.OphidianJusticar", "Server.Mobiles.OphidianZealot")]
	public class OphidianArchmage : BaseCreature
	{
		private static string[] m_Names = new string[]
			{
				"an ophidian justicar",
				"an ophidian zealot"
			};

		[Constructable]
		public OphidianArchmage()
			: base(AIType.AI_Mage, FightMode.All | FightMode.Closest, 10, 1, 0.2, 0.4)
		{
			Name = m_Names[Utility.Random(m_Names.Length)];
			Body = 85;
			BaseSoundID = 639;

			SetStr(281, 305);
			SetDex(191, 215);
			SetInt(226, 250);

			SetHits(169, 183);
			SetStam(36, 45);

			SetDamage(5, 10);

			SetSkill(SkillName.EvalInt, 95.1, 100.0);
			SetSkill(SkillName.Magery, 95.1, 100.0);
			SetSkill(SkillName.MagicResist, 75.0, 97.5);
			SetSkill(SkillName.Tactics, 65.0, 87.5);
			SetSkill(SkillName.Wrestling, 20.2, 60.0);

			Fame = 11500;
			Karma = -11500;

			VirtualArmor = 44;
		}

		public override int Meat { get { return 1; } }

		public OphidianArchmage(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(190, 220);
				PackScroll(1, 6);
				PackScroll(1, 6);
				PackReg(5, 15);
				// Category 2 MID
				PackMagicItem(1, 1, 0.05);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020214224334/uo.stratics.com/hunters/ophzealot.shtml
					// http://web.archive.org/web/20020202091744/uo.stratics.com/hunters/ophjust.shtml
					// (same creature)
					// 300 Gold, Scrolls.
					if (Spawning)
					{
						PackGold(300);
					}
					else
					{
						PackScroll(1, 6);
						PackScroll(1, 6);
					}
				}
				else
				{	// Standard RunUO
					if (Spawning)
					{
						PackReg(5, 15);

						if (Core.AOS)
							PackNecroReg(5, 15);
					}

					AddLoot(LootPack.Rich);
					AddLoot(LootPack.MedScrolls, 2);
				}
			}
		}

		public override OppositionGroup OppositionGroup
		{
			get { return OppositionGroup.TerathansAndOphidians; }
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
