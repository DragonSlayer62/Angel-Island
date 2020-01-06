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

/* Scripts/Commands/FindItemByType.cs
 * Changelog : 
 *  12/22/10, Adam
 *		Add 'property' and 'value' processing so that you can search for and item type with value X
 *		Example:
 *		[FindItemByType Spawner Running False
 *  11/21/10, Adam
 *      1. Add reflection and IsAssignableFrom to determine if what we find is derived from what we're searching for.
 *      We're emulating the 'is' keyword for a variable Type.
 *      Eg. You want to find only ChampSpecific, then search for "ChampSpecific". But find all champ engines derived from
 *      ChampEngine, search for "ChampEngine".
 *      2. Make available to GMs for world building/cleanup if the server is launched with the -build command
 *	3/10/10, Adam
 *		Add Jump List processing
 *	3/9/07, Adam
 *      Convert to a "find item by type" command
 *	9/7/06, Adam
 *		Remove the hack and make into: Find(multi)ByType 
 *	06/28/06, Adam
 *		Find Mobile by Type (currently hacked to find PlayerBarkeeper only)
 */

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
	public class FindItemByType
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindItemByType", AccessLevel.GameMaster, new CommandEventHandler(FindItemByType_OnCommand));
		}

		[Usage("FindItemByType <type> [<property> <value>]")]
		[Description("Finds an item by type with property x set to y.")]
		public static void FindItemByType_OnCommand(CommandEventArgs e)
		{
			try
			{

				if (e == null || e.Mobile == null || e.Mobile is PlayerMobile == false)
					return;

				string sProp = null;
				string sVal = null;
				string name = null;

				if (e.Length >= 1)
				{
					name = e.GetString(0);

					if (e.Length >= 2)
						sProp = e.GetString(1);

					if (e.Length >= 3)
						sVal = e.GetString(2);

					// if you are a GM the world needs to be in 'Build' mode to access this comand
					if (e.Mobile.AccessLevel < AccessLevel.Administrator && Core.Building == false)
					{
						e.Mobile.SendMessage("The server must be in build mode for you to access this command.");
						return;
					}

					PlayerMobile pm = e.Mobile as PlayerMobile;
					LogHelper Logger = new LogHelper("FindItemByType.log", e.Mobile, false);

					// reset jump table
					pm.JumpIndex = 0;
					pm.JumpList = new ArrayList();
					Type tx = ScriptCompiler.FindTypeByName(name);

					if (tx != null)
					{
						foreach (Item item in World.Items.Values)
						{
							if (item != null && !item.Deleted && tx.IsAssignableFrom(item.GetType()))
							{
								// read the properties
								PropertyInfo[] allProps = item.GetType().GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

								if (sProp != null)
								{
									foreach (PropertyInfo prop in allProps)
									{
										if (prop.Name.ToLower() == sProp.ToLower())
										{
											bool ok = false;
											string val = Properties.GetValue(e.Mobile, item, sProp);

											// match a null value
											if ((val == null || val.Length == 0 || val.EndsWith("(-null-)", StringComparison.CurrentCultureIgnoreCase)) && (sVal == null || sVal.Length == 0))
												ok = true;

											// see if the property matches
											else if (val != null && sVal != null)
											{
												string[] toks = val.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
												if (toks.Length >= 3 && toks[2].Equals(sVal, StringComparison.CurrentCultureIgnoreCase))
													ok = true;
												else
													break;
											}

											if (ok)
											{
												pm.JumpList.Add(item);
												Logger.Log(LogType.Item, item);
												break;
											}
										}
									}
								}
								else
								{	// no prop to check, everything matches
									pm.JumpList.Add(item);
									Logger.Log(LogType.Item, item);
								}
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
					e.Mobile.SendMessage("Format: FindItemByType <type>");
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}