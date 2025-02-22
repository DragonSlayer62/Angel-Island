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

/* Scripts/Commands/Maintenance.cs
 * CHANGELOG
 *	12/23/08, Adam
 *		Update to use RunUO 2.0 restart model
 *	5/7/08, Adam
 *		echo countdown to the console
 *  11/13/06, Kit
 *      Added in Rebuild option, shutdown server, merge cvs, and restart server.
 *	6/2/06, Adam
 *		Initial Revision.
 *		Shuts the server down or restarts it with a minutely message of the form:
 *		"Server restarting in N minutes for maintenance..."
 */

using System;
using System.IO;
using System.Diagnostics;
using Server;
using Server.Commands;

namespace Server.Misc
{
	public class Maintenance
	{
		private static bool m_Shutdown;						// shutdown or restart?
		private static bool m_Rebuild;                      // rebuild cvs directory?
		private static bool m_Scheduled = false;			// is maintenance scheduled?
		private static int m_Countdown = 5;					// our countdown
		private static MaintenanceTimer m_Maintenance;		// our timer
		private static int m_RebuildID;                     // process id of rebuild.exe
		private static DateTime m_When;

		public static bool Shutdown
		{
			get { return m_Shutdown; }
			set { m_Shutdown = value; }
		}

		public static bool Scheduled
		{
			get { return m_Scheduled; }
			set { m_Scheduled = value; }
		}

		public static int Countdown
		{
			get { return m_Countdown; }
			set { m_Countdown = value; }
		}

		public static bool Rebuild
		{
			get { return m_Rebuild; }
			set { m_Rebuild = value; }
		}

		public static int RebuildProcess
		{
			get { return m_RebuildID; }
			set { m_RebuildID = value; }
		}

		public static void Initialize()
		{
			CommandSystem.Register("Maintenance", AccessLevel.Administrator, new CommandEventHandler(Maintenance_OnCommand));
			m_Maintenance = new MaintenanceTimer();
		}

		public static void Maintenance_OnCommand(CommandEventArgs e)
		{
			try
			{
				if (e.Length == 0)
				{
					Usage(e);
				}
				else
				{
					string strParam = e.GetString(0);
					
					// default is 5 minutes from now
					m_When = DateTime.Now + TimeSpan.FromMinutes(5);

					if (e.Length > 1)
					{	// we have a date-time param
						string sx = null;
						for (int ix = 1; ix < e.Length; ix++)
							sx += e.GetString(ix) + " ";
						try { m_When = DateTime.Parse(sx); }
						catch 
						{
							e.Mobile.SendMessage("Bad date format.");
							e.Mobile.SendMessage("Maintenance not initiated.");
							Usage(e);
							return;
						}

						TimeSpan diff = m_When.Subtract(DateTime.Now);
						m_Countdown = (int)diff.TotalMinutes;
					}

					if (strParam.ToLower().Equals("cancel"))
					{
						m_Scheduled = false;
						if (m_Rebuild)
						{
							if (KillRebuild())
							{
								e.Mobile.SendMessage("Rebuild.exe canceled succesfully.");
								Rebuild = false;
							}
							else
								e.Mobile.SendMessage("Error closeing rebuild.exe!!!");
						}
						AutoSave.SavesEnabled = true;
						e.Mobile.SendMessage("Maintenance has been canceled.");
						World.Broadcast(0x482, true, "Server maintenance has been canceled.");
						m_Maintenance.Stop();
					}
					else if (strParam.ToLower().Equals("rebuild"))
					{
						if (Rebuild)
						{
							e.Mobile.SendMessage("The server is already prepareing for a rebuild.");
						}
						else
						{
							Rebuild = true;
							Shutdown = true;
							Scheduled = true;
							AutoSave.SavesEnabled = false;
							e.Mobile.SendMessage("You have initiated a server rebuild.");
							m_Maintenance.Start();

							if (!StartRebuild(Misc.TestCenter.Enabled))
							{
								e.Mobile.SendMessage("Rebuild.exe failed to start, canceling rebuild.");
								Rebuild = false;
								Scheduled = false;
							}
						}
					}
					else if (strParam.ToLower().Equals("restart") || strParam.ToLower().Equals("shutdown"))
					{
						if (m_Scheduled)
						{
							e.Mobile.SendMessage("The server is already restarting.");
						}
						else
						{
							m_Shutdown = strParam.ToLower().Equals("shutdown") ? true : false;
							m_Scheduled = true;
							AutoSave.SavesEnabled = false;
							e.Mobile.SendMessage("You have initiated server {0}.", m_Shutdown ? "shutdown" : "restart");
							m_Maintenance.Start();
						}
					}
					else
						Usage(e);
				}
			}
			catch (Exception exc)
			{
				LogHelper.LogException(exc);
				e.Mobile.SendMessage("There was a problem with the [Maintenance command!!  See console log");
				System.Console.WriteLine("Error with [Maintenance!");
				System.Console.WriteLine(exc.Message);
				System.Console.WriteLine(exc.StackTrace);
			}
		}

		public static bool StartRebuild(bool testcenter)
		{
			int ServerID = Core.Process.Id;

			try
			{
				Process p = new Process();
				p.StartInfo.WorkingDirectory = Core.Process.StartInfo.WorkingDirectory;
				p.StartInfo.FileName = "rebuild.exe";
				if (testcenter)
					p.StartInfo.Arguments = ServerID.ToString() + " " + "true";
				else
					p.StartInfo.Arguments = ServerID.ToString() + " " + "false";


				if (p.Start())
				{
					RebuildProcess = p.Id;
					return true;
				}
			}
			catch
			{
				return false;
			}

			return false;
		}
		public static bool KillRebuild()
		{
			try
			{
				Process p = Process.GetProcessById(RebuildProcess);
				if (p != null && p.ProcessName == "rebuild")
				{
					p.CloseMainWindow();
					p.Close();
					return true;
				}
			}
			catch
			{
				return false;
			}
			return false;
		}
		public Maintenance()
		{

		}

		public class MaintenanceTimer : Timer
		{
			public MaintenanceTimer()
				: base(TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(1.0))
			{
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				if (Maintenance.Scheduled == false)
					return;

				string text;
				if (Maintenance.Countdown > 5)
				{	// just tell staff
					text = string.Format("[Staff] Server restart in {0} minutes", Maintenance.Countdown);
					Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor, 0x482, text);
				}
				else
				{	// tell everyone
					if (Maintenance.Countdown > 1)
						text = String.Format("Server restarting in {0} minutes for maintenance...", Maintenance.Countdown);
					else if (Maintenance.Countdown == 1)
						text = String.Format("Server restarting in {0} minute for maintenance...", Maintenance.Countdown);
					else
						text = String.Format("Server restarting now...");

					World.Broadcast(0x22, true, text);
				}
				Console.WriteLine(text);


				if (Maintenance.Countdown == 0)
				{
					AutoSave.Save();
					Core.Kill(Maintenance.Shutdown == false);
				}

				Maintenance.Countdown--;
			}
		}

		private static void Usage(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Format: Maintenance <cancel|restart|shutdown|rebuild> [date time when]");
			if (m_Rebuild)
			{
				e.Mobile.SendMessage("The server is set to rebuild soon.");
			}
			else if (m_Scheduled)
			{
				e.Mobile.SendMessage("The server is set to restart soon.");
			}
			else
			{
				e.Mobile.SendMessage("The server is not set to restart.");
			}
		}
	}
}