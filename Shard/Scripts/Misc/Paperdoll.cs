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

/* Scripts/Misc/Paperdoll.cs
 * ChangeLog
 *  02/15/05, Pixie
 *		CHANGED FOR RUNUO 1.0.0 MERGE.
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */

using System;
using System.Collections;
using Server;
using Server.Network;

namespace Server.Misc
{
	public class Paperdoll
	{
		public static void Initialize()
		{
			EventSink.PaperdollRequest += new PaperdollRequestEventHandler(EventSink_PaperdollRequest);
		}

		public static void EventSink_PaperdollRequest(PaperdollRequestEventArgs e)
		{
			Mobile beholder = e.Beholder;
			Mobile beheld = e.Beheld;

			//beholder.Send( new DisplayPaperdoll( beheld, Titles.ComputeTitle( beholder, beheld ) ) );
			beholder.Send(new DisplayPaperdoll(beheld, Titles.ComputeTitle(beholder, beheld), beheld.AllowEquipFrom(beholder)));

			if (ObjectPropertyList.Enabled)
			{
				ArrayList items = beheld.Items;

				for (int i = 0; i < items.Count; ++i)
					beholder.Send(((Item)items[i]).OPLPacket);

				// NOTE: OSI sends MobileUpdate when opening your own paperdoll.
				// It has a very bad rubber-banding affect. What positive affects does it have?
			}
		}
	}
}