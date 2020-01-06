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

/* Scripts/Commands/Tame.cs
 * Changelog
 *  4/10/10, Adam
 *		Initial version.
 */
using System;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Targeting;
using Server.Mobiles;

namespace Server.Commands
{
	class Tame
	{
		public static void Initialize()
		{
			Server.CommandSystem.Register("Tame", AccessLevel.GameMaster, new CommandEventHandler(Tame_OnCommand));
		}

		public static void Tame_OnCommand(CommandEventArgs e)
		{
			e.Mobile.SendMessage("Target the creature to tame.");
			e.Mobile.Target = new TameFemaleTarget();
		}

		public class TameFemaleTarget : Target
		{
			public TameFemaleTarget()
				: base(10, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				BaseCreature pet = targeted as BaseCreature;

				if (pet != null)
				{
					if (pet.ControlMaster != null)
					{
						from.SendMessage("That creature is already tame.");
						return;
					}
					if (pet.Tamable == false)
					{
						from.SendMessage("that creature cannot be tamed.");
						return;
					}
					pet.ControlMaster = from;
					pet.Controlled = true;
					from.SendMessage(string.Format("That creature had no choice but to accept you as {0} master.", pet.Female ? "her" : "his"));
				}
			}
		}
	}
}
