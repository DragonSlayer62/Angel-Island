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
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Gumps;
using Server.Prompts;
using Server.Targeting;
using Server.Misc;
using Server.Multis;

namespace Server.Commands
{

	public class ComLogger
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("ComLogger", AccessLevel.Administrator, new CommandEventHandler(ComLogger_OnCommand));
		}

		[Usage("ComLogger")]
		[Description("Logs all commodity deeds in world info")]
		public static void ComLogger_OnCommand(CommandEventArgs e)
		{
			LogHelper Logger = new LogHelper("Commoditydeed.log", true);

			foreach (Item m in World.Items.Values)
			{

				if (m != null)
				{
					if (m is CommodityDeed && ((CommodityDeed)m).Commodity != null)
					{
						string output = string.Format("{0}\t{1,-25}\t{2,-25}",
							m.Serial + ",", ((CommodityDeed)m).Commodity + ",", ((CommodityDeed)m).Commodity.Amount);

						Logger.Log(LogType.Text, output);
					}
				}
			}
			Logger.Finish();
		}
	}
}
