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

/* Scripts/Spells/Eighth/Resurrection.cs
 * ChangeLog
 *	11/27/07, Adam
 *		Replace the simple 18 unit z order check with the new Items.Bandage.ProximityCheck(). 
 *		This function (amoung other things) checks that the PATH to the patient is not blocked.
 *		This test prevents various in-house resurect-in-a-locked/blocked-room exploits.
 *	3/17/05, Adam
 *		Add a 18 unit Z order check for healing.
 *		This fixes the "resurrect on patio" exploit.
 */

using System;
using Server.Targeting;
using Server.Network;
using Server.Gumps;

namespace Server.Spells.Eighth
{
	public class ResurrectionSpell : Spell
	{
		private static SpellInfo m_Info = new SpellInfo(
				"Resurrection", "An Corp",
				SpellCircle.Eighth,
				245,
				9062,
				Reagent.Bloodmoss,
				Reagent.Garlic,
				Reagent.Ginseng
			);

		public ResurrectionSpell(Mobile caster, Item scroll)
			: base(caster, scroll, m_Info)
		{
		}

		public override void OnCast()
		{
			Caster.Target = new InternalTarget(this);
		}

		public void Target(Mobile m)
		{
			if (!Caster.CanSee(m))
			{
				Caster.SendLocalizedMessage(500237); // Target can not be seen.
			}
			else if (m == Caster)
			{
				Caster.SendLocalizedMessage(501039); // Thou can not resurrect thyself.
			}
			else if (!Caster.Alive)
			{
				Caster.SendLocalizedMessage(501040); // The resurrecter must be alive.
			}
			else if (m.Alive)
			{
				Caster.SendLocalizedMessage(501041); // Target is not dead.
			}
			else if (!Caster.InRange(m, 1))
			{
				Caster.SendLocalizedMessage(501043); // Target is not close enough.
			}
			else if (!m.Player)
			{
				Caster.SendLocalizedMessage(501043); // Target is not a being.
			}
			else if (m.Map == null || !m.Map.CanFit(m.Location, 16, CanFitFlags.requireSurface))
			{
				Caster.SendLocalizedMessage(501042); // Target can not be resurrected at that location.
				m.SendLocalizedMessage(502391); // Thou can not be resurrected there!
			}
			//else if ( Math.Abs(m.Z - Caster.Z) > 18 )
			else if (Items.Bandage.ProximityCheck(Caster, m, 1) == false)
			{
				Caster.SendLocalizedMessage(501043); // Target is not close enough.
			}
			else if (CheckBSequence(m, true))
			{
				SpellHelper.Turn(Caster, m);

				m.PlaySound(0x214);
				m.FixedEffect(0x376A, 10, 16);

				m.SendGump(new ResurrectGump(m, Caster));
			}

			FinishSequence();
		}

		private class InternalTarget : Target
		{
			private ResurrectionSpell m_Owner;

			public InternalTarget(ResurrectionSpell owner)
				: base(1, false, TargetFlags.Beneficial)
			{
				m_Owner = owner;
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is Mobile)
				{
					m_Owner.Target((Mobile)o);
				}
			}

			protected override void OnTargetFinish(Mobile from)
			{
				m_Owner.FinishSequence();
			}
		}
	}
}