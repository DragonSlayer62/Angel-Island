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

/* Scripts/Commands/Lag.cs
 * ChangeLog
 *	3/267/08, Adam
 *		Switch from logging last 5 Heartbeat tasks to logging Scheduled tasks
 *			Heartbeat is now obsolete.
 *  11/28/06, plasma
 *      Added last 5 heartbeat tasks to log
 *	8/13/06, Pix
 *		Added console log at Adam's request.
 * 	08/3/06, weaver
 *		Initial creation.
*/
using System;
using Server;
using Server.Mobiles;

namespace Server.Commands
{
	public class LagReport
	{

		public static void Initialize()
		{
			Server.CommandSystem.Register("Lag", AccessLevel.Player, new CommandEventHandler(LagReport_OnCommand));
		}

		[Usage("Lag")]
		[Description("Reports lag to the administrators")]
		private static void LagReport_OnCommand(CommandEventArgs arg)
		{
			Mobile from = arg.Mobile;

			if (from is PlayerMobile)
			{
				PlayerMobile pm = (PlayerMobile)from;

				// Limit to 5 minutes between lag reports
				if ((pm.LastLagTime + TimeSpan.FromMinutes(5.0)) < DateTime.Now)
				{
					// Let them log again
					LogHelper lh = new LogHelper("lagreports.log", false, true);
					lh.Log(LogType.Mobile, from, Server.Engines.CronScheduler.Cron.GetRecentTasks()); //adam: added schduled tasks!

					//Requested by Adam:
					Console.WriteLine("Lag at: {0}", DateTime.Now.ToShortTimeString());

					// Update LastLagTime on PlayerMobile
					pm.LastLagTime = DateTime.Now;

					lh.Finish();

					from.SendMessage("The fact that you are experiencing lag has been logged. We will review this with other data to try and determine the cause of this lag. Thank you for your help.");
				}
				else
				{

					from.SendMessage("It has been less than five minutes since you last reported lag. Please wait five minutes between submitting lag reports.");
				}

			}

		}

	}

}
