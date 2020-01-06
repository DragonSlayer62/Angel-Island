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
using Server.Network;
using Server.Prompts;

namespace Server.Engines.Help
{
	public class PagePrompt : Prompt
	{
		private PageType m_Type;

		public PagePrompt(PageType type)
		{
			m_Type = type;
		}

		public override void OnCancel(Mobile from)
		{
			from.SendLocalizedMessage(501235, "", 0x35); // Help request aborted.
		}

		public override void OnResponse(Mobile from, string text)
		{
			from.SendLocalizedMessage(501234, "", 0x35); /* The next available Counselor/Game Master will respond as soon as possible.
															* Please check your Journal for messages every few minutes.
															*/

			PageQueue.Enqueue(new PageEntry(from, text, m_Type));
		}
	}
}