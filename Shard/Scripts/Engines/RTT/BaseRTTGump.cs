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
 *			March 27, 2007
 */

/* Scripts/Engines/RTT/BaseRTTGump.cs
 * CHANGELOG:
 *  2010.06.10 - Pix
 *      Added this.Closable = false; to constuctor
 *      Close AFK gump on timeout.
 *	3/22/10, adam
 *		uncomment the lines and instead turn it off via CoreAI.FeatureBits.RTTNotifyEnabled
 *	3/18/10, adam
 *		Remove the spam to the staff about success and failures and only broadcast when someone passes in less than 
 *		1 second. (probably cheating)
 *	3/22/09, Adam
 *		Add a timer that timesout and auto adds the player to the [macroer list if the gump is not addressed in a reasonable time.
 *		Use random numbers to reduce predictability.
 *  8/26/2007, Pix
 *      InitialVersion
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Server;
using Server.Gumps;
using Server.Mobiles;

namespace Server.RTT
{
	class RTTTimer : Timer
	{
		private Mobile m_from;

		public RTTTimer(Mobile from, TimeSpan ts)
			: base(ts)
		{
			m_from = from;
		}

		protected override void OnTick()
		{
			try
			{
				if (m_from != null && m_from is PlayerMobile && m_from.Deleted == false)
				{
					//close all RTT gumps first
					m_from.CloseGump(typeof(BaseRTTGump));

					//player did not answer RTT gump - timeout, assume we've got an AFK macroer - auto [macroer him!
					PJUM.MacroerCommand.ReportAsMacroer(null, m_from as PlayerMobile);
				}
			}
			catch (Exception exc)
			{
				Server.Commands.LogHelper.LogException(exc);
			}
		}

	}

	abstract class BaseRTTGump : Gump
	{
		private Mobile m_Mobile;
		private string m_strNotify = "";
		private string m_strSkill = "";
		private int m_CorrectResponse = -1;
		private int m_CorrectResponseOffset = 0;
		private DateTime m_RTTLaunched;
		private RTTTimer m_timeout = null;

		protected Mobile Mobile
		{
			get { return m_Mobile; }
			set { m_Mobile = value; }
		}

		protected string Notification
		{
			get { return m_strNotify; }
			set { m_strNotify = value; }
		}

		protected string Skill
		{
			get { return m_strSkill; }
			set { m_strSkill = value; }
		}

		protected int CorrectResponse
		{
			get { return m_CorrectResponse; }
			set { m_CorrectResponse = value; }
		}

		protected int CorrectResponseOffset
		{
			get { return m_CorrectResponseOffset; }
			set { m_CorrectResponseOffset = value; }
		}

		public BaseRTTGump(Mobile from, string strNotice, string strSkill, int x, int y)
			: base(x, y)
		{
			m_Mobile = from;
			m_strNotify = strNotice;
			m_strSkill = strSkill;

			this.Closable = false;

			//close any other RTT gumps
			from.CloseGump(typeof(BaseRTTGump));

			// notify staff
			if (CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RTTNotifyEnabled))
			{
				Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Administrator,
				0x482,
				String.Format("{0}({1}) is taking the RTT ({2}).", m_Mobile.Name, m_Mobile.Serial, strSkill));
			}

			// record the fact that the RTT test is being taken
			Server.Commands.LogHelper lh = new Server.Commands.LogHelper("RTT.log", false, true);
			lh.Log(Server.Commands.LogType.Mobile, m_Mobile, String.Format("({0}) RTT Launched.", strSkill));
			lh.Finish();

			//This will call any child-gump's SetupGump and set it up like the child wants.
			SetupGump();

			//Save when we launched the gump for later recording
			m_RTTLaunched = DateTime.Now;

			// if the player fails to respond in 'timeout' time, then we will cound this as an RTT failure.
			//	this does two things: (1) makes the next test 5 minutes from now, (2) will move them closer to being counted as a macroer
			m_timeout = new RTTTimer(m_Mobile, TimeSpan.FromSeconds(Utility.RandomList(90, 120, 180)));
			m_timeout.Start();
		}

		protected abstract void SetupGump();

		public override void OnResponse(Server.Network.NetState sender, RelayInfo info)
		{
			try
			{
				// kill our timeout timer
				if (m_timeout != null)
				{
					m_timeout.Stop();
					m_timeout = null;
				}

				int button = info.ButtonID;
				TimeSpan diff = DateTime.Now - m_RTTLaunched;

				if (button == m_CorrectResponse + m_CorrectResponseOffset)
				{
					// record answer
					Server.Commands.LogHelper lh = new Server.Commands.LogHelper("RTT.log", false, true);
					lh.Log(Server.Commands.LogType.Mobile, m_Mobile, string.Format("PASSED the RTT in {0} ms", diff.TotalMilliseconds));
					lh.Finish();

					m_Mobile.SendMessage("Thanks for verifying that you're at your computer.");
					((PlayerMobile)m_Mobile).RTTResult(true);

					if (CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RTTNotifyEnabled))
					{
						if (diff <= TimeSpan.FromSeconds(1.0))
						{
							Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor,
								0x22,
								String.Format("{0}({1}) has quickly passed the RTT ({2} ms) ({3}).", m_Mobile.Name, m_Mobile.Serial, diff.TotalMilliseconds, m_strNotify));
						}
						else
						{
							Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor,
								0x482,
								String.Format("{0}({1}) has passed the RTT ({2} ms).  ({3})", m_Mobile.Name, m_Mobile.Serial, diff.TotalMilliseconds, m_strNotify));
						}

						// Look for and record suspiciously fast answers
						if (diff <= TimeSpan.FromSeconds(3.0))
						{
							Server.Commands.LogHelper lh2 = new Server.Commands.LogHelper("RTTAlert.log", false, true);
							lh2.Log(Server.Commands.LogType.Mobile, m_Mobile, string.Format("{0} ms", diff.TotalMilliseconds));
							lh2.Finish();
						}
					}
				}
				else
				{
					// record answer
					Server.Commands.LogHelper lh = new Server.Commands.LogHelper("RTT.log", false, true);
					lh.Log(Server.Commands.LogType.Mobile, m_Mobile, string.Format("FAILED the RTT in {0} ms", diff.TotalMilliseconds));
					lh.Finish();

					m_Mobile.SendMessage("You have failed the AFK test.");
					((PlayerMobile)m_Mobile).RTTResult(false);

					if (CoreAI.IsDynamicFeatureSet(CoreAI.FeatureBits.RTTNotifyEnabled))
					{
						Server.Commands.CommandHandlers.BroadcastMessage(AccessLevel.Counselor,
						0x482,
						String.Format("{0}({1}) has failed the RTT. ({2})", m_Mobile.Name, m_Mobile.Serial, this.m_strNotify));
					}
				}
			}
			catch (Exception e)
			{
				Server.Commands.LogHelper.LogException(e);
			}
		}
	}
}
