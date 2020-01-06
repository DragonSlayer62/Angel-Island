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

using System;
using Server.Targeting;
using Server.Items;
using Server.Network;

namespace Server.SkillHandlers
{
	public class TasteID
	{
		public static void Initialize()
		{
			SkillInfo.Table[(int)SkillName.TasteID].Callback = new SkillUseCallback(OnUse);
		}

		public static TimeSpan OnUse(Mobile m)
		{
			m.Target = new InternalTarget();

			m.SendLocalizedMessage(502807); // What would you like to taste?

			return TimeSpan.FromSeconds(1.0);
		}

		private class InternalTarget : Target
		{
			public InternalTarget()
				: base(2, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile)
				{
					from.SendLocalizedMessage(502816); // You feel that such an action would be inappropriate
				}
				else if (targeted is Food)
				{
					if (from.CheckTargetSkill(SkillName.TasteID, targeted, 0, 100))
					{
						Food targ = (Food)targeted;

						if (targ.Poison != null)
						{
							from.SendLocalizedMessage(1038284); // It appears to have poison smeared on it
						}
						else
						{
							// No poison on the food
							from.SendLocalizedMessage(502823); // You cannot discern anything about this substance
						}
					}
					else
					{
						// Skill check failed
						from.SendLocalizedMessage(502823); // You cannot discern anything about this substance
					}
				}
				else
				{
					// The target is not food. (Potion support in the next version)
					from.SendLocalizedMessage(502820); // That's not something you can taste
				}
			}
		}
	}
}