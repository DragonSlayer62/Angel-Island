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

/* Scripts/Commands/DeleteItemByType.cs
 * Changelog : 
 *  2/27/11, Adam
 *		initial checkin
 */

using System;
using System.Reflection;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Multis;

namespace Server.Commands
{
	public class DeleteItemByType
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("DeleteItemByType", AccessLevel.GameMaster, new CommandEventHandler(DeleteItemByType_OnCommand));
		}

		[Usage("DeleteItemByType <type> [<property> <value>]")]
		[Description("Finds an item by type with property x set to y.")]
		public static void DeleteItemByType_OnCommand(CommandEventArgs e)
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
					LogHelper Logger = new LogHelper("DeleteItemByType.log", e.Mobile, false);

					// reset jump table
					pm.JumpIndex = 0;
					pm.JumpList = new ArrayList();
					Type tx = ScriptCompiler.FindTypeByName(name);

					if (tx != null)
					{
						foreach (Item item in World.Items.Values)
						{
							if (item != null && !item.Deleted && item.GetType() == tx /* tx.IsAssignableFrom(item.GetType())*/)
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

					// since we already have the items in our jump list, just reuse it for the to-delete list
					if (pm.JumpList.Count > 0)
					{
						foreach (Item ix in pm.JumpList)
						{
							if (ix != null)
							{
								Console.WriteLine("Deleting object {0}.", ix);
								ix.Delete();
							}
						}
					}

					Logger.Finish();
				}
				else
				{
					e.Mobile.SendMessage("Format: DeleteItemByType <type>");
				}
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}
	}
}