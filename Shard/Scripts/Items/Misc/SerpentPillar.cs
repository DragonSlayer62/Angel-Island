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

/* Scripts\Items\Misc\SerpentPillar.cs
 * CHANGELOG:
 *	2/25/11, Adam
 *		check Core.T2A variable to tell us if T2A is available for this shard
 */

using System;
using Server;
using Server.Multis;

namespace Server.Items
{
	public class SerpentPillar : Item
	{
		private bool m_Active;
		private string m_Word;
		private Rectangle2D m_Destination;

		[CommandProperty(AccessLevel.GameMaster)]
		public bool Active
		{
			get { return m_Active; }
			set { m_Active = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public string Word
		{
			get { return m_Word; }
			set { m_Word = value; }
		}

		[CommandProperty(AccessLevel.GameMaster)]
		public Rectangle2D Destination
		{
			get { return m_Destination; }
			set { m_Destination = value; }
		}

		[Constructable]
		public SerpentPillar()
			: this(null, new Rectangle2D(), false)
		{
		}

		public SerpentPillar(string word, Rectangle2D destination)
			: this(word, destination, true)
		{
		}

		public SerpentPillar(string word, Rectangle2D destination, bool active)
			: base(0x233F)
		{
			Movable = false;

			m_Active = active;
			m_Word = word;
			m_Destination = destination;
		}

		public override bool HandlesOnSpeech { get { return true; } }

		public override void OnSpeech(SpeechEventArgs e)
		{
			Mobile from = e.Mobile;

			if (!e.Handled && from.InRange(this, 10) && e.Speech.ToLower() == this.Word)
			{
				BaseBoat boat = BaseBoat.FindBoatAt(from, from.Map);

				if (boat == null)
					return;

				if (!this.Active || !Core.T2A)
				{
					if (boat.TillerMan != null)
						boat.TillerMan.Say(502507); // Ar, Legend has it that these pillars are inactive! No man knows how it might be undone!

					return;
				}

				Map map = from.Map;

				for (int i = 0; i < 5; i++) // Try 5 times
				{
					int x = Utility.Random(Destination.X, Destination.Width);
					int y = Utility.Random(Destination.Y, Destination.Height);
					int z = map.GetAverageZ(x, y);

					Point3D dest = new Point3D(x, y, z);

					if (boat.CanFit(dest, map, boat.ItemID))
					{
						int xOffset = x - boat.X;
						int yOffset = y - boat.Y;
						int zOffset = z - boat.Z;

						boat.Teleport(xOffset, yOffset, zOffset);

						return;
					}
				}

				if (boat.TillerMan != null)
					boat.TillerMan.Say(502508); // Ar, I refuse to take that matey through here!
			}
		}

		public SerpentPillar(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.WriteEncodedInt(0); // version

			writer.Write((bool)m_Active);
			writer.Write((string)m_Word);
			writer.Write((Rectangle2D)m_Destination);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadEncodedInt();

			m_Active = reader.ReadBool();
			m_Word = reader.ReadString();
			m_Destination = reader.ReadRect2D();
		}
	}
}