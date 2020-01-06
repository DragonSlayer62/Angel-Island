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

/* Scripts\Engines\ChampionSpawn\Champs\Seasonal\Summer\Mobiles\WalkingDead.cs
 *	ChangeLog
 *	3/12/08, Adam
 *		initial creation
 *		based on Scripts/Mobiles/Monsters/Humanoid/Melee/Vampire.cs
 */

using System;
using System.Collections;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Commands;

namespace Server.Mobiles
{
	public class WalkingDead : Vampire
	{
		// not as strong as vampires
		public override int LifeDrain { get { return base.LifeDrain / 2; } }
		public override int StamDrain { get { return base.StamDrain / 2; } }

		[Constructable]
		public WalkingDead()
			: base()
		{
			FlyArray = FlyTiles; //assign to mobile fly array for movement code to use.
			BardImmune = true;

			SpeechHue = 0x21;
			Hue = 0;
			HueMod = 0;

			// vamp stats
			SetStr(200 / 2, 300 / 2);	// 1/2 the STR of a Vampire
			SetDex(105, 135);
			SetInt(80, 105);
			SetHits(140, 176);
			SetDamage(1, 5); // all damage is via life drain

			VirtualArmor = 20;

			CoreVampSkills();
			SetSkill(SkillName.Swords, 86.0, 100.0);	// for bucher knife

			Fame = 10000;
			Karma = 0;

			InitBody();
			InitOutfit();

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

			Title = "the walking dead";
		}
		public override void InitOutfit()
		{
			WipeLayers();

			// black backpack. we need a backpack so our walking-dead can be disarmed, and black is cool
			Shaft WoodenStake = new Shaft();
			WoodenStake.Hue = 51;
			WoodenStake.Name = "wooden stake";
			PackItem(WoodenStake);
			Backpack.Hue = 0x01;

			if (Utility.RandomBool())
				AddItem(new Cleaver());
			else
				AddItem(new ButcherKnife());

			// walking dead are naked
			if (this.Female)
			{

				Item hair = new Item(0x203C);
				if (Utility.RandomMinMax(0, 100) <= 20) //20% chance to have black hair
				{
					hair.Hue = 0x1;
				}
				else
					hair.Hue = Utility.RandomHairHue();

				hair.Layer = Layer.Hair;
				AddItem(hair);
			}
			else
			{
				Item hair2 = new Item(Utility.RandomList(0x203C, 0x203B));
				hair2.Hue = Utility.RandomHairHue();
				hair2.Layer = Layer.Hair;
				AddItem(hair2);
			}
		}

		public WalkingDead(Serial serial)
			: base(serial)
		{
		}


		public override void GenerateLoot()
		{
			PackGold(170 / 2, 220 / 2); //add gold if its daytime - 1/2 of vamipres
			Item blood = new BloodVial();
			blood.Name = "blood of " + this.Name;
			PackItem(blood);

		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version
			//writer.Write(BatForm); // version 1
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					// remove batform bool from serialization
					break;
				case 0:
					bool dmy = reader.ReadBool();
					break;
			}

		}
	}
}
