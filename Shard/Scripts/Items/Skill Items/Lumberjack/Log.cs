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

namespace Server.Items
{
	[FlipableAttribute(0x1bdd, 0x1be0)]
	public class Log : Item, ICommodity
	{
		string ICommodity.Description
		{
			get
			{
				return String.Format(Amount == 1 ? "{0} log" : "{0} logs", Amount);
			}
		}

		[Constructable]
		public Log()
			: this(1)
		{
		}

		[Constructable]
		public Log(int amount)
			: base(0x1BDD)
		{
			Stackable = true;
			Weight = 2.0;
			Amount = amount;
		}

		public Log(Serial serial)
			: base(serial)
		{
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new Log(amount), amount);
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