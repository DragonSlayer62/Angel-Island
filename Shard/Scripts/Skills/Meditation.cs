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

/* Scripts\Skills\Meditation.cs
 * ChangeLog:
 * 12/26/07, Pix
 *      Now you can hold a bounty ledger and meditate.
 *	7/26/05, Adam
 *		Massive AOS cleanout
 *	2/16/05, Adam
 *		Convert casts to using the 'as' operator and remove check for .ShieldArmor in CheckMeddableArmor()
 *  9/3/04, Pix
 *		Now when trying to actively meditate in non-meddable armor, meditation fails 
 *		with the message "Regenerative forces cannot penetrate your armor."
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using Server.Items;

namespace Server.SkillHandlers
{
	class Meditation
	{
		public static void Initialize()
		{
			SkillInfo.Table[46].Callback = new SkillUseCallback(OnUse);
		}

		public static bool CheckOkayHolding(Item item)
		{
			if (item == null)
				return true;

			if (item is Spellbook || item is Runebook || item is BountyLedger)
				return true;

			//if ( Core.AOS && item is BaseWeapon && ((BaseWeapon)item).Attributes.SpellChanneling != 0 )
			//return true;

			//if ( Core.AOS && item is BaseArmor && ((BaseArmor)item).Attributes.SpellChanneling != 0 )
			//return true;

			return false;
		}

		public static bool IsMeddableArmor(BaseArmor ba)
		{
			try
			{
				if (ba == null)
				{
					return true;
				}

				if (ba.MeditationAllowance == ArmorMeditationAllowance.None)
				{
					return false;
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }

			return true;
		}

		public static bool CheckMeddableArmor(Mobile m)
		{
			if (!IsMeddableArmor(m.NeckArmor as BaseArmor) ||
				!IsMeddableArmor(m.HandArmor as BaseArmor) ||
				!IsMeddableArmor(m.HeadArmor as BaseArmor) ||
				!IsMeddableArmor(m.ArmsArmor as BaseArmor) ||
				!IsMeddableArmor(m.LegsArmor as BaseArmor) ||
				!IsMeddableArmor(m.ChestArmor as BaseArmor))
			{
				return false;
			}

			return true;
		}


		public static TimeSpan OnUse(Mobile m)
		{
			m.RevealingAction();

			if (m.Target != null)
			{
				m.SendLocalizedMessage(501845); // You are busy doing something else and cannot focus.

				return TimeSpan.FromSeconds(5.0);
			}
			else if (m.Hits < (m.HitsMax / 10)) // Less than 10% health
			{
				m.SendLocalizedMessage(501849); // The mind is strong but the body is weak.

				return TimeSpan.FromSeconds(5.0);
			}
			else if (m.Mana >= m.ManaMax)
			{
				m.SendLocalizedMessage(501846); // You are at peace.

				return TimeSpan.FromSeconds(5.0);
			}
			else
			{
				Item oneHanded = m.FindItemOnLayer(Layer.OneHanded);
				Item twoHanded = m.FindItemOnLayer(Layer.TwoHanded);

				if (Core.AOS)
				{
					if (!CheckOkayHolding(oneHanded))
						m.AddToBackpack(oneHanded);

					if (!CheckOkayHolding(twoHanded))
						m.AddToBackpack(twoHanded);
				}
				else if (!CheckOkayHolding(oneHanded) || !CheckOkayHolding(twoHanded))
				{
					m.SendLocalizedMessage(502626); // Your hands must be free to cast spells or meditate.

					return TimeSpan.FromSeconds(2.5);
				}
				else if (!CheckMeddableArmor(m))
				{
					m.SendMessage("Regenerative forces cannot penetrate your armor.");

					return TimeSpan.FromSeconds(2.5);
				}

				if (m.CheckSkill(SkillName.Meditation, 0, 100))
				{
					m.SendLocalizedMessage(501851); // You enter a meditative trance.
					m.Meditating = true;

					if (m.Player || m.Body.IsHuman)
						m.PlaySound(0xF9);
				}
				else
				{
					m.SendLocalizedMessage(501850); // You cannot focus your concentration.
				}

				return TimeSpan.FromSeconds(10.0);
			}
		}
	}
}