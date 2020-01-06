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

/* Scripts/Items/Containers/UnknownSkeletons.cs
 * CHANGELOG:
 *	12/28/10, Adam
 *		first time check-in
 *		from:
 *		http://code.google.com/p/runuomondains/source/browse/trunk/Scripts/Items/Containers/UnknownSkeletons.cs?spec=svn121&r=121
 */

using System;
using Server;

namespace Server.Items
{
	public class UnknownBardSkeleton : BaseContainer
	{
		public override int DefaultGumpID { get { return 0x9; } }

		[Constructable]
		public UnknownBardSkeleton()
			: base(0xECA + Utility.Random(9))
		{
			Name = "An Unknown Bard's Skeleton";
			Weight = 35.0;

			DropItem(new Gold(Utility.RandomMinMax(200, 400)));
			DropItem(new Doublet(Utility.RandomNondyedHue()));
			DropItem(new JesterHat(Utility.RandomNondyedHue()));
			DropItem(new Bandage(Utility.RandomMinMax(10, 20)));

			switch (Utility.Random(2))
			{
				case 0: DropItem(new Kilt(Utility.RandomNondyedHue())); break;
				case 1: DropItem(new ShortPants(Utility.RandomNondyedHue())); break;
			}

			switch (Utility.Random(3))
			{
				case 0: DropItem(new BeverageBottle(BeverageType.Ale)); break;
				case 1: DropItem(new BeverageBottle(BeverageType.Wine)); break;
				case 2: DropItem(new BeverageBottle(BeverageType.Liquor)); break;
			}

			DropItem(Loot.RandomInstrument());
		}

		public UnknownBardSkeleton(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}

	public class UnknownRogueSkeleton : BaseContainer
	{
		public override int DefaultGumpID { get { return 0x9; } }

		[Constructable]
		public UnknownRogueSkeleton()
			: base(0xECA + Utility.Random(9))
		{
			Name = "An Unknown Rogue's Skeleton";
			Weight = 35.0;

			DropItem(new LeatherChest());
			DropItem(new LeatherGloves());
			DropItem(new LeatherArms());
			DropItem(new Dagger());
			DropItem(new Shovel(50));
			DropItem(new Lockpick(Utility.RandomMinMax(1, 4)));

			if (Utility.RandomBool())
				DropItem(new Torch());
			else
				DropItem(new Lantern());

			if (0.1 >= Utility.RandomDouble())
				DropItem(Loot.RandomRangedWeapon());
			else
				DropItem(Loot.RandomWeapon());

			DropItem(new TreasureMap(Utility.RandomMinMax(3, 5), Map.Felucca));
		}

		public UnknownRogueSkeleton(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}

	public class UnknownMageSkeleton : BaseContainer
	{
		public override int DefaultGumpID { get { return 0x9; } }

		[Constructable]
		public UnknownMageSkeleton()
			: base(0xECA + Utility.Random(9))
		{
			Name = "An Unknown Mage's Skeleton";
			Weight = 35.0;

			DropItem(new Robe(Utility.RandomNondyedHue()));
			DropItem(new Sandals());
			DropItem(Loot.RandomJewelry());

			if (Utility.RandomBool())
				DropItem(new QuarterStaff());
			else
				DropItem(new GnarledStaff());

			Item item;

			for (int i = 0; i < 3; i++)
			{
				item = Loot.RandomReagent();
				item.Amount = Utility.RandomMinMax(15, 20);
				DropItem(item);
			}

			for (int i = 0; i < 3; i++)
			{
				if (0.25 >= Utility.RandomDouble() && Core.AOS)
					item = Loot.RandomScroll(0, Loot.NecromancyScrollTypes.Length, SpellbookType.Necromancer);
				else
					item = Loot.RandomScroll(0, Loot.RegularScrollTypes.Length, SpellbookType.Regular);

				item.Amount = Utility.RandomMinMax(1, 2);
				DropItem(item);
			}
		}

		public UnknownMageSkeleton(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();
		}
	}
}