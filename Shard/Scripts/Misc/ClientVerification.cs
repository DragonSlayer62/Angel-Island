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

/* Scripts\Misc\ClientVerification.cs
 * ChangeLog:
 *	3/3/10, Adam
 *		Disallow UOTD and God clients: m_AllowUOTD = false, m_AllowGod = false;
 *	2/26/08, Adam
 *		fix spelling of "recommends"	
 */

using System;
using Server;
using System.Diagnostics;
using System.IO;
using Server.Network;
using Server.Gumps;
using Server.Mobiles;

namespace Server.Misc
{
	public class ClientVerification
	{
		private enum OldClientResponse
		{
			Ignore,
			Warn,
			Annoy,
			LenientKick,
			Kick
		}

		private static bool m_DetectClientRequirement = true;

		private static OldClientResponse m_OldClientResponse = OldClientResponse.Warn;

		private static ClientVersion m_Required;
		private static bool m_AllowRegular = true, m_AllowUOTD = false, m_AllowGod = false;

		private static TimeSpan m_AgeLeniency = TimeSpan.FromDays(10);
		private static TimeSpan m_GameTimeLeniency = TimeSpan.FromHours(25);

		private static TimeSpan m_KickDelay = TimeSpan.FromSeconds(20.0);

		public static ClientVersion Required
		{
			get
			{
				return m_Required;
			}
			set
			{
				m_Required = value;
			}
		}

		public static bool AllowRegular
		{
			get
			{
				return m_AllowRegular;
			}
			set
			{
				m_AllowRegular = value;
			}
		}

		public static bool AllowUOTD
		{
			get
			{
				return m_AllowUOTD;
			}
			set
			{
				m_AllowUOTD = value;
			}
		}

		public static bool AllowGod
		{
			get
			{
				return m_AllowGod;
			}
			set
			{
				m_AllowGod = value;
			}
		}

		public static TimeSpan KickDelay
		{
			get
			{
				return m_KickDelay;
			}
			set
			{
				m_KickDelay = value;
			}
		}

		public static void Initialize()
		{
			EventSink.ClientVersionReceived += new ClientVersionReceivedHandler(EventSink_ClientVersionReceived);

			//ClientVersion.Required = null;
			//Required = new ClientVersion( "6.0.0.0" );

			if (m_DetectClientRequirement)
			{
				string path = Core.FindDataFile("client.exe");

				if (File.Exists(path))
				{
					FileVersionInfo info = FileVersionInfo.GetVersionInfo(path);

					if (info.FileMajorPart != 0 || info.FileMinorPart != 0 || info.FileBuildPart != 0 || info.FilePrivatePart != 0)
					{
						Required = new ClientVersion(info.FileMajorPart, info.FileMinorPart, info.FileBuildPart, info.FilePrivatePart);
					}
				}
			}

			if (Required != null)
			{
				Utility.PushColor(ConsoleColor.White);
				Console.WriteLine("Restricting client version to {0}. Action to be taken: {1}", Required, m_OldClientResponse);
				Utility.PopColor();
			}
		}

		private static void EventSink_ClientVersionReceived(ClientVersionReceivedArgs e)
		{
			string kickMessage = null;
			NetState state = e.State;
			ClientVersion version = e.Version;

			if (state.Mobile.AccessLevel > AccessLevel.Player)
				return;

			if (Required != null && version < Required && (m_OldClientResponse == OldClientResponse.Kick || (m_OldClientResponse == OldClientResponse.LenientKick && (DateTime.Now - state.Mobile.CreationTime) > m_AgeLeniency && state.Mobile is PlayerMobile && ((PlayerMobile)state.Mobile).GameTime > m_GameTimeLeniency)))
			{
				kickMessage = String.Format("This server requires your client version be at least {0}.", Required);
			}
			else if (!AllowGod || !AllowRegular || !AllowUOTD)
			{
				if (!AllowGod && version.Type == ClientType.God)
					kickMessage = "This server does not allow god clients to connect.";
				else if (!AllowRegular && version.Type == ClientType.Regular)
					kickMessage = "This server does not allow regular clients to connect.";
				else if (!AllowUOTD && state.IsUOTDClient)
					kickMessage = "This server does not allow UO:TD clients to connect.";

				if (!AllowGod && !AllowRegular && !AllowUOTD)
				{
					kickMessage = "This server does not allow any clients to connect.";
				}
				else if (AllowGod && !AllowRegular && !AllowUOTD && version.Type != ClientType.God)
				{
					kickMessage = "This server requires you to use the god client.";
				}
				else if (kickMessage != null)
				{
					if (AllowRegular && AllowUOTD)
						kickMessage += " You can use regular or UO:TD clients.";
					else if (AllowRegular)
						kickMessage += " You can use regular clients.";
					else if (AllowUOTD)
						kickMessage += " You can use UO:TD clients.";
				}
			}

			if (kickMessage != null)
			{
				state.Mobile.SendMessage(0x22, kickMessage);
				state.Mobile.SendMessage(0x22, "You will be disconnected in {0} seconds.", KickDelay.TotalSeconds);

				Timer.DelayCall(KickDelay, delegate
				{
					if (state.Socket != null)
					{
						Console.WriteLine("Client: {0}: Disconnecting, bad version", state);
						state.Dispose();
					}
				});
			}
			else if (Required != null && version < Required)
			{
				switch (m_OldClientResponse)
				{
					case OldClientResponse.Warn:
						{
							state.Mobile.SendMessage(0x22, "Your client is out of date. Please update your client.", Required);
							state.Mobile.SendMessage(0x22, "This server recommends that your client version be at least {0}.", Required);
							break;
						}
					case OldClientResponse.LenientKick:
					case OldClientResponse.Annoy:
						{
							SendAnnoyGump(state.Mobile);
							break;
						}
				}
			}
		}

		private static void SendAnnoyGump(Mobile m)
		{
			if (m.NetState != null && m.NetState.Version < Required)
			{
				Gump g = new WarningGump(1060637, 30720, String.Format("Your client is out of date. Please update your client.<br>This server recommends that your client version be at least {0}.<br> <br>You are currently using version {1}.<br> <br>To patch, run UOPatch.exe inside your Ultima Online folder.", Required, m.NetState.Version), 0xFFC000, 480, 360,
					delegate(Mobile mob, bool selection, object o)
					{
						m.SendMessage("You will be reminded of this again.");

						if (m_OldClientResponse == OldClientResponse.LenientKick)
							m.SendMessage("Old clients will be kicked after {0} days of character age and {1} hours of play time", m_AgeLeniency, m_GameTimeLeniency);

						Timer.DelayCall(TimeSpan.FromMinutes(Utility.Random(5, 15)), delegate { SendAnnoyGump(m); });
					}, null, false);

				g.Dragable = false;
				g.Closable = false;
				g.Resizable = false;

				m.SendGump(g);
			}
		}
	}
}