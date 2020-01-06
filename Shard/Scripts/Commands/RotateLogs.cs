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

/* Scripts/Commands/RotateLogs.cs
 * 	CHANGELOG:
 *	11/14/06, Adam
 *		Adjust rollover tag to be seconds since 1/1/2000
 * 	11/3/06, Adam
 *		Initial Version
 */

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Commands
{
	public class RotateLogs
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("RotateLogs", AccessLevel.Administrator, new CommandEventHandler(RotateLogs_OnCommand));
		}

		[Usage("RotateLogs")]
		[Description("Rotate player command logs.")]
		public static void RotateLogs_OnCommand(CommandEventArgs e)
		{
			try
			{
				RotateNow();
				e.Mobile.SendMessage("Log rotation complete.");
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
				Console.WriteLine(ex.ToString());
				e.Mobile.SendMessage("Log rotation failed.");
			}
		}

		public static void RotateNow()
		{
			try
			{
				// close the open logfile
				CommandLogging.Close();

				string root = Path.Combine(Core.BaseDirectory, "Logs");

				if (!Directory.Exists(root))
					Directory.CreateDirectory(root);

				string[] existing = Directory.GetDirectories(root);

				DirectoryInfo dir;

				// rename the commands directory with a date-time stamp
				dir = Match(existing, "Commands");
				if (dir != null)
				{
					TimeSpan tx = DateTime.Now - new DateTime(2000, 1, 1);
					string ToName = String.Format("{0}, {1:X}", DateTime.Now.ToLongDateString(), (int)tx.TotalSeconds);
					try { dir.MoveTo(FormatDirectory(root, ToName, "")); }
					catch (Exception ex)
					{
						LogHelper.LogException(ex);
						Console.WriteLine("Failed to move to {0}", FormatDirectory(root, ToName, ""));
						Console.WriteLine(ex.ToString());
						throw (ex);
					}
				}

				// reopen the logfile
				CommandLogging.Open();
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
				throw (ex);
			}
		}

		private static string FormatDirectory(string root, string name, string timeStamp)
		{
			return Path.Combine(root, String.Format("{0}", name));
		}

		private static DirectoryInfo Match(string[] paths, string match)
		{
			for (int i = 0; i < paths.Length; ++i)
			{
				DirectoryInfo info = new DirectoryInfo(paths[i]);

				if (info.Name.StartsWith(match))
					return info;
			}

			return null;
		}
	}
}