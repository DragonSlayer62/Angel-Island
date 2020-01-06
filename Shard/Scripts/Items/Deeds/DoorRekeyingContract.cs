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

/* Items/Deeds/DoorRekeyingContract.cs
 * ChangeLog:
 *  5/29/07, Adam
 *      Remove unused Credits property
 *	05/22/07, Adam
 *      first time checkin
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Multis;

namespace Server.Items
{
	public class DoorRekeyingContract : Item // Create the item class which is derived from the base item class
	{
		[Constructable]
		public DoorRekeyingContract()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "a contract for door rekeying";
			LootType = LootType.Regular;
		}

		public DoorRekeyingContract(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteInt32((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt32();
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
				from.SendMessage("Please target the house door you wish to rekey.");
				from.Target = new DoorRekeyingContractTarget(this); // Call our target
			}
		}
	}

	public class DoorRekeyingContractTarget : Target
	{
		private DoorRekeyingContract m_Deed;

		public DoorRekeyingContractTarget(DoorRekeyingContract deed)
			: base(2, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
		{
			if (target is BaseDoor)
			{
				BaseDoor door = target as BaseDoor;
				BaseHouse h1 = BaseHouse.FindHouseAt(door);
				BaseHouse h2 = BaseHouse.FindHouseAt(from);
				if (h1 == null || h1 != h2)
				{
					from.SendLocalizedMessage(502094); // You must be in your house to do this.
					return;
				}
				else if (h1.IsOwner(from) == false)
				{
					from.SendLocalizedMessage(501303); // Only the house owner may change the house locks.
					return;
				}

				// don't remove old keys because you will endup removing the main house keys
				//  we need to single this door out somehow
				// Key.RemoveKeys( from, oldKeyValue );

				// make the keys
				uint keyValue = Key.RandomValue();
				Key packKey = new Key(KeyType.Gold);
				Key bankKey = new Key(KeyType.Gold);
				packKey.KeyValue = keyValue;
				bankKey.KeyValue = keyValue;
				BankBox box = from.BankBox;
				if (box == null || !box.TryDropItem(from, bankKey, false))
					bankKey.Delete();
				from.AddToBackpack(packKey);

				// rekey door
				door.KeyValue = keyValue;

				from.SendMessage("The lock on this door has been changed, and new master key has been placed in your bank and your backpack.");
				m_Deed.Delete(); // Delete the deed                
			}
			else
			{
				from.SendMessage("That is not a door.");
			}
		}
	}
}


