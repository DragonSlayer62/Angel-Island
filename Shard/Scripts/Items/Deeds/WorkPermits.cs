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

/* Items/Deeds/WorkPermits.cs
 * ChangeLog:
 *	4/2/08, Adam
 *      first time checkin
 */

using System;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Targeting;
using Server.Multis;        // HouseSign
using Server.Commands;			// log helper

namespace Server.Items
{
	public abstract class BaseWorkPermit : Item
	{
		public BaseWorkPermit()
			: base(0x14F0)
		{
			Weight = 1.0;
			LootType = LootType.Regular;
		}

		public BaseWorkPermit(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteInt32((int)0);       // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt32();
			switch (version)
			{
				case 0:
					{
						break;
					}
			}
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
				from.SendMessage("Please target the house sign of the house to apply permit to.");
				from.Target = new WorkPermitTarget(this); // Call our target
			}
		}
	}

	public class WorkPermitTarget : Target
	{
		private BaseWorkPermit m_Deed;

		public WorkPermitTarget(BaseWorkPermit deed)
			: base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		bool UpgradeCheck(Mobile from, BaseHouse house)
		{
			// see if the upgrade is really an *upgrade*
			//  handle the special case where the user has added taxable lockbox storage below
			if (house.MaximumBarkeepCount >= 255)
			{
				from.SendMessage("Fire regulations prohibit allowing any more barkeeps to work at this residence.");
				return false;
			}

			return true;
		}

		protected override void OnTarget(Mobile from, object target) // Override the protected OnTarget() for our feature
		{
			if (target is HouseSign && (target as HouseSign).Structure != null)
			{
				HouseSign sign = target as HouseSign;
				LogHelper Logger = null;

				try
				{
					if (sign.Structure.IsFriend(from) == false)
					{
						from.SendLocalizedMessage(502094); // You must be in your house to do this.
						return;
					}
					else if (UpgradeCheck(from, (target as HouseSign).Structure) == false)
					{
						// filters out any oddball cases and askes the user to correct it
					}
					else
					{
						BaseHouse house = (target as HouseSign).Structure;
						Logger = new LogHelper("WorkPermit.log", false);
						Logger.Log(LogType.Item, house, String.Format("WorkPermit applied: {0}", m_Deed.ToString()));
						house.MaximumBarkeepCount++;
						from.SendMessage(String.Format("Permit Accepted. You may now employ up to {0} barkeepers.", house.MaximumBarkeepCount));
						m_Deed.Delete();
					}
				}
				catch (Exception ex)
				{
					LogHelper.LogException(ex);
				}
				finally
				{
					if (Logger != null)
						Logger.Finish();
				}
			}
			else
			{
				from.SendMessage("That is not a house sign.");
			}
		}
	}

	public class BarkeepWorkPermit : BaseWorkPermit
	{
		[Constructable]
		public BarkeepWorkPermit()
		{
			Name = "work permit for a barkeep";
		}

		public BarkeepWorkPermit(Serial serial)
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

	}
}


