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

/* Scripts/Commands/FindItem.cs
 * Changelog
 *  12/24/05, Kit
 *		changed to use logtype.ItemSerial which outputs serial item as well as item details
 *	03/25/05, erlein
 *		Integrated with LogHelper class.
 *	03/23/05, erlein
 *		Moved to /Scripts/Commands/FindItem.cs (for Find* command normalization)
 *	03/22/05, erlein
 *	    Fixed location, changed output format, moved speed output to
 *		client from console window, made all matches case insensitive.
 *	03/22/05, erlein
 *		Altered so reflects type before iteration of instances.
 *		Added regex match to speed up string matching.
 *	03/16/05, erlein
 *		Initial creation.
 */

using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Commands;
using System.Reflection;
using System.Collections;
using System.Text.RegularExpressions;

namespace Server.Commands
{
	public class FindItem
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindItem", AccessLevel.Administrator, new CommandEventHandler(FindItem_OnCommand));
		}

		[Usage("FindItem <property> <value>")]
		[Description("Finds all items with property matching value.")]
		public static void FindItem_OnCommand(CommandEventArgs e)
		{
			if (e.Length > 1)
			{

				LogHelper Logger = new LogHelper("finditem.log", e.Mobile, false);

				// Extract property & value from command parameters

				string sProp = e.GetString(0);
				string sVal = "";

				if (e.Length > 2)
				{

					sVal = e.GetString(1);

					// Concatenate the strings
					for (int argi = 2; argi < e.Length; argi++)
						sVal += " " + e.GetString(argi);
				}
				else
					sVal = e.GetString(1);

				Regex PattMatch = new Regex("= \"*" + sVal, RegexOptions.IgnoreCase);

				// Loop through assemblies and add type if has property

				Type[] types;
				Assembly[] asms = ScriptCompiler.Assemblies;

				ArrayList MatchTypes = new ArrayList();

				for (int i = 0; i < asms.Length; ++i)
				{
					types = ScriptCompiler.GetTypeCache(asms[i]).Types;

					foreach (Type t in types)
					{

						if (typeof(Item).IsAssignableFrom(t))
						{

							// Reflect type
							PropertyInfo[] allProps = t.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public);

							foreach (PropertyInfo prop in allProps)
								if (prop.Name.ToLower() == sProp.ToLower())
									MatchTypes.Add(t);
						}
					}
				}

				// Loop items and check vs. types

				foreach (Item item in World.Items.Values)
				{
					Type t = item.GetType();
					bool match = false;

					foreach (Type MatchType in MatchTypes)
					{
						if (t == MatchType)
						{
							match = true;
							break;
						}
					}

					if (match == false)
						continue;

					// Reflect instance of type (matched)

					if (PattMatch.IsMatch(Properties.GetValue(e.Mobile, item, sProp)))
						Logger.Log(LogType.ItemSerial, item);

				}

				Logger.Finish();
			}
			else
			{
				// Badly formatted
				e.Mobile.SendMessage("Format: FindItem <property> <value>");
			}
		}
	}
}


