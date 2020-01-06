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

/* Scripts/Commands/Host.cs
 * 	CHANGELOG:
 *  12/19/06, Adam
 *      Improve output to show if this is the PROD or TC server.
 * 	2/23/06, Adam
 *	    Initial Version
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
	public class Host
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Host", AccessLevel.GameMaster, new CommandEventHandler(Host_OnCommand));
		}

		[Usage("Host")]
		[Description("Display host information.")]
		public static void Host_OnCommand(CommandEventArgs e)
		{
			try
			{
				string host = Dns.GetHostName();
				IPHostEntry iphe = Dns.Resolve(host);
				IPAddress[] ips = iphe.AddressList;

				e.Mobile.SendMessage("You are on the \"{0}\" Server.",
					Utility.IsHostPROD(host) ? "PROD" : Utility.IsHostTC(host) ? "Test Center" : host);

				for (int i = 0; i < ips.Length; ++i)
					e.Mobile.SendMessage("IP: {0}", ips[i]);
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}