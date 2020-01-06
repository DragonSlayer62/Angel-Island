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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Guardian.cs
 * ChangeLog
 *  8/16/06, Rhiannon
 *		Changed speed settings to match SpeedInfo table.
 */

using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
	public class Guardian : BaseCreature
	{
		[Constructable]
		public Guardian()
			: base(AIType.AI_Archer, FightMode.Aggressor, 10, 1, 0.25, 0.5)
		{
			InitStats(100, 125, 25);
			Title = "the guardian";

			SpeechHue = Utility.RandomDyedHue();

			new ForestOstard().Rider = this;

			Skills[SkillName.Anatomy].Base = 120.0;
			Skills[SkillName.Tactics].Base = 120.0;
			Skills[SkillName.Archery].Base = 120.0;
			Skills[SkillName.MagicResist].Base = 120.0;
			Skills[SkillName.DetectHidden].Base = 100.0;

		}
		public override void InitBody()
		{
			if (Female = Utility.RandomBool())
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
			Hue = Utility.RandomSkinHue();

			PlateChest chest = new PlateChest();
			chest.Hue = 0x966;
			AddItem(chest);
			PlateArms arms = new PlateArms();
			arms.Hue = 0x966;
			AddItem(arms);
			PlateGloves gloves = new PlateGloves();
			gloves.Hue = 0x966;
			AddItem(gloves);
			PlateGorget gorget = new PlateGorget();
			gorget.Hue = 0x966;
			AddItem(gorget);
			PlateLegs legs = new PlateLegs();
			legs.Hue = 0x966;
			AddItem(legs);
			PlateHelm helm = new PlateHelm();
			helm.Hue = 0x966;
			AddItem(helm);

			Bow bow = new Bow();

			bow.Movable = false;
			bow.Crafter = this;
			bow.Quality = WeaponQuality.Exceptional;

			AddItem(bow);

			PackItem(new Arrow(250));
		}

		public Guardian(Serial serial)
			: base(serial)
		{
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				PackGold(250, 500);
			}
			else
			{
				if (Core.UOSP || Core.UOMO)
				{	// none circa 02 2002
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
					PackGold(250, 500);
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