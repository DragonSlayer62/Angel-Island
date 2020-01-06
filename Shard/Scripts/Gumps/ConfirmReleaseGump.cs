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
/* ChangeLog:
 *	7/9/05, Pix
 *		Added kick to pet->Obey() while releasing, so pet always releases.
 *	4/21/04, Adam
 *		1. Remove duplicate implementation of ConfirmReleaseGump and replace with a second constructor.
 *		2. Remove specific knowledge of TreeOfKnowledge and replaced with a generic rangeCheck flag
 *  4/20/05 smerX
 *		Expanded for use with TreeOfKnowledge.cs
 */


using System;
using Server;
using Server.Mobiles;

namespace Server.Gumps
{
	public class ConfirmReleaseGump : Gump
	{
		private Mobile m_From;
		private BaseCreature m_Pet;
		private bool m_RangeCheck;

		public ConfirmReleaseGump(Mobile from, BaseCreature pet)
			: this(from, pet, true)
		{

		}

		public ConfirmReleaseGump(Mobile from, BaseCreature pet, bool rangeCheck)
			: base(50, 50)
		{
			m_From = from;
			m_Pet = pet;
			m_RangeCheck = rangeCheck;

			m_From.CloseGump(typeof(ConfirmReleaseGump));

			AddPage(0);

			AddBackground(0, 0, 270, 120, 5054);
			AddBackground(10, 10, 250, 100, 3000);

			AddHtmlLocalized(20, 15, 230, 60, 1046257, true, true); // Are you sure you want to release your pet?

			AddButton(20, 80, 4005, 4007, 2, GumpButtonType.Reply, 0);
			AddHtmlLocalized(55, 80, 75, 20, 1011011, false, false); // CONTINUE

			AddButton(135, 80, 4005, 4007, 1, GumpButtonType.Reply, 0);
			AddHtmlLocalized(170, 80, 75, 20, 1011012, false, false); // CANCEL
		}

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			if (info.ButtonID == 2)
			{
				if (!m_Pet.Deleted && m_Pet.Controlled && m_From == m_Pet.ControlMaster && m_From.CheckAlive() && m_Pet.CheckControlChance(m_From))
				{
					if (m_Pet.Map == m_From.Map)
					{
						if (m_RangeCheck == true && !m_Pet.InRange(m_From, 14))
						{
							m_From.SendMessage("You are too far away from your pet.");
							return;
						}

						m_Pet.ControlTarget = null;
						m_Pet.ControlOrder = OrderType.Release;
						//Pix: added kick to Obey because when we added the ability to release a 'lost'
						// pet, if there were no players around, the pet would not release until a player
						// got in range of the pet.  This wasn't an issue before because there was 
						// a range check involved so there was always a player around when releasing the pet.
						m_Pet.AIObject.Obey();
						BaseHire m_Hire = m_Pet as BaseHire;	//added by Old Salty from here . . .
						if (m_Hire != null && m_Hire.IsHired)
						{
							m_Hire.IsHired = false;
						}										//. . . to here
					}
				}
			}
		}
	}
}
