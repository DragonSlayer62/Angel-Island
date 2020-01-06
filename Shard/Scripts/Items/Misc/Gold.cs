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

/* Scripts\Items\Misc\Gold.cs
 * ChangeLog
 *  1/8/08, Adam
 *		- Add new OnItemLifted() handler to invoke our WealthTracker system
 *		- InvokeWealthTracker
 */

using System;
using Server.Commands;			// log helper

namespace Server.Items
{
	public class Gold : Item
	{
		[Constructable]
		public Gold()
			: this(1)
		{
		}

		[Constructable]
		public Gold(int amountFrom, int amountTo)
			: this(Utility.Random(amountFrom, amountTo - amountFrom))
		{
		}

		[Constructable]
		public Gold(int amount)
			: base(0xEED)
		{
			Stackable = true;
			Weight = 0.02;
			Amount = amount;
		}

		public Gold(Serial serial)
			: base(serial)
		{
		}

		public override int GetDropSound()
		{
			if (Amount <= 1)
				return 0x2E4;
			else if (Amount <= 5)
				return 0x2E5;
			else
				return 0x2E6;
		}

		public override void OnItemLifted(Mobile from, Item item)
		{
			// see if this item has already been Audited
			if (Audited == false)
			{ // if not track it
				WealthTrackerEventArgs e = new WealthTrackerEventArgs(AuditType.GoldLifted, this, this.Parent, from);
				try
				{
					EventSink.InvokeWealthTracker(e);
				}
				catch (Exception ex)
				{
					LogHelper.LogException(ex);
				}
			}
			base.OnItemLifted(from, item);
		}

		protected override void OnAmountChange(int oldValue)
		{
			TotalGold = (TotalGold - oldValue) + Amount;
		}

		public override void UpdateTotals()
		{
			base.UpdateTotals();

			SetTotalGold(this.Amount);
		}

		public override Item Dupe(int amount)
		{
			return base.Dupe(new Gold(amount), amount);
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