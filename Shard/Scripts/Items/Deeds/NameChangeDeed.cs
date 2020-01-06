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

/* Items/Deeds/NameChangeDeed.cs
 * ChangeLog:
 *	8/26/07, Adam
 *		Double check that a name change is in your backpack when it is applied, this stops
 *		Players from getting the rename prompt and handing the deed to a friend.
 *	2/23/05, erlein
 *		Changed output format of log entry to include date/time of change.
 *  2/15/05, erlein
 *		Altered so is automatic.. added prompt which includes warning,
 *		regular expression to check for weird chars, NameVerification
 *		validate check and log entry to track name changes ("logs/namechange.log").
 *  8/27/04, Adam
 *		Add message when double clicked.
 */

using System;
using System.IO;
using System.Text.RegularExpressions;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Misc;

namespace Server.Items
{
	public class NameChangeDeed : Item
	{
		[Constructable]
		public NameChangeDeed()
			: base(0x14F0)
		{
			base.Weight = 1.0;
			base.Name = "a name change deed";
		}

		public NameChangeDeed(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}

		public override void OnDoubleClick(Mobile from)
		{
			// Make sure is in pack
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);  // Must be in pack to use!!
				return;
			}

			// Do namechange
			from.SendMessage("Please choose your new name. There will be no refunds for a poorly selected name.");
			from.Prompt = new RenamePrompt(from, this);

		}

		private class RenamePrompt : Prompt
		{
			private Mobile m_from;
			private NameChangeDeed m_ncdeed;

			public RenamePrompt(Mobile from, NameChangeDeed ncdeed)
			{
				m_from = from;
				m_ncdeed = ncdeed;
			}

			public override void OnResponse(Mobile from, string text)
			{

				// Pattern match for invalid characters
				Regex InvalidPatt = new Regex("[^-a-zA-Z0-9' ]");

				if (m_ncdeed == null || m_ncdeed.Deleted == true)
				{
					// error
					from.SendLocalizedMessage(1042001);  // Must be in pack to use!!
				}
				// Make sure is in pack (still)
				else if (!m_ncdeed.IsChildOf(from.Backpack))
				{
					from.SendLocalizedMessage(1042001);  // Must be in pack to use!!
				}
				else if (InvalidPatt.IsMatch(text))
				{
					// Invalid chars
					from.SendMessage("You may only use numbers, letters, apostrophes, hyphens and spaces in your name.");
				}
				else if (!NameVerification.Validate(text, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote))
				{
					// Invalid for some other reason
					from.SendMessage("That name is not allowed here.");
				}
				else
				{

					// Log change
					try
					{

						StreamWriter LogFile = new StreamWriter("logs/namechange.log", true);
						LogFile.WriteLine("{0}: {1},{2},{3}", DateTime.Now, from.Account, from.Name, text);
						LogFile.Close();

					}
					catch
					{
					}

					// Make the change
					from.Name = text;
					from.SendMessage("You have successfully changed your name to {0}.", text);

					// Destroy deed
					m_ncdeed.Delete();
				}
			}
		}
	}
}