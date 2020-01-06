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

/* Scripts/Items/Special/Holiday/Poinsettias.cs
  *
  *	ChangeLog:
  *	12/19/04, Jade
  *     Unbless the item.
  *
  */

using System;
using Server.Items;
using Server.Network;

namespace Server.Items
{
	public class RedPoinsettia : Item
	{
		[Constructable]
		public RedPoinsettia()
			: base(0x2330)
		{
			Weight = 1.0;
			//Jade: Unbless these
			LootType = LootType.Regular;
		}

		public RedPoinsettia(Serial serial)
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

	public class WhitePoinsettia : Item
	{
		[Constructable]
		public WhitePoinsettia()
			: base(0x2331)
		{
			Weight = 1.0;
			//Jade: Unbless these too!
			LootType = LootType.Regular;
		}

		public WhitePoinsettia(Serial serial)
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