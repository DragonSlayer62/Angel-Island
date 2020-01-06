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

/* Scripts/Commands/Abstracted/Implementors/SingleCommandImplementor.cs
 * CHANGELOG
 *  6/14/04, Pix
 *		Removed debugging message.
 *  6/7/04, Pix
 *		Reverted the previous fix, but skipped the check for IsAccessible
 *		if it gets the BountyCommand.
 *  6/7/04, Pix
 *		1.0RC0's OnTarget added a check to the new IsAccessible command which
 *		broke the Bounty command.  Removing the check for IsAccessible.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using System.Collections;
using Server;
using Server.Targeting;

namespace Server.Commands
{
	public class SingleCommandImplementor : BaseCommandImplementor
	{
		public SingleCommandImplementor()
		{
			Accessors = new string[] { "Single" };
			SupportRequirement = CommandSupport.Single;
			AccessLevel = AccessLevel.Counselor;
			Usage = "Single <command>";
			Description = "Invokes the command on a single targeted object. This is the same as just invoking the command directly.";
		}

		public override void Register(BaseCommand command)
		{
			base.Register(command);

			for (int i = 0; i < command.Commands.Length; ++i)
				Server.CommandSystem.Register(command.Commands[i], command.AccessLevel, new CommandEventHandler(Redirect));
		}

		public void Redirect(CommandEventArgs e)
		{
			BaseCommand command = (BaseCommand)Commands[e.Command];

			if (command == null)
				e.Mobile.SendMessage("That is either an invalid command name or one that does not support this modifier.");
			else if (e.Mobile.AccessLevel < command.AccessLevel)
				e.Mobile.SendMessage("You do not have access to that command.");
			else if (command.ValidateArgs(this, e))
				Process(e.Mobile, e.Target, command, e.Arguments);
		}

		public override void Process(Mobile from, object target, BaseCommand command, string[] args)
		{
			if (command.ValidateArgs(this, new CommandEventArgs(from, command.Commands[0], GenerateArgString(args), args)))
				if (target == null)
					from.BeginTarget(-1, command.ObjectTypes == ObjectTypes.All, TargetFlags.None, new TargetStateCallback(OnTarget), new object[] { command, args });
				else
					OnTarget(from, target, new object[] { command, args });
		}

		public void OnTarget(Mobile from, object targeted, object state)
		{
			object[] states = (object[])state;
			BaseCommand command = (BaseCommand)states[0];
			string[] args = (string[])states[1];

			if (command is BountySystem.BountyCommand)
			{
				//from.SendMessage("Use the bountycommand, Luke!");
			}
			else if (!BaseCommand.IsAccessible(from, targeted))
			{
				from.SendMessage("That is not accessible.");
				return;
			}

			switch (command.ObjectTypes)
			{
				case ObjectTypes.Both:
					{
						if (!(targeted is Item) && !(targeted is Mobile))
						{
							from.SendMessage("This command does not work on that.");
							return;
						}

						break;
					}
				case ObjectTypes.Items:
					{
						if (!(targeted is Item))
						{
							from.SendMessage("This command only works on items.");
							return;
						}

						break;
					}
				case ObjectTypes.Mobiles:
					{
						if (!(targeted is Mobile))
						{
							from.SendMessage("This command only works on mobiles.");
							return;
						}

						break;
					}
			}

			RunCommand(from, targeted, command, args);
		}
	}
}