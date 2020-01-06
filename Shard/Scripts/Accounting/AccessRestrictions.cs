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

/* Scripts/Accounting/AccountHandler.cs
 * ChangeLog:
 *	2/27/06 - Pix.
 *		Added.
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Misc;

namespace Server
{
	public class AccessRestrictions
	{
		public static void Initialize()
		{
			EventSink.SocketConnect += new SocketConnectEventHandler(EventSink_SocketConnect);
		}

		private static void EventSink_SocketConnect(SocketConnectEventArgs e)
		{
			try
			{
				IPAddress ip = ((IPEndPoint)e.Socket.RemoteEndPoint).Address;

				if (Firewall.IsBlocked(ip))
				{
					Console.WriteLine("Client: {0}: Firewall blocked connection attempt.", ip);
					e.AllowConnection = false;
					return;
				}
				else if (IPLimiter.SocketBlock && !IPLimiter.Verify(ip))
				{
					Console.WriteLine("Client: {0}: Past IP limit threshold", ip);

					using (StreamWriter op = new StreamWriter("ipLimits.log", true))
						op.WriteLine("{0}\tPast IP limit threshold\t{1}", ip, DateTime.Now);

					e.AllowConnection = false;
					return;
				}
			}
			catch
			{
				e.AllowConnection = false;
			}
		}
	}
}