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
using Server;

namespace Server.Misc
{
	public class Broadcasts
	{
		public static void Initialize()
		{
			EventSink.Crashed += new CrashedEventHandler(EventSink_Crashed);
			EventSink.Shutdown += new ShutdownEventHandler(EventSink_Shutdown);
		}

		public static void EventSink_Crashed(CrashedEventArgs e)
		{
			try
			{
				World.Broadcast(0x35, true, "The server has crashed.");
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}

		public static void EventSink_Shutdown(ShutdownEventArgs e)
		{
			try
			{
				World.Broadcast(0x35, true, "The server has shut down.");
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}