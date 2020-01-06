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

namespace Server.Items
{
	public class ShipwreckedItem : Item, IDyable
	{
		public ShipwreckedItem(int itemID)
			: base(itemID)
		{
			int weight = this.ItemData.Weight;

			if (weight >= 255)
				weight = 1;

			this.Weight = weight;
		}

		public override void OnSingleClick(Mobile from)
		{
			this.LabelTo(from, 1050039, String.Format("#{0}\t#1041645", LabelNumber));
		}

		public override void AddNameProperties(ObjectPropertyList list)
		{
			base.AddNameProperties(list);
			list.Add(1041645); // recovered from a shipwreck
		}

		public ShipwreckedItem(Serial serial)
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

		public bool Dye(Mobile from, DyeTub sender)
		{
			if (Deleted)
				return false;

			if (ItemID >= 0x13A4 && ItemID <= 0x13AE)
			{
				Hue = sender.DyedHue;
				return true;
			}

			from.SendLocalizedMessage(sender.FailMessage);
			return false;
		}
	}
}