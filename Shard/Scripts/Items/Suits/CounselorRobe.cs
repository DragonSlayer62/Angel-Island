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

/* Scripts/Items/Suits/CounselorRobe.cs
 * ChangeLog
 *  7/21/06, Rhiannon
 *		Added appropriate access levels to Reporter and FightBroker robes
 *	6/27/06, Adam
 *		- Add Reporter Robes and Fight Broker Robes
 *		- (re)set the player mobile titles automatically
 *		- clear fame and karma (titles)
 *	4/17/06, Adam
 *		Explicitly set name
 *	11/07/04, Jade
 *		Changed hue to 0x66.
 */

using System;
using Server;

namespace Server.Items
{
	public class CounselorRobe : BaseSuit
	{
		private const int m_hue = 0x66;	// Counselor blue

		[Constructable]
		public CounselorRobe()
			: base(AccessLevel.Counselor, m_hue, 0x204F)
		{
			Name = "Counselor Robe";
		}

		public CounselorRobe(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (base.OnEquip(from) == true)
			{
				from.Title = null;
				from.Fame = 0;
				from.Karma = 0;
				return true;
			}
			else return false;
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

			if (Hue != m_hue)
				Hue = m_hue;
		}
	}

	public class FightBrokerRobe : BaseSuit
	{
		private const int m_hue = 0x8AB;	// Valorite hue - Fight Broker hue

		[Constructable]
		public FightBrokerRobe()
			: base(AccessLevel.FightBroker, m_hue, 0x204F)
		{
			Name = "Fight Broker Robe";
		}

		public FightBrokerRobe(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (base.OnEquip(from) == true)
			{
				from.Title = "the fight broker";
				from.Fame = 0;
				from.Karma = 0;
				return true;
			}
			else return false;
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

			if (Hue != m_hue)
				Hue = m_hue;
		}
	}

	public class ReporterRobe : BaseSuit
	{
		private const int m_hue = 0x979;	// Agapite hue - Reporter color

		[Constructable]
		public ReporterRobe()
			: base(AccessLevel.Reporter, m_hue, 0x204F)
		{
			Name = "Reporter Robe";
		}

		public ReporterRobe(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (base.OnEquip(from) == true)
			{
				from.Title = "the reporter";
				from.Fame = 0;
				from.Karma = 0;
				return true;
			}
			else return false;
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

			if (Hue != m_hue)
				Hue = m_hue;
		}
	}
}