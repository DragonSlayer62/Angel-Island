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

/* Scripts/Items/Containers/Strongbox.cs
 * CHANGELOG:
 *	7/7/06, Adam
 *		Move the cleanup routine(Validate()) to Heartbeat like all other standard cleanup
 *	3/12/05: Pixie
 *		Made ownerless strongboxes inaccessible to all players.
 */

using System;
using Server;
using Server.Multis;
using Server.Network;

namespace Server.Items
{
	[FlipableAttribute(0xe80, 0x9a8)]
	public class StrongBox : BaseContainer, IChopable
	{
		private Mobile m_Owner;
		private BaseHouse m_House;

		public override int DefaultGumpID { get { return 0x4B; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(16, 51, 168, 73); }
		}

		public StrongBox(Mobile owner, BaseHouse house)
			: base(0xE80)
		{
			m_Owner = owner;
			m_House = house;

			MaxItems = 25;
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Mobile Owner
		{
			get
			{
				return m_Owner;
			}
			set
			{
				m_Owner = value;
				InvalidateProperties();
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public BaseHouse House
		{
			get
			{
				return m_House;
			}
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public override int MaxWeight
		{
			get
			{
				return 0;
			}
		}

		public StrongBox(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Owner);
			writer.Write(m_House);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Owner = reader.ReadMobile();
						m_House = reader.ReadItem() as BaseHouse;

						break;
					}
			}
		}

		public override bool Decays
		{
			get
			{
				if (m_House != null && m_Owner != null && !m_Owner.Deleted)
					return !m_House.IsCoOwner(m_Owner);
				else
					return true;
			}
		}

		public override TimeSpan DecayTime
		{
			get
			{
				return TimeSpan.FromMinutes(30.0);
			}
		}

		public override void AddNameProperty(ObjectPropertyList list)
		{
			if (m_Owner != null)
				list.Add(1042887, m_Owner.Name); // a strong box owned by ~1_OWNER_NAME~
			else
				base.AddNameProperty(list);
		}

		public override void OnSingleClick(Mobile from)
		{
			if (m_Owner != null)
			{
				LabelTo(from, 1042887, m_Owner.Name); // a strong box owned by ~1_OWNER_NAME~

				if (CheckContentDisplay(from))
					LabelTo(from, "({0} items, {1} stones)", TotalItems, TotalWeight);
			}
			else
			{
				base.OnSingleClick(from);
			}
		}

		public override bool IsAccessibleTo(Mobile m)
		{
			if (m.AccessLevel >= AccessLevel.GameMaster)
			{
				return true;
			}

			if (m_Owner == null || m_Owner.Deleted)
			{
				return false;
			}

			if (m_House == null || m_House.Deleted)
			{
				return true;
			}

			return m == m_Owner && m_House.IsCoOwner(m) && base.IsAccessibleTo(m);
		}

		private void Chop(Mobile from)
		{
			Effects.PlaySound(Location, Map, 0x3B3);
			from.SendLocalizedMessage(500461); // You destroy the item.
			Destroy();
		}

		public void OnChop(Mobile from)
		{
			if (m_House != null && !m_House.Deleted && m_Owner != null && !m_Owner.Deleted)
			{
				if (from == m_Owner || m_House.IsOwner(from))
					Chop(from);
			}
			else
			{
				Chop(from);
			}
		}
	}
}
