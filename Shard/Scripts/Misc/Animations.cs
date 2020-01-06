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
	public class Animations
	{
		public static void Initialize()
		{
			EventSink.AnimateRequest += new AnimateRequestEventHandler(EventSink_AnimateRequest);
		}

		private static void EventSink_AnimateRequest(AnimateRequestEventArgs e)
		{
			Mobile from = e.Mobile;

			int action;

			switch (e.Action)
			{
				case "bow": action = 32; break;
				case "salute": action = 33; break;
				default: return;
			}

			if (from.Alive && !from.Mounted && from.Body.IsHuman)
				from.Animate(action, 5, 1, true, false, 0);
		}
	}
}