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

/* Items/Deeds/ClothingBlessDeed.cs
 * ChangeLog:
 *	02/25/05, Adam
 *		remove references to 'Insured' (no more insurance)
 *		reuse the  flag as 'PlayerCrafted' (item.cs)
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;

namespace Server.Items
{
	public class ClothingBlessTarget : Target // Create our targeting class (which we derive from the base target class)
	{
		private ClothingBlessDeed m_Deed;

		public ClothingBlessTarget(ClothingBlessDeed deed)
			: base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
		{
			if (target is BaseClothing)
			{
				Item item = (Item)target;

				if (item.LootType == LootType.Blessed || item.BlessedFor == from /*|| (Mobile.InsuranceEnabled && item.Insured)*/ ) // Check if its already newbied (blessed)
				{
					from.SendLocalizedMessage(1045113); // That item is already blessed
				}
				else if (item.LootType != LootType.Regular)
				{
					from.SendLocalizedMessage(1045114); // You can not bless that item
				}
				else
				{
					if (item.RootParent != from) // Make sure its in their pack or they are wearing it
					{
						from.SendLocalizedMessage(500509); // You cannot bless that object
					}
					else
					{
						item.LootType = LootType.Blessed;
						from.SendLocalizedMessage(1010026); // You bless the item....

						m_Deed.Delete(); // Delete the bless deed
					}
				}
			}
			else
			{
				from.SendLocalizedMessage(500509); // You cannot bless that object
			}
		}
	}

	public class ClothingBlessDeed : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public ClothingBlessDeed()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "a clothing bless deed";
			LootType = LootType.Blessed;
		}

		public ClothingBlessDeed(Serial serial)
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
			LootType = LootType.Blessed;

			int version = reader.ReadInt();
		}

		public override bool DisplayLootType { get { return false; } }

		public override void OnDoubleClick(Mobile from) // Override double click of the deed to call our target
		{
			if (!IsChildOf(from.Backpack)) // Make sure its in their pack
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendLocalizedMessage(1005018); // What item would you like to bless? (Clothes Only)
				from.Target = new ClothingBlessTarget(this); // Call our target
			}
		}
	}
}


