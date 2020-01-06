using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Ethics
{
	public abstract class Power
	{
		protected PowerDefinition m_Definition;

		public PowerDefinition Definition { get { return m_Definition; } }

		public virtual bool CheckInvoke(Player from)
		{
			if (!from.Mobile.CheckAlive())
				return false;

			if (from.Power < m_Definition.Power)
			{	// from ciloc-1.txt
				from.Mobile.SendLocalizedMessage(501074); // You lack the necessary life force.
				return false;
			}

			// question(1): from message boards - ok
			// question(2): from message boards - ok
			if (m_Definition.Sphere > from.Power / 10)
			{	// from ciloc-1.txt
				from.Mobile.SendLocalizedMessage(501073); // This power is beyond your ability.
				return false;
			}

			return true;
		}

		public abstract void BeginInvoke(Player from);

		public virtual void FinishInvoke(Player from)
		{
			from.Power -= m_Definition.Power;
		}
	}
}