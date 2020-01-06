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
using Server.Spells;

namespace Server.Items
{
	public class NecromancerSpellbook : Spellbook
	{
		public override SpellbookType SpellbookType { get { return SpellbookType.Necromancer; } }
		public override int BookOffset { get { return 100; } }
		public override int BookCount { get { return 16; } }

		public override Item Dupe(int amount)
		{
			Spellbook book = new NecromancerSpellbook();

			book.Content = this.Content;

			return base.Dupe(book, amount);
		}

		[Constructable]
		public NecromancerSpellbook()
			: this((ulong)0)
		{
		}

		[Constructable]
		public NecromancerSpellbook(ulong content)
			: base(content, 0x2253)
		{
			Layer = Layer.Invalid;
		}

		public NecromancerSpellbook(Serial serial)
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
			Layer = Layer.Invalid;
		}
	}
}