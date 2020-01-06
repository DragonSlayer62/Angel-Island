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
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Network;

namespace Server.Misc
{
	public class AttackMessage
	{
		private const string AggressorFormat = "You are attacking {0}!";
		private const string AggressedFormat = "{0} is attacking you!";
		private const int Hue = 0x22;

		private static TimeSpan Delay = TimeSpan.FromMinutes(1.0);

		public static void Initialize()
		{
			EventSink.AggressiveAction += new AggressiveActionEventHandler(EventSink_AggressiveAction);
		}

		public static void EventSink_AggressiveAction(AggressiveActionEventArgs e)
		{
			Mobile aggressor = e.Aggressor;
			Mobile aggressed = e.Aggressed;

			if (!aggressor.Player || !aggressed.Player)
				return;

			if (!CheckAggressions(aggressor, aggressed))
			{
				aggressor.LocalOverheadMessage(MessageType.Regular, Hue, true, String.Format(AggressorFormat, aggressed.Name));
				aggressed.LocalOverheadMessage(MessageType.Regular, Hue, true, String.Format(AggressedFormat, aggressor.Name));
			}
		}

		public static bool CheckAggressions(Mobile m1, Mobile m2)
		{
			List<AggressorInfo> list = m1.Aggressors;

			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if (info.Attacker == m2 && DateTime.Now < (info.LastCombatTime + Delay))
					return true;
			}

			list = m2.Aggressors;

			for (int i = 0; i < list.Count; ++i)
			{
				AggressorInfo info = (AggressorInfo)list[i];

				if (info.Attacker == m1 && DateTime.Now < (info.LastCombatTime + Delay))
					return true;
			}

			return false;
		}
	}
}