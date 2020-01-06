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

/* Scripts/Misc/Fastwalk.cs
 * CHANGELOG
 *
 *  02/05/07 Taran Kain
 *		Added a bit of flexibility and logging capabilities.
 */

using System;
using System.Collections.Generic;
using Server;
using Server.Commands;

namespace Server.Misc
{
	// This fastwalk detection is no longer required
	// As of B36 PlayerMobile implements movement packet throttling which more reliably controls movement speeds
	public class Fastwalk
	{
		public static bool ProtectionEnabled = false;
		public static int WarningThreshold = 4;
		public static TimeSpan WarningCooldown = TimeSpan.FromSeconds(0.4);

		private static Dictionary<Mobile, List<DateTime>> m_Blocks = new Dictionary<Mobile, List<DateTime>>();

		public static void Initialize()
		{
			EventSink.FastWalk += new FastWalkEventHandler(OnFastWalk);
		}

		public static void OnFastWalk(FastWalkEventArgs e)
		{
			if (!m_Blocks.ContainsKey(e.NetState.Mobile))
			{
				m_Blocks.Add(e.NetState.Mobile, new List<DateTime>());
			}
			m_Blocks[e.NetState.Mobile].Add(DateTime.Now);

			if (ProtectionEnabled)
			{
				e.Blocked = true;//disallow this fastwalk
				//Console.WriteLine("Client: {0}: Fast movement detected (name={1})", e.NetState, e.NetState.Mobile.Name);
			}

			try
			{
				List<DateTime> blocks = m_Blocks[e.NetState.Mobile];
				if (e.FastWalkCount > WarningThreshold &&
					blocks.Count >= 2 && // sanity check, shouldn't be possible to reach this point w/o Count >= 2
					(blocks[blocks.Count - 1] - blocks[blocks.Count - 2]) > WarningCooldown)
				{
					Console.WriteLine("FW Warning");
				}
			}
			catch (Exception ex) // we can only exception if Mobile.FwdMaxSteps < 2 - make sure SecurityManagementConsole doesn't set it too low
			{
				LogHelper.LogException(ex);
				Console.WriteLine(ex);
			}
		}
	}
}
