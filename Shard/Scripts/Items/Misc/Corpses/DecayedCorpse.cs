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

/* Scripts/Items/Misc/Corpses/DecayedCorpse.cs
 * ChangeLog
 *	2/17/05, mith
 *		Changed inheritance from Container to BaseContainer to fix ownership bugs in 1.0.0
 */

using System;
using Server;

namespace Server.Items
{
	public class DecayedCorpse : BaseContainer
	{
		private Timer m_DecayTimer;
		private DateTime m_DecayTime;

		private static TimeSpan m_DefaultDecayTime = TimeSpan.FromMinutes(7.0);

		public override int DefaultGumpID { get { return 0x9; } }
		public override int DefaultDropSound { get { return 0x42; } }

		public override Rectangle2D Bounds
		{
			get { return new Rectangle2D(20, 85, 104, 111); }
		}

		public DecayedCorpse(string name)
			: base(Utility.Random(0xECA, 9))
		{
			Movable = false;
			Name = name;

			BeginDecay(m_DefaultDecayTime);
		}

		public void BeginDecay(TimeSpan delay)
		{
			if (m_DecayTimer != null)
				m_DecayTimer.Stop();

			m_DecayTime = DateTime.Now + delay;

			m_DecayTimer = new InternalTimer(this, delay);
			m_DecayTimer.Start();
		}

		public override void OnAfterDelete()
		{
			if (m_DecayTimer != null)
				m_DecayTimer.Stop();

			m_DecayTimer = null;
		}

		private class InternalTimer : Timer
		{
			private DecayedCorpse m_Corpse;

			public InternalTimer(DecayedCorpse c, TimeSpan delay)
				: base(delay)
			{
				m_Corpse = c;
				Priority = TimerPriority.FiveSeconds;
			}

			protected override void OnTick()
			{
				m_Corpse.Delete();
			}
		}

		// Do not display (x items, y stones)
		public override bool CheckContentDisplay(Mobile from)
		{
			return false;
		}

		// Do not display (x items, y stones)
		public override bool DisplaysContent { get { return false; } }

		public override void AddNameProperty(ObjectPropertyList list)
		{
			list.Add(1046414, Name); // the remains of ~1_NAME~
		}

		public override void OnSingleClick(Mobile from)
		{
			this.LabelTo(from, 1046414, Name); // the remains of ~1_NAME~
		}

		public DecayedCorpse(Serial serial)
			: base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)1); // version

			writer.Write(m_DecayTimer != null);

			if (m_DecayTimer != null)
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
						BeginDecay(m_DefaultDecayTime);

						break;
					}
				case 1:
					{
						if (reader.ReadBool())
							BeginDecay(reader.ReadDeltaTime() - DateTime.Now);

						break;
					}
			}
		}
	}
}