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
using System.Reflection;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Commands
{
	public class ConvertPlayers
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("ConvertPlayers", AccessLevel.Administrator, new CommandEventHandler(Convert_OnCommand));
		}

		public static void Convert_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Converting all players to PlayerMobile.  You will be disconnected.  Please Restart the server after the world has finished saving.");
			ArrayList mobs = new ArrayList(World.Mobiles.Values);
			int count = 0;

			foreach (Mobile m in mobs)
			{
				if (m.Player && !(m is PlayerMobile))
				{
					count++;
					if (m.NetState != null)
						m.NetState.Dispose();

					PlayerMobile pm = new PlayerMobile(m.Serial);
					pm.DefaultMobileInit();

					ArrayList copy = new ArrayList(m.Items);
					for (int i = 0; i < copy.Count; i++)
						pm.AddItem((Item)copy[i]);

					CopyProps(pm, m);

					for (int i = 0; i < m.Skills.Length; i++)
					{
						pm.Skills[i].Base = m.Skills[i].Base;
						pm.Skills[i].SetLockNoRelay(m.Skills[i].Lock);
					}

					World.Mobiles[m.Serial] = pm;
				}
			}

			if (count > 0)
			{
				NetState.ProcessDisposedQueue();
				World.Save();

				Console.WriteLine("{0} players have been converted to PlayerMobile.  Please restart the server.", count);
				while (true)
					Console.ReadLine();
			}
			else
			{
				e.Mobile.SendMessage("Couldn't find any Players to convert.");
			}
		}

		private static void CopyProps(Mobile to, Mobile from)
		{
			Type type = typeof(Mobile);

			PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

			for (int p = 0; p < props.Length; p++)
			{
				PropertyInfo prop = props[p];

				if (prop.CanRead && prop.CanWrite)
				{
					try
					{
						prop.SetValue(to, prop.GetValue(from, null), null);
					}
					catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
				}
			}
		}
	}
}
