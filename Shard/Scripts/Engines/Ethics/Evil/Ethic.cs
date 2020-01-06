using System;
using System.Collections.Generic;
using System.Text;
using Server.Factions;

namespace Server.Ethics.Evil
{
	public sealed class EvilEthic : Ethic
	{
		public EvilEthic()
		{
			m_Definition = new EthicDefinition(
					0x455,
					"Evil", "(Evil)",
					"I am evil incarnate",
					new Power[]
					{
						new UnholySense(),
						new UnholyItem(),
						new SummonFamiliar(),
						new VileBlade(),
						new Blight(),
						new UnholyShield(),
						new UnholySteed(),
						new UnholyWord()
					}
				);
		}

		public override bool IsEligible(Mobile mob)
		{
			if (Core.NewEthics)
			{	// must be part of a faction
				Faction fac = Faction.Find(mob);
				return (fac is Minax || fac is Shadowlords);
			}
			else
			{
				if ((mob.CreationTime + TimeSpan.FromHours(24)) > DateTime.Now)
					return false;

				return true;
			}
		}
	}
}