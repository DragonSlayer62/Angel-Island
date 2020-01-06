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

/* Scripts/Misc/PlayerStepTracking.cs
 * ChangeLog
 *  1/16/11, Pix,
 *      Not used w/Siege.
 *  6/11/04, Pix
 *		Initial version
 */

using System;
using System.Collections;
using Server;
using Server.Accounting;
using Server.Mobiles;
using Server.Multis;

namespace Server.Misc
{
	public class PlayerStepTracking
	{
		public static void Initialize()
		{
            if (!Core.UOSP) //we don't do this for SP.
            {
			    EventSink.Movement += new MovementEventHandler(EventSink_Movement);
            }
		}


		public static void EventSink_Movement(MovementEventArgs e)
		{
            if (!Core.UOSP) //we don't do this for SP.
            {
                Mobile from = e.Mobile;

                if (!from.Player)
                    return;

                if (from is PlayerMobile)
                {
                    Account acct = from.Account as Account;

                    if (acct.m_STIntervalStart + TimeSpan.FromMinutes(20.0) > DateTime.Now)
                    {//within 20 minutes from last step - count step
                        acct.m_STSteps++;
                    }
                    else
                    {
                        //ok, we're outside of a 20-minute period,
                        //so see if they've moved enough within the last 10 
                        //minutes... if so, increment time
                        if (acct.m_STSteps > 50)
                        {
                            //Add an house to the house's refresh time
                            BaseHouse house = null;
                            for (int i = 0; i < 5; i++)
                            {
                                Mobile m = acct[i];
                                if (m != null)
                                {
                                    ArrayList list = BaseHouse.GetHouses(m);
                                    if (list.Count > 0)
                                    {
                                        house = (BaseHouse)list[0];
                                        break;
                                    }
                                }
                            }
                            if (house != null)
                            {
                                house.RefreshHouseOneDay();
                            }
                        }
                        acct.m_STIntervalStart = DateTime.Now;
                        acct.m_STSteps = 1;
                    }
                }
            }
		}

	}
}