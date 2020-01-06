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

/* Items/Deeds/RareSpellbookSet1Deed.cs
 * ChangeLog:
 *	4/1/10, adam
 *		Remove LootType = LootType.Blessed; from deserialize
 *		This was preventing the items from dropping via lootpacks
 *	5/14/09, Adam
 *		initial creation
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class RareSpellbookSet1Deed : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public RareSpellbookSet1Deed()
			: base(0x14F0)
		{
			Weight = 1.0;
			Hue = Utility.RandomList(2207, 2425, 2213, 2419);
			Name = "a rare spellbook deed";
			LootType = LootType.Regular;
		}

		public RareSpellbookSet1Deed(Serial serial)
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

			// must be regular to work with LootPacks
			if (LootType != LootType.Regular)
				LootType = LootType.Regular;
		}

		public override bool DisplayLootType { get { return false; } }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack)) // Make sure its in their pack
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				this.Delete();
				from.SendMessage("A rare spellbook has been placed in your backpack.");
				Spellbook book = new Spellbook();	// new spell book
				book.Hue = Hue;						// take the hue of this deed.
				book.Name = "magical spellbook";	// an interesting name
				from.AddToBackpack(book);			// stash it
			}
		}
	}
}


