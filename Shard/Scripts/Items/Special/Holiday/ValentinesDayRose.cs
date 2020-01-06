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

/* Scripts\Items\Special\Holiday\ValentinesDayRose.cs
 * ChangeLog:
 * 12/18/06 Adam
 *		Initial Creation 	
 */

using System;
using System.Text.RegularExpressions;
using Server;
using Server.Items;
using Server.Misc;
using Server.Mobiles;
using Server.Targeting;
using Server.Prompts;

namespace Server.Items
{
	public class ValentinesDayRose : Item // Create the item class which is derived from the base item class
	{
		private bool m_personalized = false;
		public bool Personalized
		{
			get { return m_personalized; }
			set { m_personalized = value; }
		}

		[Constructable]
		public ValentinesDayRose()
			// ugly, but an easy way to code 'you get the cheap red roses way more often than the neat-o purple ones'
			: base(Utility.RandomList(9035, 9036, 9037, 6377, 6378, 6377, 6378, 6377, 6378, 6377, 6378, 6377, 6378, 6377, 6378))
		{
			Name = "a rose";
			Weight = 1.0;
			Hue = 0;
			LootType = LootType.Newbied;
		}

		public ValentinesDayRose(Serial serial)
			: base(serial)
		{
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
			if (Personalized == true)
				from.SendMessage("That rose has already been inscribed.");
			else
			{
				from.SendMessage("What dost thou wish to inscribe?");
				from.Prompt = new RenamePrompt(from, this);
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_personalized);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
			m_personalized = reader.ReadBool();
		}

		private class RenamePrompt : Prompt
		{
			private Mobile m_from;
			ValentinesDayRose m_rose;

			public RenamePrompt(Mobile from, ValentinesDayRose rose)
			{
				m_from = from;
				m_rose = rose;
			}

			public override void OnResponse(Mobile from, string text)
			{
				char[] exceptions = new char[] { ' ', '-', '.', '\'', ':', ',' };
				Regex InvalidPatt = new Regex("[^-a-zA-Z0-9':, ]");
				if (InvalidPatt.IsMatch(text))
				{
					// Invalid chars
					from.SendMessage("You may only use numbers, letters, apostrophes, hyphens, colons, commas, and spaces in the inscription.");

				}
				else if (!NameVerification.Validate(text, 2, 32, true, true, true, 4, exceptions, NameVerification.BuildList(true, true, false, true)))
				{
					// Invalid for some other reason
					from.SendMessage("That inscription is not allowed here.");
				}
				else
				{
					m_rose.Name = text;
					m_rose.Personalized = true;
					from.SendMessage("Thou hast successfully inscribed thy rose.");
				}
			}
		}
	}
}


