using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Ethics.Evil
{
	public sealed class UnholyShield : Power
	{
		public UnholyShield()
		{
			m_Definition = new PowerDefinition(
					20,
					7,
					"Unholy Shield",
					"Velgo K'blac",
					""
				);
		}

		public override void BeginInvoke(Player from)
		{
			if (Core.NewEthics)
			{
				if (from.IsShielded)
				{
					from.Mobile.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, false, "You are already under the protection of an unholy shield.");
					return;
				}

				from.BeginShield();

				from.Mobile.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, false, "You are now under the protection of an unholy shield.");
			}
			else
			{
				if (from.Mobile.CheckState(Mobile.ExpirationFlagID.MonsterIgnore))
				{
					//from.Mobile.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, false, "You are already under the protection of an unholy shield.");
					return;
				}

				// Question(9) on the boards - how long?
				// Answered: "Use this ability to make monsters ignore you for 1 hour."
				from.Mobile.ExpirationFlags.Add(new Mobile.ExpirationFlag(from.Mobile, Mobile.ExpirationFlagID.MonsterIgnore, TimeSpan.FromMinutes(60)));
			}

			FinishInvoke(from);
		}
	}
}
