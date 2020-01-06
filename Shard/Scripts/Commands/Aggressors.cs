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

/* scripts/commands/Aggressors.cs
 * 	CHANGELOG:
 * 	3/24/05, Kitaras
 *	Initial Version
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Commands;

namespace Server.Commands
{

	public class AggressorsCommand : BaseCommand
	{

		public static void Initialize()
		{
			TargetCommands.Register(new AggressorsCommand());
		}

		public AggressorsCommand()
		{
			AccessLevel = AccessLevel.GameMaster;
			Supports = CommandSupport.Simple;
			Commands = new string[] { "Aggressors", "Aggres" };
			ObjectTypes = ObjectTypes.Mobiles;

			Usage = "Aggressors <target>";
			Description = "Lists the aggressor list of the target";
		}

		public override void Execute(CommandEventArgs e, object obj)
		{
			Mobile m = obj as Mobile;
			Mobile from = e.Mobile;

			if (m != null)
			{
				List<AggressorInfo> aggressors = m.Aggressors;
				if (aggressors.Count > 0)
				{
					for (int i = 0; i < aggressors.Count; ++i)
					{

						AggressorInfo info = (AggressorInfo)aggressors[i];
						Mobile temp = info.Attacker;
						from.SendMessage("Aggressor:{0} '{1}' Ser:{2}, Time:{3}, Expired:{4}",
									(temp is PlayerMobile ? ((PlayerMobile)temp).Account.ToString() : ((Mobile)temp).Name),
									temp.GetType().Name,
									temp.Serial,
									info.LastCombatTime.TimeOfDay,
									info.Expired);
					}
				}
			}
			else
			{
				AddResponse("Please target a mobile.");
			}
		}

	}


}

