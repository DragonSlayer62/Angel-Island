using System;
using Server;
using Server.Items;

namespace Server.Engines.ResourcePool
{
	/// <summary>
	/// Summary description for PaymentCheck.
	/// </summary>
	public class PaymentCheck : BankCheck
	{
		public override void OnSingleClick(Mobile from)
		{
			from.Send(new Server.Network.AsciiMessage(Serial, ItemID, Server.Network.MessageType.Label, 0x3B2, 3, "", "For the sale of commodities: " + this.Worth)); // A bank check:
		}

		public PaymentCheck(int amount)
			: base(amount)
		{
		}

		public PaymentCheck(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
		}
	}
}
