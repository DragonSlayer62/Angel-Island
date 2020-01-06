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

/* Scripts/Commands/FindMultiByType.cs
 * Changelog : 
 *	3/9/07, Adam
 *		first time checkin
 */

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
	public class FindMultiByType
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindMultiByType", AccessLevel.Administrator, new CommandEventHandler(FindMultiByType_OnCommand));
		}

		[Usage("FindMultiByType <type>")]
		[Description("Finds a multi by type.")]
		public static void FindMultiByType_OnCommand(CommandEventArgs e)
		{
			try
			{
				if (e.Length == 1)
				{
					LogHelper Logger = new LogHelper("FindMultiByType.log", e.Mobile, false);

					string name = e.GetString(0);

					foreach (ArrayList list in Server.Multis.BaseHouse.Multis.Values)
					{
						for (int i = 0; i < list.Count; i++)
						{
							BaseHouse house = list[i] as BaseHouse;
							// like Server.Multis.Tower
							if (house.GetType().ToString().ToLower().IndexOf(name.ToLower()) >= 0)
							{
								Logger.Log(house);
							}
						}
					}
					Logger.Finish();
				}
				else
				{
					e.Mobile.SendMessage("Format: FindMultiByType <type>");
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}