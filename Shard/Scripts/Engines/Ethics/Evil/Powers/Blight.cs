using System;
using System.Collections.Generic;
using System.Text;
using Server.Spells;

namespace Server.Ethics.Evil
{
	public sealed class Blight : Power
	{
		public Blight()
		{
			m_Definition = new PowerDefinition(
					15,
					6,
					"Blight",
					"Velgo Ontawl",
					""
				);
		}

		public override void BeginInvoke(Player from)
		{
			from.Mobile.BeginTarget(12, true, Targeting.TargetFlags.None, new TargetStateCallback(Power_OnTarget), from);
			from.Mobile.SendMessage("Where do you wish to blight?");
		}

		private void Power_OnTarget(Mobile fromMobile, object obj, object state)
		{
			Player from = state as Player;

			IPoint3D p = obj as IPoint3D;

			if (p == null)
				return;

			if (!CheckInvoke(from))
				return;

			bool powerFunctioned = false;

			SpellHelper.GetSurfaceTop(ref p);

			foreach (Mobile mob in from.Mobile.GetMobilesInRange(6))
			{
				if (mob == from.Mobile || !SpellHelper.ValidIndirectTarget(from.Mobile, mob))
					continue;

				if (mob.GetStatMod("Holy Curse") != null && Core.NewEthics)
					continue;

				if (!from.Mobile.CanBeHarmful(mob, false))
					continue;

				if (!mob.Hero && Core.OldEthics)
					continue;

				from.Mobile.DoHarmful(mob, true);

				if (Core.OldEthics)
				{
					// Blight	Velgo Ontawl	
					// This targetable area effect power causes damage to all heroes in the range. Damage done lies in the 20-40 range.
					Effects.SendMovingEffect(from.Mobile, mob, 0x36E4, 7, 0, false, true, 33, 0);
					mob.PlaySound(0x210);
					int damage = Utility.Random(20,40);
					mob.Damage(damage,from.Mobile);
				}
				else
				{
					mob.AddStatMod(new StatMod(StatType.All, "Holy Curse", -10, TimeSpan.FromMinutes(30.0)));
					mob.FixedParticles(0x374A, 10, 15, 5028, EffectLayer.Waist);
					mob.PlaySound(0x1FB);
				}

					powerFunctioned = true;
			}

			if (powerFunctioned)
			{
				SpellHelper.Turn(from.Mobile, p);

				if (Core.OldEthics)
					Effects.PlaySound(p, from.Mobile.Map, 0x210);
				else
					Effects.PlaySound(p, from.Mobile.Map, 0x1FB);

				from.Mobile.LocalOverheadMessage(Server.Network.MessageType.Regular, 0x3B2, false, "You curse the area.");

				FinishInvoke(from);
			}
			else
			{
				from.Mobile.FixedEffect(0x3735, 6, 30);
				from.Mobile.PlaySound(0x5C);
			}
		}
	}
}
