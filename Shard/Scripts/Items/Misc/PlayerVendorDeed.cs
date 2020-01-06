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

/* Items/Misc/PlayerVendorDeed.cs
 * CHANGELOG:
 *  1/14/08, Adam
 *      Add support for Commission based PricingModel
 *	08/03/06, weaver
 *		Allowed placement within siege tents.
 *	05/15/06, weaver
 *		Added check for null region.
 *	05/03/06, weaver
 *		Allowed placement within tents.
 * 	04/15/05, Kitaras
 *		Updated calls to PlayerVendor() to pass a house as well as mobile.
 *	12/12/04, Jade
 *		Unblessed the deeds.
 *	6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server;
using Server.Mobiles;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	public class ContractOfEmployment : Item
	{
		public override int LabelNumber { get { return 1041243; } } // a contract of employment

		[Constructable]
		public ContractOfEmployment()
			: base(0x14F0)
		{
			Weight = 1.0;
			//Jade: make these unblessed
			LootType = LootType.Regular;
		}

		public ContractOfEmployment(Serial serial)
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
			else if (from.AccessLevel >= AccessLevel.GameMaster)
			{
				from.SendLocalizedMessage(503248);//Your godly powers allow you to place this vendor whereever you wish.

				Mobile v = new PlayerVendor(from, BaseHouse.FindHouseAt(from));
				v.Direction = from.Direction & Direction.Mask;
				v.MoveToWorld(from.Location, from.Map);

				v.SayTo(from, 503246); // Ah! it feels good to be working again.

				this.Delete();
			}
			else
			{
				//find the house there at
				BaseHouse house = BaseHouse.FindHouseAt(from);

				// wea: allow placement within tents
				if (house == null)
				{
					if (from.Region != null)
					{
						// is there a tent belonging to the person's account here though?
						if (from.Region is HouseRegion)
						{
							HouseRegion hr = (HouseRegion)from.Region;

							if ((hr.House is Tent || hr.House is SiegeTent) && hr.House.Owner.Account == from.Account)
								house = ((HouseRegion)from.Region).House;

						}
					}
				}

				if (house == null)
				{
					from.SendLocalizedMessage(503240);//Vendors can only be placed in houses.	
				}
				else if (!house.IsFriend(from))
				{
					from.SendLocalizedMessage(503242); //You must ask the owner of this house to make you a friend in order to place this vendor here,
				}
				else if (!house.Public)
				{
					from.SendLocalizedMessage(503241);//You cannot place this vendor.  Make sure the building is public and you have not reached your vendor limit.
				}
				else if (!house.CanPlaceNewVendor())
				{
					from.SendLocalizedMessage(503241); // You cannot place this vendor or barkeep.  Make sure the house is public or a shop and has sufficient storage available.
				}
				//else if (house.FindTownshipNPC() != null)
				else if (house.CanPlacePlayerVendorInThisTownshipHouse() == false)
				{
					from.SendMessage("You cannot place a vendor in a house with the township NPCs present.");
				}
				else
				{
					Mobile v = new PlayerVendor(from, house);
					v.Direction = from.Direction & Direction.Mask;
					v.MoveToWorld(from.Location, from.Map);

					v.SayTo(from, 503246); // Ah! it feels good to be working again.

					this.Delete();
				}
			}
		}
	}
}