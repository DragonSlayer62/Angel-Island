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
using Server.Network;
using Server;

namespace Server.Misc
{
	public class FoodDecayTimer : Timer
	{
		public static void Initialize()
		{
			new FoodDecayTimer().Start();
		}

		public FoodDecayTimer()
			: base(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5))
		{
			Priority = TimerPriority.OneMinute;
		}

		protected override void OnTick()
		{
			FoodDecay();
		}

		public static void FoodDecay()
		{
			foreach (NetState state in NetState.Instances)
			{
				HungerDecay(state.Mobile);
				ThirstDecay(state.Mobile);
			}
		}

		public static void HungerDecay(Mobile m)
		{
			if (m != null && m.Hunger >= 1)
				m.Hunger -= 1;
		}

		public static void ThirstDecay(Mobile m)
		{
			if (m != null && m.Thirst >= 1)
				m.Thirst -= 1;
		}
	}
}