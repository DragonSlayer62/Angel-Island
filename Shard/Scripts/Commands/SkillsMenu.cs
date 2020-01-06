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
using Server;
using Server.Targeting;
using Server.Gumps;
using Server.Scripts.Gumps;

namespace Server.Commands
{
	public class Skills
	{
		public static void Initialize()
		{
			Register();
		}

		public static void Register()
		{
			Server.CommandSystem.Register("Skills", AccessLevel.Counselor, new CommandEventHandler(Skills_OnCommand));
		}

		private class SkillsTarget : Target
		{
			public SkillsTarget()
				: base(-1, true, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object o)
			{
				if (o is Mobile)
					from.SendGump(new SkillsGump(from, (Mobile)o));
			}
		}

		[Usage("Skills")]
		[Description("Opens a menu where you can view or edit skills of a targeted mobile.")]
		private static void Skills_OnCommand(CommandEventArgs e)
		{
			e.Mobile.Target = new SkillsTarget();
		}
	}
}