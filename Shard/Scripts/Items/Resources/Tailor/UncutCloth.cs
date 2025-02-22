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
using Server.Items;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0x1765, 0x1767)]
	public class UncutCloth : Item, IScissorable, IDyable, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} piece of cloth" : "{0} pieces of cloth", Amount);
			}
		}

		[Constructable]
		public UncutCloth()
			: this(1)
		{
		}

		[Constructable]
		public UncutCloth(int amount)
			: base(0x1767)
		{
			Stackable = true;
			Weight = 0.1;
			Amount = amount;
		}

		public UncutCloth(Serial serial)
			: base(serial)
		{
		}

		public bool Dye(Mobile from, DyeTub sender)
		{
			if (Deleted)
				return false;

			Hue = sender.DyedHue;

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

		public override void OnSingleClick(Mobile from)
		{
			int number = (Amount == 1) ? 1049124 : 1049123;

			from.Send(new MessageLocalized(Serial, ItemID, MessageType.Regular, 0x3B2, 3, number, "", Amount.ToString()));
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new UncutCloth(), amount);
		}

		public bool Scissor(Mobile from, Scissors scissors)
		{
			if (Deleted || !from.CanSee(this)) return false;

			base.ScissorHelper(from, new Bandage(), 1);

			return true;
		}
	}
}