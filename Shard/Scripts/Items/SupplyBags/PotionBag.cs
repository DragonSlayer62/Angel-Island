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

/* Scripts\Items\SupplyBags\PotionBag.cs
 * ChangeLog
 *	4/4/08, Adam
 *		first time checkin
 */

using System;
using Server;
using Server.Items;

namespace Server.Items
{
	public class PotionBag : Backpack
	{
		[Constructable]
		public PotionBag()
			: this(1)
		{
			Movable = true;
			Hue = 0x979;
			Name = "a Potion Bag";
		}
		[Constructable]
		public PotionBag(int amount)
		{
			// Begin bag of potion kegs
			this.Name = "Various Potion Kegs";
			PlaceItemIn(this, 45, 149, MakePotionKeg(PotionEffect.CureGreater, 0x2D));
			PlaceItemIn(this, 69, 149, MakePotionKeg(PotionEffect.HealGreater, 0x499));
			PlaceItemIn(this, 93, 149, MakePotionKeg(PotionEffect.PoisonDeadly, 0x46));
			PlaceItemIn(this, 117, 149, MakePotionKeg(PotionEffect.RefreshTotal, 0x21));
			PlaceItemIn(this, 141, 149, MakePotionKeg(PotionEffect.ExplosionGreater, 0x74));
			PlaceItemIn(this, 93, 82, new Bottle(1000));
		}

		public PotionBag(Serial serial)
			: base(serial)
		{
		}

		private void PlaceItemIn(Container parent, int x, int y, Item item)
		{
			parent.AddItem(item);
			item.Location = new Point3D(x, y, 0);
		}

		private Item MakePotionKeg(PotionEffect type, int hue)
		{
			PotionKeg keg = new PotionKeg();

			keg.Held = 100;
			keg.Type = type;
			keg.Hue = hue;

			return keg;
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
