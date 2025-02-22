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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Brigand.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 *  7/02/06, Kit
 *		InitBody/Outfit additions
 *	7/26/05, erlein
 *		Automated removal of AoS resistance related function calls. 1 lines removed.
 *	4/13/05, Adam
 *		Switch to new region specific loot model
 *	2/4/05, Adam
 *		Hookup PowderOfTranslocation drop rates to the CoreManagementConsole
 *	1/25/05, Adam
 *		Add PowderOfTranslocation as region specific loot 
 *		Brigands are the only ones that carry this loot.
 *	12/11/04, Pix
 *		Changed ControlSlots for IOBF.
 *  11/10/04, Froste
 *      Implemented new random IOB drop system and changed drop change to 12%
 *	11/05/04, Pigpen
 *		Made changes for Implementation of IOBSystem. Changes include:
 *		Removed IsEnemy and Aggressive Action Checks. These are now handled in BaseCreature.cs
 *		Set Creature IOBAlignment to Brigand.
 *	10/1/04, Adam
 *		Change BandageDelay from 10 to 10-13
 *	9/19/04, Adam
 *		1. Brigands are now as tough as savages. Boost stats and skills to match.
 *		2. Give Brigands the ability to heal with bandages (like savages)
 *		3. have brigands rummage corpses (ther're brigands!)
 *		4. Drop Brigand IOB 5% of the time.
 *		5. small gold boost as well
 *  9/16/04, Pigpen
 * 		Added IOB Functionality to items BrigandKinBoots and BrigandKinBandana
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Engines.IOBSystem;

namespace Server.Mobiles
{
	public class Brigand : BaseCreature
	{
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public Brigand()
			: base(AIType.AI_Melee, FightMode.All | FightMode.Closest, 10, 1, 0.25, 0.5)
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "the brigand";
			Hue = Utility.RandomSkinHue();
			IOBAlignment = IOBAlignment.Brigand;
			ControlSlots = 2;

			if (Core.UOAI || Core.UOAR)
			{
				SetStr(96, 115);
				SetDex(86, 105);
				SetInt(51, 65);

				SetDamage(23, 27);

				SetSkill(SkillName.Fencing, 60.0, 82.5);
				SetSkill(SkillName.Macing, 60.0, 82.5);
				SetSkill(SkillName.Poisoning, 60.0, 82.5);
				SetSkill(SkillName.MagicResist, 57.5, 80.0);
				SetSkill(SkillName.Swords, 60.0, 82.5);
				SetSkill(SkillName.Tactics, 60.0, 82.5);
			}
			else
			{
				SetStr(86, 100);
				SetDex(81, 95);
				SetInt(61, 75);

				SetDamage(10, 23);

				SetSkill(SkillName.Fencing, 66.0, 97.5);
				SetSkill(SkillName.Macing, 65.0, 87.5);
				SetSkill(SkillName.MagicResist, 25.0, 47.5);
				SetSkill(SkillName.Swords, 65.0, 87.5);
				SetSkill(SkillName.Tactics, 65.0, 87.5);
				SetSkill(SkillName.Wrestling, 15.0, 37.5);
			}

			InitBody();
			InitOutfit();

			Fame = 1000;
			Karma = -1000;

			PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : false; } }

		public override bool CanBandage { get { return Core.UOAI || Core.UOAR ? true : base.CanBandage; } }
		public override TimeSpan BandageDelay { get { return Core.UOAI || Core.UOAR ? TimeSpan.FromSeconds(Utility.RandomMinMax(10, 13)) : base.BandageDelay; } }

		public Brigand(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
			}
		}
		public override void InitOutfit()
		{
			WipeLayers();
			Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
			hair.Hue = Utility.RandomNondyedHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			if (Female)
				AddItem(new Skirt(Utility.RandomNeutralHue()));
			else
				AddItem(new ShortPants(Utility.RandomNeutralHue()));

			AddItem(new Boots(Utility.RandomNeutralHue()));
			AddItem(new FancyShirt());
			AddItem(new Bandana());

			switch (Utility.Random(7))
			{
				case 0: AddItem(new Longsword()); break;
				case 1: AddItem(new Cutlass()); break;
				case 2: AddItem(new Broadsword()); break;
				case 3: AddItem(new Axe()); break;
				case 4: AddItem(new Club()); break;
				case 5: AddItem(new Dagger()); break;
				case 6: AddItem(new Spear()); break;
			}

		}
		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(100, 150);

				// Froste: 12% random IOB drop
				if (0.12 > Utility.RandomDouble())
				{
					Item iob = Loot.RandomIOB();
					PackItem(iob);
				}

				// if we are in our own stronghold, add 1/3 more gold+
				if (IOBRegions.GetIOBStronghold(this) == IOBAlignment)
				{
					// 30% boost to gold
					PackGold(base.GetGold() / 3);
				}
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// http://web.archive.org/web/20020207053514/uo.stratics.com/hunters/brigand.shtml
					// 100 - 200 Gold, Clothing

					if (Spawning)
					{
						PackGold(100, 200);
					}

					// note: I believe the stratics docs are incomplete here as I'm sure the brigand also drops "weapon carried"
					//	in lieu of new information we will drop this weapon.
				}
				else
				{
					AddLoot(LootPack.Average);
				}
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
