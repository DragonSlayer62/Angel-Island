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

/* Scripts/Commands/Abstracted/Implementors/RegionCommandImplementor.cs
 * CHANGELOG
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using System.Collections;
using Server;

namespace Server.Commands
{
	public class RegionCommandImplementor : BaseCommandImplementor
	{
		public RegionCommandImplementor()
		{
			Accessors = new string[] { "Region" };
			SupportRequirement = CommandSupport.Region;
			SupportsConditionals = true;
			AccessLevel = AccessLevel.Administrator;
			Usage = "Region <command> [condition]";
			Description = "Invokes the command on all appropriate mobiles in your current region. Optional condition arguments can further restrict the set of objects.";
		}

		public override void Compile(Mobile from, BaseCommand command, ref string[] args, ref object obj)
		{
			try
			{
				ObjectConditional cond = ObjectConditional.Parse(from, ref args);

				bool items, mobiles;

				if (!CheckObjectTypes(command, cond, out items, out mobiles))
					return;

				Region reg = from.Region;

				ArrayList list = new ArrayList();

				if (mobiles)
				{
					foreach (Mobile mob in reg.Mobiles.Values)
					{
						if (cond.CheckCondition(mob))
							list.Add(mob);
					}
				}
				else
				{
					command.LogFailure("This command does not support mobiles.");
					return;
				}

				obj = list;
			}
			catch (Exception ex)
			{
				LogHelper.LogException(ex);
				from.SendMessage(ex.Message);
			}
		}
	}
}