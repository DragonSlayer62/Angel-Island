using System;
using System.Collections.Generic;
using System.Text;
using Server.Spells;

namespace Server.Ethics.Hero
{
	public sealed class HolyWord : Power
	{
		public HolyWord()
		{
			m_Definition = new PowerDefinition(
					100,
					8,
					"Holy Word",
					"Erstok Oostrac",
					""
				);
		}

		public override void BeginInvoke(Player from)
		{
			from.Mobile.BeginTarget(12, true, Targeting.TargetFlags.Harmful, new TargetStateCallback(Power_OnTarget), from);

			// Question(8)[A] on the boards
			from.Mobile.SendMessage("Which being do you wish to destroy?");
		}

		// Question(8)[B] on the boards
		private void Power_OnTarget(Mobile fromMobile, object obj, object state)
		{
			Player from = state as Player;

			Mobile target = obj as Mobile;

			if (target == null || target.Player == false || !target.Evil)
			{	// not sure
				from.Mobile.SendLocalizedMessage(501087);	// This can only be used against your mortal enemies
				return;
			}

			if (!CheckInvoke(from))
				return;

			SpellHelper.Turn(from.Mobile, target);

			DoHolyWord(from.Mobile, target);

			FinishInvoke(from);
		}

		private void DoHolyWord(Mobile from, Mobile combatant)
		{
			Mobile[] buf = new Mobile[2] { from, combatant };
			Timer.DelayCall(TimeSpan.FromSeconds(0.5), new TimerStateCallback(DoHolyWord_Stage1), buf);
		}

		private void DoHolyWord_Stage1(object state)
		{
			Mobile from = ((Mobile[])state)[0] as Mobile;
			Mobile combatant = ((Mobile[])state)[1] as Mobile;

			if (from.CanBeHarmful(combatant))
			{
				from.MovingParticles(combatant, 0x36FA, 1, 0, false, true, 0, 0, 9533, 9534, 0, (EffectLayer)255, 0);
				from.PlaySound(0x1FB);
				Timer.DelayCall(TimeSpan.FromSeconds(1.0), new TimerStateCallback(DoHolyWord_Stage2), state);
			}
		}

		private void DoHolyWord_Stage2(object state)
		{
			Mobile from = ((Mobile[])state)[0] as Mobile;
			Mobile combatant = ((Mobile[])state)[1] as Mobile;

			if (from.CanBeHarmful(combatant))
			{
				from.PlaySound(0x209);
				from.DoHarmful(combatant);

				// death
				AOS.Damage(combatant, from, combatant.Hits, combatant.Hits, 0, 0, 0, 0);
				combatant.Poison = Poison.Lethal;	// die on next tick

				// This power consumes a huge amount of lifeforce, but allows you to target a hero and simply kill them dead. 
				// No sphere nor lifeforce is granted for the kill.
				combatant.ExpirationFlags.Add(new Mobile.ExpirationFlag(from, Mobile.ExpirationFlagID.NoPoints, TimeSpan.FromMinutes(1)));
			}
		}
	}
}

