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

/* Scripts/Items/TreasureThemes/HangingDragonArmor.cs
 * CHANGELOG
 *	04/07/05, Kitaras	
 *		Initial Creation
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Mobiles;
using Server.Multis;
using Server.Misc;

namespace Server.Items
{
	[Flipable(5095, 5096)]
	public class HangingDragonChest : Item
	{


		[Constructable]
		public HangingDragonChest()
			: base(5095)
		{
			Weight = 20.0;
			Hue = 1645;
			Name = "hanging dragonscale chest";

		}

		public HangingDragonChest(Serial serial)
			: base(serial)
		{
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

	[Flipable(5093, 5094)]
	public class HangingDragonLegs : Item
	{


		[Constructable]
		public HangingDragonLegs()
			: base(5093)
		{
			Weight = 20.0;
			Hue = 1645;
			Name = "hanging dragonscale legs";

		}

		public HangingDragonLegs(Serial serial)
			: base(serial)
		{
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

	[Flipable(5097, 5098)]
	public class HangingDragonArms : Item
	{


		[Constructable]
		public HangingDragonArms()
			: base(5097)
		{
			Weight = 20.0;
			Hue = 1645;
			Name = "hanging dragonscale arms";

		}

		public HangingDragonArms(Serial serial)
			: base(serial)
		{
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
