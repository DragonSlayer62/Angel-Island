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

/* Scripts/Multis/Tent/SiegeTent.cs
 * ChangeLog:
 *	08/14/06, weaver
 *		Modified component construction to pass BaseHouse reference to the backpack.
 *	05/22/06, weaver
 *		Added initial 24 hour decay time.
 *		Added overrides to disable refreshing.
 *		Set default price to 0
 *	05/18/06, weaver
 *		Initial creation. 
 */

using System;
using Server;
using Server.Items;
using Server.Gumps;
using Server.Multis.Deeds;

namespace Server.Multis
{

	public class SiegeTent : BaseHouse
	{
		public static Rectangle2D[] AreaArray = new Rectangle2D[] { new Rectangle2D(-3, -3, 7, 7), new Rectangle2D(-1, 4, 3, 1) };
		public override Rectangle2D[] Area { get { return AreaArray; } }
		public override int DefaultPrice { get { return 0; } }
		public override HousePlacementEntry ConvertEntry { get { return HousePlacementEntry.TwoStoryFoundations[0]; } }

		private TentBedRoll m_TentBed;
		private TentBackpack m_TentPack;

		public TentBackpack TentPack
		{
			get
			{
				return m_TentPack;
			}
		}

		public TentBedRoll TentBed
		{
			get
			{
				return m_TentBed;
			}
		}

		private int m_RoofHue;

		public SiegeTent(Mobile owner, int Hue)
			: base(0xFFE, owner, 270, 2, 2)
		{
			m_RoofHue = Hue;

			// wea: this gets called after the base class, overriding it 
			DecayMinutesStored = 60 * 24;   // 24 hours!!
		}

		public SiegeTent(Serial serial)
			: base(serial)
		{
		}

		public void GenerateTent()
		{
			TentWalls walls = new TentWalls(TentStyle.Siege);
			TentRoof roof = new TentRoof(m_RoofHue);
			TentFloor floor = new TentFloor();

			walls.MoveToWorld(this.Location, this.Map);
			roof.MoveToWorld(this.Location, this.Map);
			floor.MoveToWorld(this.Location, this.Map);

			Addons.Add(walls);
			Addons.Add(roof);
			Addons.Add(floor);

			// Create tent bed
			m_TentBed = new TentBedRoll(this);
			m_TentBed.MoveToWorld(new Point3D(this.X, this.Y + 1, this.Z), this.Map);
			m_TentBed.Movable = false;

			// Create secute tent pack within the tent
			m_TentPack = new TentBackpack(this);
			m_TentPack.MoveToWorld(new Point3D(this.X - 1, this.Y - 1, this.Z), this.Map);
			SecureInfo info = new SecureInfo((Container)m_TentPack, SecureLevel.Anyone);
			m_TentPack.IsSecure = true;
			this.Secures.Add(info);
			m_TentPack.Movable = false;
			m_TentPack.Hue = m_RoofHue;
		}

		public override void MoveToWorld(Point3D location, Map map)
		{
			base.MoveToWorld(location, map);
			GenerateTent();
		}

		public override void OnDelete()
		{
			m_TentBed.Delete();
			m_TentPack.Delete();
			base.OnDelete();
		}

		public override HouseDeed GetDeed()
		{
			return new SiegeTentBag();
		}

		// Override standard decay handling so no refresh takes place

		public override void Refresh()
		{
			return;
		}

		public override void RefreshHouseOneDay()
		{
			return;
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);//version

			writer.Write(m_TentBed);
			writer.Write(m_TentPack);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();

			m_TentBed = (TentBedRoll)reader.ReadItem();
			m_TentPack = (TentBackpack)reader.ReadItem();
		}
	}
}
