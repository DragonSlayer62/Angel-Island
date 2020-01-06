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

/* Items/Misc/HouseSitterDeed.cs
 * CHANGELOG:
 *  11/12/04 - Jade
 *      Change spelling to make housesitter one word.
 *	11/7/04 - Pix
 *		Changed to be Regular loottype.
 *	11/6/04 - Pix
 *		Initial Version
 */

using System;
using Server;
using Server.Mobiles;
using Server.Multis;

namespace Server.Items
{
	public class HouseSitterDeed : Item
	{
		[Constructable]
		public HouseSitterDeed()
			: base(0x14F0)
		{
			Weight = 1.0;
			LootType = LootType.Regular;
			Name = "a housesitter contract";//Jade: change spelling to housesitter.
		}

		public HouseSitterDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); //version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);

				if (house == null)
				{
					from.SendLocalizedMessage(503240);//Vendors can only be placed in houses.	
				}
				else if (!house.IsFriend(from) && (from.AccessLevel < AccessLevel.GameMaster))
				{
					from.SendLocalizedMessage(503242); //You must ask the owner of this house to make you a friend in order to place this vendor here,
				}
				else if (!house.CanPlaceNewVendor())
				{
					from.SendLocalizedMessage(503241); // You cannot place this vendor or barkeep.  Make sure the house is public or a shop and has sufficient storage available.
				}
				else
				{
					Mobile v = new HouseSitter(from);
					v.Direction = from.Direction & Direction.Mask;
					v.MoveToWorld(from.Location, from.Map);

					((HouseSitter)v).SendStatusTo(from);

					this.Delete();
				}
			}
		}
	}
}