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

namespace Server.Misc
{
	public class RenameRequests
	{
		public static void Initialize()
		{
			EventSink.RenameRequest += new RenameRequestEventHandler(EventSink_RenameRequest);
		}

		private static void EventSink_RenameRequest(RenameRequestEventArgs e)
		{
			Mobile from = e.From;
			Mobile targ = e.Target;
			string name = e.Name;

			if (from.CanSee(targ) && from.InRange(targ, 12) && targ.CanBeRenamedBy(from))
			{
				name = name.Trim();

				if (NameVerification.Validate(name, 1, 16, true, false, true, 0, NameVerification.Empty))
					targ.Name = name;
				else
					from.SendMessage("That name is unacceptable.");
			}
		}
	}
}