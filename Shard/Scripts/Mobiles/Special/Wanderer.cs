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
using Server;

namespace Server.Mobiles
{
	public class Wanderer : Mobile
	{
		private Timer m_Timer;

		[Constructable]
		public Wanderer()
		{
			this.Name = "Me";
			this.Body = 0x1;
			this.AccessLevel = AccessLevel.GameMaster;

			m_Timer = new InternalTimer(this);
			m_Timer.Start();
		}

		public Wanderer(Serial serial)
			: base(serial)
		{
			m_Timer = new InternalTimer(this);
			m_Timer.Start();
		}

		public override void OnDelete()
		{
			m_Timer.Stop();

			base.OnDelete();
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
		}

		private class InternalTimer : Timer
		{
			private Wanderer m_Owner;
			private int m_Count = 0;

			public InternalTimer(Wanderer owner)
				: base(TimeSpan.FromSeconds(0.1), TimeSpan.FromSeconds(0.1))
			{
				m_Owner = owner;
			}

			protected override void OnTick()
			{
				if ((m_Count++ & 0x3) == 0)
				{
					m_Owner.Direction = (Direction)(Utility.Random(8) | 0x80);
				}

				m_Owner.Move(m_Owner.Direction);
			}
		}
	}
}