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

/* Scripts\Engines\ChampionSpawn\Champs\Harrower\Items\StarRoomGate.cs
 * CHANGELOG
 *  01/05/07, plasma
 *      Changed CannedEvil namespace to ChampionSpawn for cleanup!
 *  6/5/04, Pix
 *		Merged in 1.0RC0 code.
 */


using System;
using Server;

namespace Server.Items
{
	public class StarRoomGate : Moongate
	{
		private bool m_Decays;
		private DateTime m_DecayTime;
		private Timer m_Timer;

		public override int LabelNumber { get { return 1049498; } } // dark moongate

		[Constructable]
		public StarRoomGate()
			: this(false)
		{
		}

		[Constructable]
		public StarRoomGate(bool decays, Point3D loc, Map map)
			: this(decays)
		{
			MoveToWorld(loc, map);
			Effects.PlaySound(loc, map, 0x20E);
		}

		[Constructable]
		public StarRoomGate(bool decays)
			: base(new Point3D(5143, 1774, 0), Map.Felucca)
		{
			Dispellable = false;
			ItemID = 0x1FD4;

			if (decays)
			{
				m_Decays = true;
				m_DecayTime = DateTime.Now + TimeSpan.FromMinutes(2.0);

				m_Timer = new InternalTimer(this, m_DecayTime);
				m_Timer.Start();
			}
		}

		public StarRoomGate(Serial serial)
			: base(serial)
		{
		}

		public override void OnAfterDelete()
		{
			if (m_Timer != null)
				m_Timer.Stop();

			base.OnAfterDelete();
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version

			writer.Write(m_Decays);

			if (m_Decays)
				writer.WriteDeltaTime(m_DecayTime);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			switch (version)
			{
				case 0:
					{
						m_Decays = reader.ReadBool();

						if (m_Decays)
						{
							m_DecayTime = reader.ReadDeltaTime();

							m_Timer = new InternalTimer(this, m_DecayTime);
							m_Timer.Start();
						}

						break;
					}
			}
		}

		private class InternalTimer : Timer
		{
			private Item m_Item;

			public InternalTimer(Item item, DateTime end)
				: base(end - DateTime.Now)
			{
				m_Item = item;
			}

			protected override void OnTick()
			{
				m_Item.Delete();
			}
		}
	}
}