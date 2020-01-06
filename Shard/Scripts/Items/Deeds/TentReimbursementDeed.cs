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

/* Items/Deeds/TentReimbursementDeed.cs
 * ChangeLog:
 *	2/27/10, Adam
 *		Created.
 */

using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Commands;			// log helper

namespace Server.Items
{
	public class TentReimbursementDeed : Item
	{
		BaseContainer m_container;					// the id of the container (for debug reasons)
		int m_ContainerID;							// saved serial number of above container (should container get deleted)

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseContainer Container
		{
			get { return m_container; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Serial ContainerID
		{
			get { return m_ContainerID; }
		}

		public TentReimbursementDeed(BaseContainer c)
			: base(0x14F0)
		{
			base.Weight = 1.0;
			base.LootType = LootType.Newbied;						// newbie shouldn't loose this
			base.Name = "tent reimbursement deed";
			m_container = c;										// the storage container
			m_ContainerID = (int)m_container.Serial;				// identifies the prize
			LogHelper Logger = new LogHelper("TentReimbursementDeed.log", false);
			string temp = String.Format("A Tent Reimbursement Deed ({0}) has been created.", this.Serial);
			Logger.Log(LogType.Item, m_container, temp);
			Logger.Finish();
		}

		public TentReimbursementDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_container);
			writer.Write(m_ContainerID);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_container = reader.ReadItem() as BaseContainer;
			m_ContainerID = reader.ReadInt();

			if (m_container == null)
			{
				LogHelper Logger = new LogHelper("TentReimbursementDeed.log", false);
				string temp;
				temp = String.Format("Orphaned Tent Reimbursement Deed({0}) loaded for nonexistent Backpack(0x{1:X}).", this.Serial, m_ContainerID);
				Logger.Log(LogType.Text, temp);
				Logger.Finish();
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			base.OnSingleClick(from);

			string text = null;
			text = "Double click to recover your possessions.";
			this.LabelTo(from, text);
		}

		public override void OnDoubleClick(Mobile from)
		{
			// must not be locked down
			if (this.IsLockedDown == true || this.IsSecure == true)
			{
				from.SendMessage("That is locked down.");
				return;
			}

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
				return;
			}

			LogHelper Logger = new LogHelper("TentReimbursementDeed.log", false);

			if (m_container != null && m_container.Deleted == false)
			{
				m_container.MoveToWorld(from.Location, from.Map);		// move the the map
				from.SendMessage("Your possessions have been returned.");
				string temp = String.Format("Mobile({0}) using Deed({1}) has recovered their possessions.", from.Serial, this.Serial);
				Logger.Log(LogType.Item, m_container, temp);
			}
			else
			{
				from.SendMessage("There was a problem recovering your possessions.");
				string temp = String.Format("Tent doods missing for Mobile({0}) using Deed({1}) on Backpack(0x{2:X}).", from.Serial, this.Serial, m_ContainerID);
				Logger.Log(LogType.Text, temp);
			}

			// cleanup
			Logger.Finish();
			this.Delete();
		}
	}

}


