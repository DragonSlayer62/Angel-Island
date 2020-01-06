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

/* Change Log
 *
 * 1/11/05, Darva
 *		Made magic unlock work on all magic locked chests.
 */
using System;
using Server.Targeting;
using Server.Network;
using Server.Items;

namespace Server.Spells.Third
{
	public class UnlockSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Unlock Spell", "Ex Por",
				SpellCircle.Third,
				215,
				9001,
				Reagent.Bloodmoss,
				Reagent.SulfurousAsh
			);

		public UnlockSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public void Target(LockableContainer targ)
		{
			if (Multis.BaseHouse.CheckSecured(targ))
			{
				// You cannot cast this on a secure item.
				Caster.SendLocalizedMessage(503098);
			}
			else if (CheckSequence())
			{
				SpellHelper.Turn(Caster, targ);

				Point3D loc = targ.GetWorldLocation();

				Effects.SendLocationParticles(
					EffectItem.Create(loc, targ.Map, EffectItem.DefaultDuration),
					0x376A, 9, 32, 5024);

				Effects.PlaySound(loc, targ.Map, 0x1FF);

				if (targ.Locked && targ.LockLevel != 0)
				{
					double magery = Caster.Skills[SkillName.Magery].Value;
					int level = (int)(magery * 0.8) - 4;

					if (targ.LockLevel == -255)
					{
						targ.Locked = false;
						FinishSequence();
						return;
					}
					if (level >= targ.RequiredSkill && !(targ is TreasureMapChest && ((TreasureMapChest)targ).Level > 2))
					{
						targ.Locked = false;

						if (targ.LockLevel == 0)
							targ.LockLevel = -1;
					}
					else
					{
						// My spell does not seem to have an effect on that lock.
						Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503099);
					}
				}
				else
				{
					// That did not need to be unlocked.
					Caster.LocalOverheadMessage(MessageType.Regular, 0x3B2, 503101);
				}
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private UnlockSpell m_Owner;

			public InternalTarget(UnlockSpell owner)
				: base(12, false, TargetFlags.None)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is LockableContainer)
					m_Owner.Target((LockableContainer)o);
				else
					from.SendLocalizedMessage(501666); // You can't unlock that!

				// TODO: Really we can cast on anything, even mobiles, but it will effect and say 'That did not need to be unlocked'
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}