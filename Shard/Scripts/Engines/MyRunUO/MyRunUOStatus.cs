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
using Server.Commands;

namespace Server.Engines.MyRunUO
{
	public class MyRunUOStatus
	{
		public static void Initialize()
		{
			if (Config.Enabled)
			{
				Timer.DelayCall(TimeSpan.FromSeconds(20.0), Config.StatusUpdateInterval, new TimerCallback(Begin));

				CommandSystem.Register("UpdateWebStatus", AccessLevel.Administrator, new CommandEventHandler(UpdateWebStatus_OnCommand));
			}
		}

		[Usage("UpdateWebStatus")]
		[Description("Starts the process of updating the MyRunUO online status database.")]
		public static void UpdateWebStatus_OnCommand(CommandEventArgs e)
		{
			if (m_Command == null || m_Command.HasCompleted)
			{
				Begin();
				e.Mobile.SendMessage("Web status update process has been started.");
			}
			else
			{
				e.Mobile.SendMessage("Web status database is already being updated.");
			}
		}

		private static DatabaseCommandQueue m_Command;

		public static void Begin()
		{
			if (m_Command != null && !m_Command.HasCompleted)
				return;

			DateTime start = DateTime.Now;
			Console.WriteLine("MyRunUO: Updating status database");

			try
			{
				m_Command = new DatabaseCommandQueue("MyRunUO: Status database updated in {0:F1} seconds", "MyRunUO Status Database Thread");

				m_Command.Enqueue("DELETE FROM myrunuo_status");

				List<NetState> online = NetState.Instances;

				for (int i = 0; i < online.Count; ++i)
				{
					NetState ns = online[i];
					Mobile mob = ns.Mobile;

					if (mob != null)
						m_Command.Enqueue(String.Format("INSERT INTO myrunuo_status VALUES ({0})", mob.Serial.Value.ToString()));
				}
			}
			catch (Exception e)
			{
				LogHelper.LogException(e);
				Console.WriteLine("MyRunUO: Error updating status database");
				Console.WriteLine(e);
			}

			if (m_Command != null)
				m_Command.Enqueue(null);
		}
	}
}
