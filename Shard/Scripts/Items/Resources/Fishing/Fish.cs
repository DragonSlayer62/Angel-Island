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
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
	public class Fish : Item, ICarvable
	{
		public void Carve(Mobile from, Item item)
		{
			base.ScissorHelper(from, new RawFishSteak(), 4);
		}

		[Constructable]
		public Fish()
			: this(1)
		{
		}

		[Constructable]
		public Fish(int amount)
			: base(Utility.Random(0x09CC, 4))
		{
			Stackable = true;
			Weight = 1.0;
			Amount = amount;
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new Fish(amount), amount);
		}

		public Fish(Serial serial)
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
