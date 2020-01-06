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

/* Scripts/Commands/FindItemByID.cs
 * ChangeLog
 *  3//26/07, Adam
 *      Convert to find an item by ItemID
 *	03/25/05, erlein
 *		Integrated with LogHelper class.
 *		Reformatted so readable (functionality left unchanged).
 *	03/23/05, erlein
 *		Moved to /Scripts/Commands/FindItemByID.cs (for Find* command normalization).
 *		Changed namespace to Server.Commands.
 *	9/15/04, Adam
 *		Added header and copyright
 */

using System;
using Server;

namespace Server.Commands
{

	public class FindItemByID
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("FindItemByID", AccessLevel.GameMaster, new CommandEventHandler(FindItemByID_OnCommand));
		}

		[Usage("FindItemByID <ItemID>")]
		[Description("Finds an item by graphic ID.")]
		public static void FindItemByID_OnCommand(CommandEventArgs e)
		{
			try
			{
				if (e.Length == 1)
				{
					//erl: LogHelper class handles generic logging functionality
					LogHelper Logger = new LogHelper("FindItemByID.log", e.Mobile, false);

					int ItemId = 0;
					string sx = e.GetString(0).ToLower();

					try
					{
						if (sx.StartsWith("0x"))
						{   // assume hex
							sx = sx.Substring(2);
							ItemId = int.Parse(sx, System.Globalization.NumberStyles.AllowHexSpecifier);
						}
						else
						{   // assume decimal
							ItemId = int.Parse(sx);
						}
					}
					catch
					{
						e.Mobile.SendMessage("Format: FindItemByID <ItemID>");
						return;
					}

					foreach (Item ix in World.Items.Values)
					{
						if (ix is Item)
							if (ix.ItemID == ItemId)
							{
								Logger.Log(LogType.Item, ix);
							}
					}
					Logger.Finish();
				}
				else
					e.Mobile.SendMessage("Format: FindItemByID <ItemID>");
			}
			catch (Exception err)
			{

				e.Mobile.SendMessage("Exception: " + err.Message);
			}

			e.Mobile.SendMessage("Done.");
		}
	}
}
