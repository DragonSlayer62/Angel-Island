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

/* Scripts/Commands/Abstracted/Implementors/GlobalCommandImplementor.cs
 * CHANGELOG
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;

namespace Server.Commands
{
	public class GlobalCommandImplementor : BaseCommandImplementor
	{
		public GlobalCommandImplementor()
		{
			Accessors = new string[] { "Global" };
			SupportRequirement = CommandSupport.Global;
			SupportsConditionals = true;
			AccessLevel = AccessLevel.Administrator;
			Usage = "Global <command> [condition]";
			Description = "Invokes the command on all appropriate objects in the world. Optional condition arguments can further restrict the set of objects.";
		}

		public override void Compile(Mobile from, BaseCommand command, ref string[] args, ref object obj)
		{
			try
			{
				ObjectConditional cond = ObjectConditional.Parse(from, ref args);

				bool items, mobiles;

				if (!CheckObjectTypes(command, cond, out items, out mobiles))
					return;

				ArrayList list = new ArrayList();

				if (items)
				{
					foreach (Item item in World.Items.Values)
					{
						if (cond.CheckCondition(item))
							list.Add(item);
					}
				}

				if (mobiles)
				{
					foreach (Mobile mob in World.Mobiles.Values)
					{
						if (cond.CheckCondition(mob))
							list.Add(mob);
					}
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