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

/* Items/Spcial/Holiday/Christmas/EternalEmbers.cs
 * CHANGELOG:
 * 01/21/06, Kit
 *		Added secure level check for co-owner/owner only to be able to toggle on/off.
 * 12/11/05, Kit
 *		Initial Creation
 */

using System;
using Server.Network;
using Server.Regions;
using Server.Gumps;
using Server.Multis;

namespace Server.Items
{
	public class EternalEmbers : Item
	{
		private bool isLit;
		private SecureLevel m_Level;

		[CommandProperty(AccessLevel.GameMaster)]
		public SecureLevel Level
		{
			get { return m_Level; }
			set { m_Level = value; }
		}

		[Constructable]
		public EternalEmbers()
			: base(0xDE1)
		{
			Stackable = false;
			Weight = 5.0;
			isLit = false;
			Name = "eternal embers";
			m_Level = SecureLevel.CoOwners;

		}

		public EternalEmbers(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			writer.Write((int)m_Level);

			writer.Write(isLit);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 1:
					{
						m_Level = (SecureLevel)reader.ReadInt();
						goto case 0;
					}
				case 0:
					{
						isLit = reader.ReadBool();
						break;
					}
			}
			if (version < 1)
			{
				m_Level = SecureLevel.CoOwners;

			}

		}

		public override void OnSectorActivate()
		{

			base.OnSectorActivate();
			if (isLit)
			{
				this.Light = LightType.Circle300;
			}
		}

		public override void OnSectorDeactivate()
		{
			base.OnSectorDeactivate();
			if (isLit)
			{
				this.Light = LightType.Empty;
			}
		}

		public override void OnAdded(object parent)
		{
			if (isLit)
			{
				isLit = false;
				this.ItemID = 0xDE1;
				this.Light = LightType.Empty;
			}
			base.OnAdded(parent);

		}

		public override void OnMovement(Mobile m, Point3D oldLocation)
		{

			if (isLit)
			{
				isLit = false;
				this.ItemID = 0xDE1;
				this.Light = LightType.Empty;
			}
			base.OnMovement(m, oldLocation);
		}

		public bool CheckAccess(Mobile m)
		{

			BaseHouse house = BaseHouse.FindHouseAt(this);

			return (house != null && house.HasSecureAccess(m, m_Level));
		}

		public override void OnDoubleClick(Mobile from)
		{
			if (!this.IsLockedDown)
			{
				from.SendMessage("this must be locked down to use");
				return;
			}

			if (!CheckAccess(from))
			{
				from.SendMessage("You cannot access this");
				return;

			}

			if (isLit)
			{
				this.ItemID = 0xDE1;
				this.Light = LightType.Empty;
				isLit = false;
			}
			else
			{
				this.ItemID = 0xDE3;
				this.Light = LightType.Circle300;

				isLit = true;
			}

		}
	}
}