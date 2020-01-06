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

/* Items/Deeds/StorageTaxCredits.cs
 * ChangeLog:
 *	05/5/07, Adam
 *      first time checkin
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Multis;        // HouseSign

namespace Server.Items
{
	public class StorageTaxCredits : Item // Create the item class which is derived from the base item class
	{
		private ushort m_Credits;
		public ushort Credits
		{
			get
			{
				return m_Credits;
			}
		}

		[Constructable]
		public StorageTaxCredits()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "tax credits: storage";
			LootType = LootType.Regular;

			// 30 credits: Cost is 1K each and decays at 1 per day
			m_Credits = 30 * 24;
		}

		public StorageTaxCredits(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteInt32((int)0); // version

			writer.Write(m_Credits);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt32();

			m_Credits = reader.ReadUShort();
		}

		public override bool DisplayLootType { get { return false; } }

		public override void OnDoubleClick(Mobile from)
		{
			if (from.Backpack == null || !IsChildOf(from.Backpack)) // Make sure its in their pack
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else
			{
				from.SendMessage("Please target the house sign of the house to apply credits to.");
				from.Target = new StorageTaxCreditsTarget(this); // Call our target
			}
		}
	}

	public class StorageTaxCreditsTarget : Target
	{
		private StorageTaxCredits m_Deed;

		public StorageTaxCreditsTarget(StorageTaxCredits deed)
			: base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
		{
			if (target is HouseSign && (target as HouseSign).Structure != null)
			{
				HouseSign sign = target as HouseSign;

				if (sign.Structure.IsFriend(from) == false)
				{
					from.SendLocalizedMessage(502094); // You must be in your house to do this.
					return;
				}

				if (sign.Structure.CanAddStorageCredits(m_Deed.Credits) == false)
				{
					from.SendMessage("That house cannot hold more credits.");
					return;
				}

				sign.Structure.StorageTaxCredits += (uint)m_Deed.Credits;
				from.SendMessage("Your total storage credits are {0}.", sign.Structure.StorageTaxCredits);
				m_Deed.Delete(); // Delete the deed                
			}
			else
			{
				from.SendMessage("That is not a house sign.");
			}
		}
	}
}


