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

/* Scripts/Mobiles/Monsters/Humanoid/Melee/Caveman.cs
 * ChangeLog
 *  7/02/06, Kit
 *		InitBody/InitOutfit additions
 *	1/6/06, Pix
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
	public class Caveman : BaseCreature
	{
		[Constructable]
		public Caveman()
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();
			Hue = 33770;	// best looking color - tribe should be same hue
			IOBAlignment = IOBAlignment.None;
			ControlSlots = 2;

			SetStr(96, 115);
			SetDex(86, 105);
			SetInt(51, 65);

			SetDamage(23, 27);

			SetSkill(SkillName.Fencing, 80.0, 92.5);		// not used
			SetSkill(SkillName.Macing, 80.0, 92.5);
			SetSkill(SkillName.Tactics, 80.0, 92.5);
			SetSkill(SkillName.Wrestling, 99.9, 130.5);	// for the womenz!
			SetSkill(SkillName.Poisoning, 60.0, 82.5);	// not used
			SetSkill(SkillName.MagicResist, 77.5, 96.0);	// not used

			Fame = 0;
			Karma = 0;

			InitBody();
			InitOutfit();
			PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

		}

		public override bool AlwaysAttackable { get { return true; } }
		public override bool ShowFameTitle { get { return false; } }
		public override bool CanRummageCorpses { get { return Core.UOAI || Core.UOAR ? true : false; } }
		public override bool ClickTitle { get { return false; } }
		public override bool CanBandage { get { return true; } }
		public override TimeSpan BandageDelay { get { return TimeSpan.FromSeconds(Utility.RandomMinMax(13, 16)); } }

		public Caveman(Serial serial)
			: base(serial)
		{
		}

		public override void InitBody()
		{
			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Title = "the cavewoman";
				Name = NameList.RandomName("caveman_female");

			}
			else
			{
				Body = 0x190;
				Title = "the caveman";
				Name = NameList.RandomName("caveman_male");
			}
		}
		public override void InitOutfit()
		{
			WipeLayers();
			if (this.Female)
			{

				AddItem(new Kilt(Utility.RandomList(1863, 1836, 1133, 1141)));
				int hairHue = Utility.RandomList(1140, 1108, 1140, 1147, 1149, 1175, 2412);
				Item hair = new LongHair();
				hair.Hue = hairHue;
				hair.Layer = Layer.Hair;
				hair.Movable = false;
				AddItem(hair);
			}
			else
			{
				AddItem(new Kilt(Utility.RandomList(1863, 1836, 1133, 1141)));
				int hairHue = Utility.RandomList(1140, 1108, 1140, 1147, 1149, 1175, 2412);
				Item hair = new LongHair();
				hair.Hue = hairHue;
				hair.Layer = Layer.Hair;
				hair.Movable = false;
				AddItem(hair);

				// MediumLongBeard seems to be a long beard + Mustache
				Item beard = new MediumLongBeard();
				beard.Hue = hairHue;
				beard.Movable = false;
				beard.Layer = Layer.FacialHair;
				AddItem(beard);

				// Adam: I'll probably get in trouble for this :0
				if (this.Female == false)
				{
					// 10% staff, 90% club
					if (Utility.RandomDouble() < 0.10)
						AddItem(new GnarledStaff());
					else
						AddItem(new Club());
				}
			}
		}

		public override void GenerateLoot()
		{
			if (Core.UOAI || Core.UOAR)
			{
				if (this.Female == true)
					PackGold(60, 80);		// women have no weapons
				else
					PackGold(100, 150);

				switch (Utility.Random(3))
				{
					case 0: PackItem(new Fish()); break;
					case 1: AddItem(new ChickenLeg()); break;
					case 2: AddItem(new CookedBird()); break;
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
