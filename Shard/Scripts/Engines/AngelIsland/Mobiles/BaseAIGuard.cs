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

/* Scripts/Engines/AngelIsland/BaseAIGuard.cs
 *	Created 4/3/04 by mith
 *	ChangeLog
 *	5/17/04, mith
 *		Cleaned up the way we delete armor in OnBeforeDeath().
 *	4/10/04 changes by mith
 *		Added chance to get a bow along with the other weapons. 
 *		Changed Mace to WarMace.
 *	4/7/04, changes by mith
 *		Changed FightMode to Aggressor instead of None.
 *		Removed tunic/doublet/body sash creation so these items don't drop to corpse.
 *		Replaced halberd with Broadsword so they don't deal quite so much damage as quick.
 */
using System;
using System.Collections;
using Server.Misc;
using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Mobiles
{
	public class BaseAIGuard : BaseCreature
	{
		[Constructable]
		public BaseAIGuard()
			: base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.6, 0.8)
		{
			InitStats(400, 100, 100);
			Title = "the guard";

			SpeechHue = Utility.RandomDyedHue();

			Hue = Utility.RandomSkinHue();

			if (Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");

				switch (Utility.Random(2))
				{
					case 0: AddItem(new LeatherSkirt()); break;
					case 1: AddItem(new LeatherShorts()); break;
				}

				switch (Utility.Random(5))
				{
					case 0: AddItem(new FemaleLeatherChest()); break;
					case 1: AddItem(new FemaleStuddedChest()); break;
					case 2: AddItem(new LeatherBustierArms()); break;
					case 3: AddItem(new StuddedBustierArms()); break;
					case 4: AddItem(new FemalePlateChest()); break;
				}
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");

				AddItem(new PlateChest());
				AddItem(new PlateArms());
				AddItem(new PlateGorget());
				AddItem(new PlateLegs());
			}

			Item hair = new Item(Utility.RandomList(0x203B, 0x203C, 0x203D, 0x2044, 0x2045, 0x2047, 0x2049, 0x204A));

			hair.Hue = Utility.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;

			AddItem(hair);

			if (Utility.RandomBool() && !this.Female)
			{
				Item beard = new Item(Utility.RandomList(0x203E, 0x203F, 0x2040, 0x2041, 0x204B, 0x204C, 0x204D));

				beard.Hue = hair.Hue;
				beard.Layer = Layer.FacialHair;
				beard.Movable = false;

				AddItem(beard);
			}

			Broadsword weapon = new Broadsword();

			weapon.Movable = false;
			weapon.Crafter = this;
			weapon.Quality = WeaponQuality.Exceptional;

			AddItem(weapon);

			Container pack = new Backpack();

			pack.Movable = false;

			pack.DropItem(new Gold(10, 25));

			AddItem(pack);

			SetSkill(SkillName.Anatomy, 70.1, 80.0);
			SetSkill(SkillName.Tactics, 70.1, 80.0);
			SetSkill(SkillName.Swords, 70.1, 80.0);
			SetSkill(SkillName.MagicResist, 70.1, 80.0);
		}

		public BaseAIGuard(Serial serial)
			: base(serial)
		{
		}

		public override bool OnBeforeDeath()
		{
			ArrayList items = new ArrayList(this.Items);

			foreach (Item i in items)
			{
				if (i is BaseArmor || i is BaseClothing)
					i.Delete();
			}

			return base.OnBeforeDeath();
		}

		public void DropItem(Item item)
		{
			if (Summoned || item == null)
			{
				if (item != null)
					item.Delete();

				return;
			}

			Container pack = Backpack;

			if (pack == null)
			{
				pack = new Backpack();

				pack.Movable = false;

				AddItem(pack);
			}

			pack.DropItem(item);
		}

		public bool DropWeapon(int minLevel, int maxLevel)
		{
			if (1.0 <= Utility.RandomDouble())
				return false;

			if (maxLevel > 2)
				maxLevel = 2;

			Cap(ref minLevel, 0, 2);
			Cap(ref maxLevel, 0, 2);

			BaseWeapon weapon = new Broadsword();

			double random = Utility.RandomDouble();
			if (random >= .75)
			{ weapon = new WarFork(); }
			else if (random >= .50)
			{ weapon = new WarMace(); }
			else if (random >= .25)
			{
				weapon = new Bow();
				Arrow arrows = new Arrow();
				arrows.Amount = 25;
				DropItem(arrows);
			}

			if (weapon == null)
				return false;

			weapon.DamageLevel = (WeaponDamageLevel)RandomMinMaxScaled(minLevel, maxLevel);
			weapon.AccuracyLevel = (WeaponAccuracyLevel)RandomMinMaxScaled(0, maxLevel);
			weapon.DurabilityLevel = (WeaponDurabilityLevel)RandomMinMaxScaled(0, maxLevel);

			DropItem(weapon);

			return true;
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