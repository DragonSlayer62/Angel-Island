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

/* Scripts/Commands/FindMobileByType.cs
 * Changelog : 
 *	2/15/11, Adam
 *		Check for a null type when using reflection to find the type
 *  11/21/10, Adam
 *      1. Add reflection and IsAssignableFrom to determine if what we find is derived from what we're searching for.
 *      We're emulating the 'is' keyword for a variable Type.
 *      Eg. You want to find only ChampSpecific, then search for "ChampSpecific". But find all champ engines derived from
 *      ChampEngine, search for "ChampEngine".
 *      2. Make available to GMs for world building/cleanup if the server is launched with the -build command
 *	6/18/08, Adam
 *      first time checkin
 */

using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
	public class FindMobileByType
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindMobileByType", AccessLevel.GameMaster, new CommandEventHandler(FindMobileByType_OnCommand));
		}

		[Usage("FindMobileByType <type>")]
		[Description("Finds a mobile by type.")]
		public static void FindMobileByType_OnCommand(CommandEventArgs e)
		{
			try
			{
				if (e.Length == 1)
				{
					if (e == null || e.Mobile == null || e.Mobile is PlayerMobile == false)
						return;

					// if you are a GM the world needs to be in 'Build' mode to access this comand
					if (e.Mobile.AccessLevel < AccessLevel.Administrator && Core.Building == false)
					{
						e.Mobile.SendMessage("The server must be in build mode for you to access this command.");
						return;
					}

					PlayerMobile pm = e.Mobile as PlayerMobile;
					LogHelper Logger = new LogHelper("FindMobileByType.log", e.Mobile, false);
					string name = e.GetString(0);

					// reset jump table
					pm.JumpIndex = 0;
					pm.JumpList = new ArrayList();
					Type tx = ScriptCompiler.FindTypeByName(name);

					if (tx != null)
					{
						foreach (Mobile mob in World.Mobiles.Values)
						{
							if (mob != null && !mob.Deleted && tx.IsAssignableFrom(mob.GetType()))
							{
								pm.JumpList.Add(mob);
								Logger.Log(LogType.Mobile, mob);
							}
						}
					}
					else
					{
						e.Mobile.SendMessage("{0} is not a recognized type.", name);
					}
					Logger.Finish();
				}
				else
				{
					e.Mobile.SendMessage("Format: FindMobileByType <type>");
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}