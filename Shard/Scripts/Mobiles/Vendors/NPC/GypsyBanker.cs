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

using System;
using System.Collections;
using Server;
using Server.Gumps;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.ContextMenus;

namespace Server.Mobiles
{
	public class GypsyBanker : Banker
	{
		public override bool IsActiveVendor { get { return false; } }
		public override NpcGuild NpcGuild { get { return NpcGuild.None; } }
		public override bool ClickTitle { get { return false; } }

		[Constructable]
		public GypsyBanker()
		{
			Title = "the gypsy banker";
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			switch (Utility.Random(4))
			{
				case 0: AddItem(new JesterHat(RandomBrightHue())); break;
				case 1: AddItem(new Bandana(RandomBrightHue())); break;
				case 2: AddItem(new SkullCap(RandomBrightHue())); break;
			}

			Item item = FindItemOnLayer(Layer.Pants);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.Shoes);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.OuterLegs);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.InnerLegs);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.OuterTorso);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.InnerTorso);

			if (item != null)
				item.Hue = RandomBrightHue();

			item = FindItemOnLayer(Layer.Shirt);

			if (item != null)
				item.Hue = RandomBrightHue();
		}

		public GypsyBanker(Serial serial)
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