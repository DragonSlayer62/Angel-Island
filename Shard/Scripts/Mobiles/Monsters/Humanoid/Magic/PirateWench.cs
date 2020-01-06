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

/* Scripts/Mobiles/Monsters/Humanoid/Magic/PirateWench.cs
 * ChangeLog:
 *	7/9/10, adam
 *		o Merge pirate class hierarchy (all pirates are now derived from class Pirate)
 *		o switch over to using AI_HumanMage
 *		o defuff skills a tad since AI_HumanMage mad her much tougher to kill
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 8 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	4/9/05, Adam
 *		Upgrade treasure map level to 3 from 2
 *	1/2/05, Adam
 *		Cleanup pirate name management, make use of Titles
 *			Show title when clicked = true
 *  1/02/05, Jade
 *      Increased speed to bring Pirates up to par with other human IOB kin.
 *	12/30/04 Created by Adam
 */

using System;
using System.Collections;
using Server.Items;
using Server.Targeting;
using Server.Spells;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	[CorpseName("corpse of a salty seadog")]
	public class PirateWench : Pirate
	{
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 3 : 0; } }

		[Constructable]
		public PirateWench()
			: base(AIType.AI_HumanMage)
		{
		}

		public override void InitClass()
		{
			ControlSlots = 3;

			SetStr(171, 200);
			SetDex(126, 145);
			SetInt(276, 305);

			SetHits(103, 120);

			VirtualArmor = 50;

			SetDamage(24, 26);

			SetSkill(SkillName.EvalInt, 70.1, 80.0);
			SetSkill(SkillName.Magery, 70.1, 80.0);
			SetSkill(SkillName.Meditation, 70.1, 80.0);
			SetSkill(SkillName.MagicResist, 70.1, 80.0);
			SetSkill(SkillName.Wrestling, 70.1, 90.0);
			SetSkill(SkillName.Anatomy, 70.1, 90.0);
			SetSkill(SkillName.Healing, 70.1, 90.0);

			Fame = 8000;
			Karma = -8000;

			// we don't use weapons
			FightStyle &= ~FightStyle.Melee;
		}

		public override void InitBody()
		{
			Female = true;
			base.InitBody();
			Title = "the pirate wench";
		}

		public override void InitOutfit()
		{
			base.InitOutfit();
			Item hat = FindItemOnLayer(Layer.Helm);
			if (hat != null)
				hat.Delete();

			AddItem(new SkullCap(Utility.RandomRedHue()));

			Item sword = Weapon as Item;
			if (sword != null)
				sword.Delete();
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackReg(8, 12);
				PackScroll(2, 7);
				PackScroll(2, 7);
				PackMagicEquipment(1, 2, 0.25, 0.25);
				PackMagicEquipment(1, 2, 0.05, 0.05);
				PackGold(170, 220);

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				// pack bulk reg
				PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));

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
				{
					// ai special
				}
			}
		}

		public PirateWench(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			if (base.Version == 0)
				return;

			int version = reader.ReadInt();
		}
	}
}
