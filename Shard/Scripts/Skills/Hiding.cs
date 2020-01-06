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


/* Scripts\Skills\Hiding.cs
 *	ChangeLog :
 *	07/23/08, weaver
 *		Automated IPooledEnumerable optimizations. 1 loops updated.
 */

using System;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Multis;

namespace Server.SkillHandlers
{
	public class Hiding
	{
		public static void Initialize()
		{
			SkillInfo.Table[21].Callback = new SkillUseCallback(OnUse);
		}

		public static TimeSpan OnUse(Mobile m)
		{
			if (m.Target != null || m.Spell != null)
			{
				m.SendLocalizedMessage(501238); // You are busy doing something else and cannot hide.
				return TimeSpan.FromSeconds(1.0);
			}


			double bonus = 0.0;

			BaseHouse house = BaseHouse.FindHouseAt(m);

			if (house != null && house.IsFriend(m))
			{
				bonus = 100.0;
			}
			else if (!Core.AOS)
			{
				if (house == null)
					house = BaseHouse.FindHouseAt(new Point3D(m.X - 1, m.Y, 127), m.Map, 16);

				if (house == null)
					house = BaseHouse.FindHouseAt(new Point3D(m.X + 1, m.Y, 127), m.Map, 16);

				if (house == null)
					house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y - 1, 127), m.Map, 16);

				if (house == null)
					house = BaseHouse.FindHouseAt(new Point3D(m.X, m.Y + 1, 127), m.Map, 16);

				if (house != null)
					bonus = 50.0;
			}

			int range = 18 - (int)(m.Skills[SkillName.Hiding].Value / 10);

			bool badCombat = (m.Combatant != null && m.InRange(m.Combatant.Location, range) && m.Combatant.InLOS(m));
			bool ok = (!badCombat /*&& m.CheckSkill( SkillName.Hiding, 0.0 - bonus, 100.0 - bonus )*/ );

			if (ok)
			{
				IPooledEnumerable eable = m.GetMobilesInRange(range);
				foreach (Mobile check in eable)
				{
					if (check.InLOS(m) && check.Combatant == m)
					{
						badCombat = true;
						ok = false;
						break;
					}
				}
				eable.Free();

				ok = (!badCombat && m.CheckSkill(SkillName.Hiding, 0.0 - bonus, 100.0 - bonus));
			}

			if (badCombat)
			{
				m.RevealingAction();

				m.LocalOverheadMessage(MessageType.Regular, 0x22, 501237); // You can't seem to hide right now.

				return TimeSpan.FromSeconds(1.0);
			}
			else if (m.CheckState(Mobile.ExpirationFlagID.EvilCrim))
			{	// Evils that kill innocents are flagged with a special criminal flag the prevents them from gate/hide
				
				m.RevealingAction();

				// question(6)
				m.LocalOverheadMessage(MessageType.Regular, 0x22, 501237); // You can't seem to hide right now.

				return TimeSpan.FromSeconds(1.0);
			}
			else
			{
				if (ok)
				{
					m.Hidden = true;

					// Publish 15
					// Players who successfully use their hiding skill while under the effects of an invisibility spell will no longer be revealed when the invisibility timer expires.
					if (Core.Publish >= 15 || Core.UOAI || Core.UOAR || Core.UOMO)
						Spells.Sixth.InvisibilitySpell.RemoveTimer(m);

					m.LocalOverheadMessage(MessageType.Regular, 0x1F4, 501240); // You have hidden yourself well.
				}
				else
				{
					m.RevealingAction();

					m.LocalOverheadMessage(MessageType.Regular, 0x22, 501241); // You can't seem to hide here.
				}

				return TimeSpan.FromSeconds(10.0);
			}
		}
	}
}
