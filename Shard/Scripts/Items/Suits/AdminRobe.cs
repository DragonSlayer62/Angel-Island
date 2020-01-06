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

/* Scripts/Items/Suits/AdminRobe.cs
 * ChangeLog
 *  7/21/06, Rhiannon
 *		Added Owner access level to Godly robe
 *	6/27/06, Adam
 *		- (re)set the player mobile titles automatically if not Godly
 *		- clear fame and karma (titles) if not Godly
 *		- reset color on robe load
 *		- Add Godly version of robes for Adam And Jade
 *	4/17/06, Adam
 *		Explicitly set name
 *	11/07/04, Jade
 *		Changed hue to 0x1.
 */

using System;
using Server;

namespace Server.Items
{
	public class AdminRobe : BaseSuit
	{
		private const int m_hue = 0x0;	// Admin color

		[Constructable]
		public AdminRobe()
			: base(AccessLevel.Administrator, m_hue, 0x204F)
		{
			Name = "Administrator Robe";
		}

		public AdminRobe(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (base.OnEquip(from) == true)
			{
				from.Title = "a shard administrator";
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

	public class GodlyRobe : BaseSuit
	{
		private const int m_hue = 0x1;	// godly colors (Adam and Jade Only)

		[Constructable]
		public GodlyRobe()
			: base(AccessLevel.Owner, m_hue, 0x204F)
		{
			Name = "Godly Robe";
		}

		public GodlyRobe(Serial serial)
			: base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (base.OnEquip(from) == true)
			{	// gods can have any title they want
				// gods can have fame/karma titles as well
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