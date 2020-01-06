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

/* ChangeLog
 * Scripts/Items/Deeds/NPCNameChangeDeed.cs
 * 
 *  04/05/06 Taran Kain
 *		Made to work on PlayerBarkeepers as well.
 *	3/29/06 Taran Kain
 *		Initial version. Adapted from NPCTitleChangeDeed.
 */

using System;
using System.Text.RegularExpressions;
using Server;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class NpcNameChangeDeedTarget : Target
	{
		private NpcNameChangeDeed m_Deed;

		public NpcNameChangeDeedTarget(NpcNameChangeDeed deed)
			: base(1, false, TargetFlags.None)
		{
			m_Deed = deed;
		}

		protected override void OnTarget(Mobile from, object target)
		{
			if (target is PlayerVendor)
			{
				PlayerVendor vendor = (PlayerVendor)target;
				if (vendor.IsOwner(from))
				{
					from.SendMessage("What dost thou wish to name thy servant?");
					from.Prompt = new RenamePrompt(from, vendor, m_Deed);
				}

				else
				{
					vendor.SayTo(from, "I do not work for thee! Only my master may change my name.");
				}
			}
			else if (target is PlayerBarkeeper)
			{
				PlayerBarkeeper barkeep = (PlayerBarkeeper)target;
				if (barkeep.IsOwner(from))
				{
					from.SendMessage("What dost thou wish to name thy servant?");
					from.Prompt = new RenamePrompt(from, barkeep, m_Deed);
				}

				else
				{
					barkeep.SayTo(from, "I do not work for thee! Only my master may change my name.");
				}
			}
			else
			{
				from.SendMessage("Thou canst only change the names of thy servants.");
			}
		}


		private class RenamePrompt : Prompt
		{
			private Mobile m_from;
			private NpcNameChangeDeed m_deed;
			private Mobile vendor;

			public RenamePrompt(Mobile from, Mobile target, NpcNameChangeDeed deed)
			{
				m_from = from;
				m_deed = deed;
				vendor = target;
			}

			public override void OnResponse(Mobile from, string text)
			{
				Regex InvalidPatt = new Regex("[^-a-zA-Z0-9' ]");
				if (InvalidPatt.IsMatch(text))
				{
					// Invalid chars
					from.SendMessage("You may only use numbers, letters, apostrophes, hyphens and spaces in the name to be changed.");

				}
				else if (!NameVerification.Validate(text, 2, 16, true, true, true, 1, NameVerification.SpaceDashPeriodQuote, NameVerification.BuildList(true, true, false, true)))
				{
					// Invalid for some other reason
					from.SendMessage("That name is not allowed here.");
				}
				else
				{
					vendor.Name = text;
					from.SendMessage("Thou hast successfully changed thy servant's name.");
					m_deed.Delete();
				}
			}
		}
	}


	public class NpcNameChangeDeed : Item
	{
		[Constructable]
		public NpcNameChangeDeed()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "an npc name change deed";
		}

		public NpcNameChangeDeed(Serial serial)
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
			// Make sure deed is in pack
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
				return;
			}

			// Create target and call it
			from.SendMessage("Whose name dost thou wish to change?");
			from.Target = new NpcNameChangeDeedTarget(this);
		}
	}
}
