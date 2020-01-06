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

/* Scripts/Engines/PJUM/PJUM.cs
 * CHANGELOG:
 *  7/05/10, Pix
 *      changed UpdateLocations() to UpdateAnnouncements() - now changes the message after the bounty has been collected
 *  5/17/10, Adam
 *		eliminate the old PJUM.MakeAllEntriesCriminal() feature and replace it with the new mobile function
 * 			m.MakeCriminal(dt - DateTime.Now);
 *	02/11/06, Adam
 *		Make common the formatting of sextant coords.
 *	01/13/06, Pix
 *		New file - result from separation of TCCS and PJUM systems
 */

using System;
using Server;
using Server.Engines;
using Server.Items;
using Server.Mobiles;
using Server.BountySystem;

namespace Server.PJUM
{
	/// <summary>
	/// Summary description for PJUM.
	/// </summary>
	public class PJUM
	{
		//public PJUM()
		//{
		//}

		public static void Initialize()
		{
			Timer.DelayCall(TimeSpan.FromMinutes(1.0), TimeSpan.FromMinutes(1.0), new TimerCallback(PJUM_Work));
		}

		public static void PJUM_Work()
		{
			try
			{
				PJUM.UpdateAnnouncements();
			}
			catch (Exception ex) { EventSink.InvokeLogException(new LogExceptionEventArgs(ex)); }
		}

		public static bool HasBeenReported(Mobile m)
		{
			for (int i = 0; i < TCCS.TheList.Count; i++)
			{
				if (((ListEntry)TCCS.TheList[i]).Mobile == m)
				{
					return true;
				}
			}
			return false;
		}

		public static ListEntry AddMacroer(string[] lines, Mobile m, DateTime dt)
		{
			ListEntry le = new ListEntry(lines, m, dt, ListEntryType.PJUM);
			TCCS.AddEntry(le);
			m.MakeCriminal(dt - DateTime.Now);
			return le;
		}


		public static void UpdateAnnouncements()
		{
			foreach (ListEntry le in TCCS.TheList)
			{
				if (le == null) continue;
				if (le.Type != ListEntryType.PJUM) continue;

				//First, update locations
				string location;
				int xLong = 0, yLat = 0, xMins = 0, yMins = 0;
				bool xEast = false, ySouth = false;
				Map map = le.Mobile.Map;
				bool valid = Sextant.Format(le.Mobile.Location, map, ref xLong, ref yLat, ref xMins, ref yMins, ref xEast, ref ySouth);

				if (valid)
					location = Sextant.Format(xLong, yLat, xMins, yMins, xEast, ySouth);
				else
					location = "????";

				if (!valid)
					location = string.Format("{0} {1}", le.Mobile.X, le.Mobile.Y);

				if (map != null)
				{
					Region reg = le.Mobile.Region;

					if (reg != map.DefaultRegion)
					{
						location += (" in " + reg);
					}
				}

				le.Lines[1] = String.Format("{0} was last seen at {1}.", le.Mobile.Name, location);

				//Next update if no bounty
				PlayerMobile pm = le.Mobile as PlayerMobile;
				if (pm != null)
				{
					if (BountyKeeper.BountiesOnPlayer(pm) <= 0)
					{
						le.Lines[0] = String.Format("{0} is an enemy of the kingdom for unlawful resource gathering.", pm.Name);
					}
				}
			}

		}
	}
}
