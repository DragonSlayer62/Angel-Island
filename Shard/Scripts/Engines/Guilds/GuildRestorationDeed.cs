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

/* Engines/Guilds/GuildRestorationDeed.cs
 * ChangeLog:
 *  2/26/07, Adam
 *      Save a static copy of the guildstone serial. When we cannot find the guildstone OnDoubleClick
 *          we can correlate it with the new "GuildDisband.log" file.
 *  2/7/07, Adam
 *      Check IsFreezeDrying in OnDelete so as to not delete the guild stone when we are getting freeze dried
 *  2/6/07, Adam
 *      Add a Guildstone property to return the serial of the linked stone
 *	2/05/07, Adam
 *      - Make use of new MoveItemToIntStorage, and RetrieveItemFromIntStorage for those times when a guild stone is moved.
 *      - Add OnDelete() override to delete the guildstone if the deed is deleted
 *	2/05/07, Pix
 *		Redesigned so that the deed now takes a Guild.  This makes it so anyone can place
 *		the guildstone if they have the deed (and they pass the normal requirements), instead
 *		of just the person who deeded the stone.
 *	2/10/06, Adam
 *		Created file
 */

using System;
using Server;
using Server.Guilds;
using Server.Network;
using Server.Prompts;
using Server.Items;
using Server.Multis;
using Server.Commands;

namespace Server.Items
{
	public class GuildRestorationDeed : Item
	{
		Guild m_Guild = null;
		Serial m_Serial = 0x0;
		public Guild Guild { get { return m_Guild; } set { m_Guild = value; } }

		[Constructable]
		public GuildRestorationDeed()
			: base(0x14F0)
		{
			Weight = 1.0;
			Name = "a guild restoration deed";

			// we hate newbied things, but we do not wish the guild now being carried on the player
			//	to become orphaned. However there is an undocumented magical phrase:
			//	"i wish to place my guild stone"
			LootType = LootType.Newbied;
		}

		public GuildRestorationDeed(Guild guild)
			: this()
		{
			m_Guild = guild;
			m_Serial = guild.Guildstone.Serial;
			Name = "a guild restoration deed for " + m_Guild.Abbreviation;
		}

		public GuildRestorationDeed(Serial serial)
			: base(serial)
		{
		}

		[CommandProperty(AccessLevel.Counselor)]
		public string GuildAbbr
		{
			get
			{
				if (m_Guild != null && m_Guild.Abbreviation != null)
					return m_Guild.Abbreviation;
				else
					return null;
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public Serial Guildstone
		{
			get
			{
				if (m_Guild != null && m_Guild.Guildstone != null)
					return m_Guild.Guildstone.Serial;
				else
					return (Serial)0;
			}
		}

		[CommandProperty(AccessLevel.Counselor)]
		public Serial StoneSerial
		{
			get
			{
				return (Serial)m_Serial;
			}
		}

		public override void OnDelete()
		{
			// if the deed is deleted, the stone is deleted
			if (IsFreezeDrying == false && m_Guild != null && m_Guild.Guildstone != null)
				m_Guild.Guildstone.Delete();

			base.OnDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)2); // version

			//version 2 additions
			writer.Write(m_Serial);

			//version 1 additions:
			writer.Write(m_Guild);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 2:
					m_Serial = reader.ReadInt();
					goto case 1;
				case 1:
					m_Guild = reader.ReadGuild() as Guild;
					goto case 0;
				case 0:
					break;
			}
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (from.Alive == false && from.AccessLevel <= AccessLevel.Player)
			{
				from.SendMessage("You are dead and cannot do that.");
			}
			else if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); //This must be in your backpack
			}
			else
			{
				if (m_Guild != null && m_Guild.Guildstone != null && m_Guild.Guildstone is Guildstone)
				{
					Guildstone stone = m_Guild.Guildstone as Guildstone;
					if (stone.PlaceStone(from))
					{
						from.SendMessage("You place the guildstone.");
						m_Guild = null; // you need to clear this else you delete your guildstone!
						this.Delete();
					}
					else
					{
						from.SendMessage("You cannot place this stone here.");
					}
				}
				else
				{
					from.SendMessage("The guildstone cannot be found.");
					try
					{
						throw new ApplicationException(String.Format("The Guildstone {1} for GuildRestorationDeed {0} is missing. Check GuildDisbanded.log.", this.Serial, this.m_Serial));
					}
					catch (Exception exp)
					{
						LogHelper.LogException(exp);
						this.m_Guild = null;    // you need to clear this else you delete your guildstone!
						this.Delete();          // no sense of leaving this around
					}
				}
			}
		}
	}
}


