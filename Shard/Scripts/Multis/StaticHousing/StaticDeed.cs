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
/* Scripts/Multis/StaticHousing/StaticDeed.cs
 *  Changelog:
 *	8/12/07, Adam
 *		change HouseID read-level access from Admin to GM
 *	6/11/07, Pix
 *		Changed constructor to have the Name set based on the ID.
 *		This is needed so that the deed is displayed correctly in the vendor's list.
 *	06/08/2007, plasma
 *		Initial creation
 * 
 */
using System;
using Server;
using Server.Multis;
using Server.Multis.Deeds;
using Server.Items;
namespace Server.Multis.StaticHousing
{
	public class StaticDeed : HouseDeed
	{
		private string m_HouseID = String.Empty;
		public override int Price { get { return StaticHouseHelper.GetPrice(this.HouseID); } }

		[CommandProperty(AccessLevel.GameMaster, AccessLevel.Administrator)]
		public string HouseID
		{
			get { return m_HouseID; }
			set
			{
				int temp = StaticHouseHelper.GetFoundationID(value);
				if (temp != 0)
				{
					m_HouseID = value;
					base.MultiID = temp;
				}
			}
		}

		public StaticDeed(string houseID, string description)
			: base(0x14F0, StaticHouseHelper.GetFoundationID(houseID), new Point3D(0, 4, 0))
		{
			Weight = 1.0;
			LootType = LootType.Newbied;
			m_HouseID = houseID;
			Name = "deed to a " + description;
		}

		[Constructable]
		public StaticDeed()
			: this(null, "static house")
		{
		}

		[Constructable]
		public StaticDeed(string houseID)
			: this(houseID, "static house")
		{
		}

		public override BaseHouse GetHouse(Mobile owner)
		{
			return new StaticHouse(owner, m_HouseID);
		}
		public StaticDeed(Serial serial)
			: base(serial)
		{
		}

		public override int LabelNumber { get { return 1041211; } }
		public override Rectangle2D[] Area { get { return null; } }

		public override void OnDoubleClick(Mobile from)
		{
			if (!IsChildOf(from.Backpack))
				from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
			else if (m_HouseID == null)
				from.SendMessage("House ID is invalid! ");
			else if (m_HouseID != string.Empty && StaticHouseHelper.IsBlueprintInFile(m_HouseID))
				from.Target = new HousePlacementTarget(this);
			else
				from.SendMessage("House ID is invalid! " + m_HouseID.ToString());
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
			writer.Write(m_HouseID);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					m_HouseID = reader.ReadString();
					break;
			}
		}
	}
}