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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/PirateDeckHand.cs	
 * ChangeLog:
 *	7/9/10, adam
 *		o Merge pirate class hierarchy (all pirates are now derived from class Pirate)
 *		o Remove old RunUO Heal-with-bandages model and use new style which uses real bandages
 *		o Replace AI with new AI_HumanMelee AI .. allows healing with bandages, potions and potion buffs
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 1 lines removed.
 *	4/13/05, Kit
 *		Switch to new region specific loot model
 *	1/2/05, Adam
 *		Cleanup pirate name management, make use of Titles
 *			Show title when clicked = false
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
	public class PirateDeckHand : Pirate
	{
		public override int TreasureMapLevel { get { return Core.UOAI || Core.UOAR ? 2 : 0; } }

		[Constructable]
		public PirateDeckHand()
			: base(AIType.AI_HumanMelee)
		{
		}

		public override void InitClass()
		{
			ControlSlots = 2;

			SetStr(96, 115);
			SetDex(86, 105);
			SetInt(51, 65);

			SetDamage(23, 27);

			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 57.5, 80.0);
			SetSkill(SkillName.Healing, 60.0, 82.5);
			SetSkill(SkillName.Anatomy, 60.0, 82.5);

			Fame = 1000;
			Karma = -1000;

			// weapons only
			FightStyle = FightStyle.Melee;
		}

		public override void InitBody()
		{
			base.InitBody();
			Title = "the deckhand";
		}
		public override void InitOutfit()
		{
			base.InitOutfit();
			Item hat = FindItemOnLayer(Layer.Helm);
			if (hat != null)
				hat.Delete();

			AddItem(new SkullCap(Utility.RandomRedHue()));
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGem();
				PackMagicEquipment(1, 3);
				PackGold(100, 150);

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

		public PirateDeckHand(Serial serial)
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
