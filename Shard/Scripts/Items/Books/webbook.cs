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
/* Scripts/Items/Books/webbook.cs
   Created 12/14/04, Pigpen 
 */

using System;
using System.Text;
using Server.Gumps;
using Server.Network;
using Server.Items;

namespace Server.Gumps
{
	public class WebBook : Item
	{
		private string m_Description;
		private string m_URL;
		private DateTime lastused = DateTime.Now;
		private TimeSpan delay = TimeSpan.FromSeconds(5);

		[CommandProperty(AccessLevel.GameMaster)]
		public string Description
		{
			get
			{
				return m_Description;
			}
			set
			{
				m_Description = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string URL
		{
			get
			{
				return m_URL;
			}
			set
			{
				m_URL = value;
				InvalidateProperties();
			}
		}

		[Constructable]
		public WebBook()
			: base(0xFF4)
		{
			Movable = false;
			Hue = 1170;
			Name = "web book";
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (lastused + delay > DateTime.Now)
			{
				from.SendMessage("Your request is already being processed. Please wait 5 seconds between uses.");
				return;
			}
			else
			{
				lastused = DateTime.Now;
				from.LaunchBrowser(m_URL);
			}
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_Description != null && m_Description.Length > 0)
				LabelTo(from, m_Description);

			base.OnSingleClick(from);
		}

		public WebBook(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_Description);
			writer.Write(m_URL);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Description = reader.ReadString();
						m_URL = reader.ReadString();

						break;
					}
			}
		}
	}
}