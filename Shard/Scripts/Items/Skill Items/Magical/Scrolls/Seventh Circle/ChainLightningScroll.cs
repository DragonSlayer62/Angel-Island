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
using Server;
using Server.Items;

namespace Server.Items
{
	public class ChainLightningScroll : SpellScroll
	{
		public override int LabelNumber { get { return 8029; } } // Chain Lightning Scroll

		[Constructable]
		public ChainLightningScroll()
			: this(1)
		{
		}

		[Constructable]
		public ChainLightningScroll(int amount)
			: base(48, 0x1F5D, amount)
		{
		}

		public ChainLightningScroll(Serial serial)
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

		public override Item Dupe(int amount)
		{
			return base.Dupe(new ChainLightningScroll(amount), amount);
		}
	}
}