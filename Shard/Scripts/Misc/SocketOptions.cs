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

/* Scripts/Misc/SocketOptions.cs
 * ChangeLog
 *	2/15/11, Adam
 *		Core EventShard can be turned on with all other server configurations and overrides the default port for that shard.
 *		For example, one can turn on -uoev -uomo yielding an event shard with the Mortalis rule set, but a port number that
 *		won't collide with the production Mortalis.
 *		Note you can even have a Test Center version of an event shard, i.e., -uoev -uomo -uotc
 *  11/12/10, Adam
 *      Add in Core.TestCenter listening port
 *	2/27/06, Pix
 *		Changes for IPLimiter.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Server;
using Server.Misc;
using Server.Network;

namespace Server
{
	public class SocketOptions
	{
		private const bool NagleEnabled = false; // Should the Nagle algorithm be enabled? This may reduce performance
		private const int CoalesceBufferSize = 512; // MSS that the core will use when buffering packets
		private const int PooledSockets = 32; // The number of sockets to initially pool. Ideal value is expected client count. 

		public const int AngelIslandPort = 2593;
		public const int SiegePort = 2594;
		public const int TestCenterPort = 2595;
		public const int MortalisPort = 2596;
		public const int AIResurrectionPort = 2597;
		public const int EventShardPort = 2598;
		//public const int AIResurrectionPort = 2599;

		private static IPEndPoint[] m_ListenerEndPoints;
		//        = new IPEndPoint[] {
		//			new IPEndPoint( IPAddress.Any, Port ), // Default: Listen on port 2593 on all IP addresses
		//			
		//			// Examples:
		//			// new IPEndPoint( IPAddress.Any, 80 ), // Listen on port 80 on all IP addresses
		//			// new IPEndPoint( IPAddress.Parse( "1.2.3.4" ), 2593 ), // Listen on port 2593 on IP address 1.2.3.4
		//		};

		public static void Initialize()
		{
			m_ListenerEndPoints = new IPEndPoint[1];

			if (Core.UOEV)
			{
				// Core EventShard can be turned on with all other server configurations and overrides the default port for that shard.
				//  For example, one can turn on -uoev -uomo yielding an event shard with the Mortalis rule set, but a port number that
				//	won't collide with the production Mortalis.
				//	Note you can even have a Test Center version of an event shard, i.e., -uoev -uomo -uotc
				m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, EventShardPort);
			}
			else
			{
				// Core TestCenter can be turned on with Core.UOSP giving us a SP TC
				//  otherwise it is the usual AI TC
				//  but whatever the test center mode, it always gets its own port
				if (Core.UOTC)
				{
					m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, TestCenterPort);
				}
				else
				{
					if (Core.UOSP)
						m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, SiegePort);
					else if (Core.UOMO)
						m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, MortalisPort);
					else if (Core.UOAR)
						m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, AIResurrectionPort);
					else
						m_ListenerEndPoints[0] = new IPEndPoint(IPAddress.Any, AngelIslandPort);
				}
			}

			SendQueue.CoalesceBufferSize = CoalesceBufferSize;
			SocketPool.InitialCapacity = PooledSockets;

			EventSink.SocketConnect += new SocketConnectEventHandler(EventSink_SocketConnect);

			Listener.EndPoints = m_ListenerEndPoints;
		}

		private static void EventSink_SocketConnect(SocketConnectEventArgs e)
		{
			if (!e.AllowConnection)
				return;

			if (!NagleEnabled)
				e.Socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1); // RunUO uses its own algorithm
		}
	}
}

//using System;
//using System.IO;
//using System.Net;
//using System.Net.Sockets;
//using Server;
//using Server.Misc;
//using Server.Network;

//namespace Server
//{
//    public class SocketOptions
//    {
//        private const int CoalesceBufferSize = 512; // MSS that the core will use when buffering packets, a value of 0 will turn this buffering off and Nagle on

//        private static int[] m_AdditionalPorts = new int[0];
//        //private static int[] m_AdditionalPorts = new int[]{ 2594 };

//        public static void Initialize()
//        {
//            NetState.CreatedCallback = new NetStateCreatedCallback( NetState_Created );
//            SendQueue.CoalesceBufferSize = CoalesceBufferSize;

//            if ( m_AdditionalPorts.Length > 0 )
//                EventSink.ServerStarted += new ServerStartedEventHandler( EventSink_ServerStarted );
//        }

//        public static void EventSink_ServerStarted()
//        {
//            //for (int i = 0; i < m_AdditionalPorts.Length; ++i)
//            //{
//            //    Core.MessagePump.AddListener(new Listener(m_AdditionalPorts[i]));
//            //}
//        }

//        public static void NetState_Created( NetState ns )
//        {
//            if ( IPLimiter.SocketBlock && !IPLimiter.Verify( ns.Address ) )
//            {
//                Console.WriteLine( "Login: {0}: Past IP limit threshold", ns );

//                using ( StreamWriter op = new StreamWriter( "ipLimits.log", true ) )
//                    op.WriteLine( "{0}\tPast IP limit threshold\t{1}", ns, DateTime.Now );

//                ns.Dispose();
//                return;
//            }

//            Socket s = ns.Socket;

//            s.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 15000 );
//            s.SetSocketOption( SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 15000 );

//            if ( CoalesceBufferSize > 0 )
//                s.SetSocketOption( SocketOptionLevel.Tcp, SocketOptionName.NoDelay, 1 ); // RunUO uses its own algorithm
//        }
//    }
//}