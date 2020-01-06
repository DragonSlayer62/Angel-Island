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

/* Items/Deeds/TeleporterDeed.cs
 * ChangeLog:
 *	9/13/06, Pix.
 *		Added logic to convert this deed to new teleporter pair addon deed on doubleclick and in backpack
 *  8/27/04, Adam
 *		Created.
 *		Add message when double clicked.
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;

namespace Server.Items
{
	public class TeleporterDeed : Item
	{
		[Constructable]
		public TeleporterDeed()
			: base(0x14F0)
		{
			base.Weight = 1.0;
			base.Name = "a teleporter deed";
		}

		public TeleporterDeed(Serial serial)
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

		public override void OnDoubleClick(Mobile from)
		{
			//from.SendMessage( "Please page a GM for Teleporter instalation." );
			if (this.IsChildOf(from.Backpack))
			{
				from.AddToBackpack(new TeleporterAddonDeed());
				this.Delete();
				from.SendMessage("Your old teleporter deed has been converted to a new teleporter deed.");
			}
			else
			{
				from.SendMessage("This must be in your backpack to use.");
			}
		}
	}
}


