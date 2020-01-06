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

/* Items/Misc/Stonebag.cs
 * ChangeLog:
 * 4/2/10, adam
 *     Initial Version
 */

using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Targeting;
using System.Collections;
using Server.Multis;

namespace Server.Items
{
	/// <summary>
	/// Summary description for Stonebag.
	/// </summary>
	public class Stonebag : Bag
	{
		[Constructable]
		public Stonebag()
			: base()
		{
			Name = "stonebag";
			Hue = 0x33;
			MaxItems = Utility.RandomMinMax(9, 30);
			LootType = LootType.Newbied;
		}

		public Stonebag(Serial serial)
			: base(serial)
		{
		}

		public override bool CheckHold(Mobile m, Item item, bool message, bool checkItems, int plusItems, int plusWeight)
		{
			if (item is Moonstone == false)
			{
				if (message)
					m.SendMessage("You may only store moonstones in this bag.");

				return false;
			}

			return base.CheckHold(m, item, message, checkItems, plusItems, plusWeight);
		}

		public override bool OnDragDrop(Mobile from, Item item)
		{
			bool bReturn = false;
			if (item is Moonstone)
			{
				bReturn = base.OnDragDrop(from, item);
				if (bReturn)
				{
					// if ok,,,
				}
			}
			else
			{
				from.SendMessage("You may only store moonstones in there.");
				return false;
			}

			return bReturn;
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
