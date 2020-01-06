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

using System;
using Server.Network;
using Server.Prompts;
using Server.Guilds;
using Server.Multis;
using Server.Regions;

namespace Server.Items
{
	public class GuildTeleporter : Item
	{
		private Item m_Stone;

		public override int LabelNumber { get { return 1041054; } } // guildstone teleporter

		[Constructable]
		public GuildTeleporter()
			: this(null)
		{
		}

		public GuildTeleporter(Item stone)
			: base(0x1869)
		{
			Weight = 1.0;
			LootType = LootType.Blessed;

			m_Stone = stone;
		}

		public GuildTeleporter(Serial serial)
			: base(serial)
		{
		}

		public override bool DisplayLootType { get { return false; } }

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Stone);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			LootType = LootType.Blessed;

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Stone = reader.ReadItem();

						break;
					}
			}

			if (Weight == 0.0)
				Weight = 1.0;
		}

		public override void OnDoubleClick(Mobile from)
		{
			Guildstone stone = m_Stone as Guildstone;

			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			}
			else if (stone == null || stone.Deleted || stone.Guild == null || stone.Guild.Teleporter != this)
			{
				from.SendLocalizedMessage(501197); // This teleporting object can not determine what guildstone to teleport
			}
			else
			{
				BaseHouse house = BaseHouse.FindHouseAt(from);

				if (house == null)
				{
					from.SendLocalizedMessage(501138); // You can only place a guildstone in a house.
				}
				else if (!house.IsOwner(from))
				{
					from.SendLocalizedMessage(501141); // You can only place a guildstone in a house you own!
				}
				else
				{
					m_Stone.MoveToWorld(from.Location, from.Map);
					Delete();
					stone.Guild.Teleporter = null;
				}
			}
		}
	}
}